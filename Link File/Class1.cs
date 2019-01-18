using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Security_Check;

namespace Link_File
{
    //Transaction assigned as automatic
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    //Creating an external command to link the elements with files
    public class LinkFile : IExternalCommand
    {
        //instances to store application and the document
        UIDocument m_document=null;

        string linkTextFileName = null, tempFileName = null, elementFamily = null;
        string linkDirectoryPath=null, destinationPath=null;

        List<string> linkFilePaths = new List<string>();
        List<string> linkFileNames = new List<string>();

        bool checkResult=true;

        bool security = false;

        //execute method for the IExternalCommand
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //call to the security check method to check for authentication
                security = SecurityLNT.Security_Check();

                if (security == false)
                {
                    return Result.Succeeded;
                }

                //open  the active document in revit
                m_document = commandData.Application.ActiveUIDocument;

                //call to method to get directory and file
                GetDirectoryAndFile();

                //call method to make the file selection and get paths
                FIleSelection();

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            throw new NotImplementedException();
            
        }

           
     
        //method to get directory and file
        private void GetDirectoryAndFile()
        {
            try
            {
                //get the path of the active document in revit
                linkTextFileName = m_document.Document.PathName;

                DirectoryInfo directory = null;

                //convert the active file path into directory name
                if (File.Exists(linkTextFileName))
                {
                    directory = new FileInfo(linkTextFileName).Directory;
                    linkTextFileName = Path.GetFileNameWithoutExtension(linkTextFileName);                    
                }

                //linked files folder path in the project directory
                linkDirectoryPath = directory.ToString() +@"\"+linkTextFileName;

                //check if directory exists
                if (!Directory.Exists(linkDirectoryPath))
                {
                    //create directory
                    System.IO.Directory.CreateDirectory(linkDirectoryPath);
                }

                //create the file in the directory if not existing
                linkTextFileName = linkDirectoryPath + @"\LinkedFiles.txt";
                if (!System.IO.File.Exists(linkTextFileName))
                {
                    System.IO.File.CreateText(linkTextFileName);
                }
            }
            catch
            {
                return;
            }
            
        }



        //method for choosing the files to be linked with elements
        private void FIleSelection()
        {
            bool loopContinue=false;
            do
            {
                try
                {
                    //create instance of the form
                    using (ChoiceForm formInstance = new ChoiceForm())
                    {
                        //loop works till the user gives an input in any of the fields
                        while (checkResult)
                        {
                            DialogResult dialogueResult = formInstance.ShowDialog();

                            if (DialogResult.OK == dialogueResult)
                            {
                                //check whether the textboxes are empty or not
                                checkResult = formInstance.checker();

                                if (checkResult)
                                {
                                    //get the file paths of the selected files
                                    linkFilePaths = formInstance.GetFileNames();

                                    //copy the files into the Linked Files folder in the project directory
                                    //add 'Linked Files' part to the file name extracted from the 
                                    //path to make it eparate from the family names in the file
                                    foreach (string filePath in linkFilePaths)
                                    {
                                        tempFileName = Path.GetFileName(filePath);
                                        destinationPath = linkDirectoryPath + @"\" + tempFileName;
                                        System.IO.File.Copy(filePath, destinationPath, true);
                                        linkFileNames.Add(@"Linked Files\" + tempFileName);
                                    }

                                    //call the get element method and prompt the user to select the element
                                    GetElement();
                                    //write the data to the file
                                    WriteToFile();
                                    checkResult = false;
                                }
                                else
                                {
                                    MessageBox.Show("Select the file and press OK.");
                                }
                            }
                            else if (DialogResult.Cancel == dialogueResult)
                                checkResult = false;
                        }
                        //explicitly close the file
                        formInstance.Close();
                    }
                }
                catch
                {
                    loopContinue = false;
                }                                

                //pop up a message box which asks the user whether he wants to add more files
                DialogResult dialogResult = MessageBox.Show("Do you want to link files with more Families?"
                    , "Continue?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    linkFileNames.Clear();
                    linkFilePaths.Clear();
                    checkResult = true;
                    loopContinue = true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    loopContinue = false;
                }

            } while (loopContinue == true);
            
        }



        // method for user to pick the elements
        private void GetElement()
        {
            try
            {
                Element element = null;
                Reference elementReference = null;

                //prompt the user to make the slection and convert the reference into element
                elementReference = m_document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element
                    , new SelectionFilter(m_document.Document), "Pick the element to be linked with the family.");
                element = m_document.Document.GetElement(elementReference);

                //get the element type from the selected element
                element = m_document.Document.GetElement(element.GetTypeId());

                elementFamily = element.Name;
            }
            catch
            {
                return;
            }
            
        }



        //method to write all data to file
        private void WriteToFile()
        {
            bool familyExists = false;
            string line = null;
            List<string> foundFamily = new List<string>();

            try
            {      
                //start a new file reader
                System.IO.StreamReader file = new System.IO.StreamReader(linkTextFileName);

                //check whether the file contains the currently selected family or not
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(elementFamily))
                    {
                        familyExists = true;
                        break;
                    }
                }
                //close the file
                file.Close();
            
                //if the family doesnt exist at present, just write the data to the 
                //file after ading the family name at the beginning of the list
                if (familyExists == false)
                {
                    linkFileNames.Insert(0, elementFamily);
                    File.AppendAllLines(linkTextFileName, linkFileNames);
                }
                //if the family exists in the file already do the following
                else
                {
                    //read all the contents of the file intoa string array
                    string[] fileContents = File.ReadAllLines(linkTextFileName);

                    foundFamily.Clear();

                    for (int i = 0; i < fileContents.Count(); i++)
                    {
                        //find where the family is repeated in the string array
                        if (fileContents[i].Contains(elementFamily))
                        {
                            //from the above detected location onwards copy the filenames into another list                       
                            for (int j = i+1; j < fileContents.Count(); j++)
                            {
                                if (fileContents[j].Contains("Linked Files"))
                                    foundFamily.Add(fileContents[j]);
                                else
                                    break;
                            }

                            //LINKFILENAMES LIST CONTAINS ALL THE NEW FILES TO BE LINKED WITH THE FAMILY
                            //add to the linkFileNames list's end all the elements in the foundFamily list 
                            //which is not included in the linkFileName list
                            linkFileNames.AddRange(foundFamily.Except(linkFileNames).ToList());
                            //add the family name at the beginning of the list
                            linkFileNames.Insert(0, elementFamily);
                            //add family name to the found family list begining
                            foundFamily.Insert(0, elementFamily);
                        }
                    }

                    //change all the contents of the fileContents list into a list 
                    //which excludes all the file names in familyfound list
                    //FILECONTAINS LIST= FILECONTENTS LIST-(COMMON ELEMENTS IN FILECONTENTS LIST AND FOUNDFAMILY LIST)
                    fileContents = fileContents.ToList().Except(foundFamily).ToArray();

                    //add filecontents into the end of linkfilenames list
                    linkFileNames.AddRange(fileContents);

                    //overwrite all the contents in linktextfilename with data in linkfilenames list
                    File.WriteAllLines(linkTextFileName, linkFileNames);
                }
            }
            catch
            {
                return;
            }           

        }
    }
        
}
