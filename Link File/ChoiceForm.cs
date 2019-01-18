using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Link_File
{
    public partial class ChoiceForm : Form
    {
        //variable to store the file path of selected file
        string filePath = null;

        //list of all the paths from the textboxes
        List<string> filePaths = new List<string>();
                
        public ChoiceForm()
        {
            InitializeComponent();
        }

        //method to return paths of all the files selected. (textbox.text is used because 
        //it can be easily changed for each event and minimizes the number of variables)
        public List<string> GetFileNames()
        {
            if (textBox1.Text != "Choose the File 1")
                filePaths.Add(textBox1.Text);
            if (textBox2.Text != "Choose the File 2")
                filePaths.Add(textBox2.Text);
            if (textBox3.Text != "Choose the File 3")
                filePaths.Add(textBox3.Text);
            if (textBox4.Text != "Choose the File 4")
                filePaths.Add(textBox4.Text);
            if (textBox5.Text != "Choose the File 5")
                filePaths.Add(textBox5.Text);
            return filePaths;
        }

        //check whether any text box has a value
        public bool checker()
        {
            if ((textBox1.Text.Length != 0 && textBox1.Text != "Choose the File 1") || (textBox2.Text.Length != 0 && textBox2.Text != "Choose the File 2")
                || (textBox3.Text.Length != 0 && textBox3.Text != "Choose the File 3") || (textBox4.Text.Length != 0 && textBox4.Text != "Choose the File 4") 
                || (textBox5.Text.Length != 0 && textBox5.Text != "Choose the File 5"))
                return true;
            else
                return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if text property is not 'remove' then assign the filepath to the textbox.text andchange buton.text to 'remove'
            if (button1.Text != "Remove")
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    filePath = openFileDialog1.FileName;
                    if (0 != filePath.Length)
                    {
                        textBox1.Text = filePath;
                        button1.Text = "Remove";
                    }
                }
            }
            //if buton.text is 'remove' change the text property of textbox and enabled property of the button based on the text in the textbox below or above
            else if (button1.Text == "Remove")
            {
                button1.Text = "File 1";
                textBox1.Text = "Choose the File 1";
                button1.Enabled = true;
                if (textBox2.Text == "Choose the File 2")
                {
                    button2.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if text property is not 'remove' then assign the filepath to the textbox.text andchange buton.text to 'remove'
            if (button2.Text != "Remove")
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK && button2.Text != "Remove") // Test result.
                {
                    filePath = openFileDialog1.FileName;
                    if (0 != filePath.Length)
                    {
                        textBox2.Text = filePath;
                        button2.Text = "Remove";
                    }
                }
            }
            //if buton.text is 'remove' change the text property of textbox and enabled property of the button based on the text in the textbox below or above
            else if (button2.Text == "Remove")
            {
                button2.Text = "File 2";
                textBox2.Text = "Choose the File 2";
                button2.Enabled = true;
                if (textBox3.Text == "Choose the File 3")
                {
                    button3.Enabled = false;
                }
                if (textBox1.Text == "Choose the File 1")
                {
                    button2.Enabled = false;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if text property is not 'remove' then assign the filepath to the textbox.text andchange buton.text to 'remove'
            if (button3.Text != "Remove")
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK && button3.Text != "Remove") // Test result.
                {
                    filePath = openFileDialog1.FileName;
                    if (0 != filePath.Length)
                    {
                        textBox3.Text = filePath;
                        button3.Text = "Remove";
                    }
                }
            }
            //if buton.text is 'remove' change the text property of textbox and enabled property of the button based on the text in the textbox below or above
            else if (button3.Text == "Remove")
            {
                button3.Text = "File 3";
                textBox3.Text = "Choose the File 3";
                button3.Enabled = true;
                if (textBox4.Text == "Choose the File 4")
                {
                    button4.Enabled = false;
                }
                if (textBox2.Text == "Choose the File 2")
                {
                    button3.Enabled = false;
                }
                if (textBox1.Text == "Choose the File 1")
                    button3.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if text property is not 'remove' then assign the filepath to the textbox.text andchange buton.text to 'remove'
            if (button4.Text != "Remove")
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK && button4.Text != "Remove") // Test result.
                {
                    filePath = openFileDialog1.FileName;
                    if (0 != filePath.Length)
                    {
                        textBox4.Text = filePath;
                        button4.Text = "Remove";
                    }
                }
            }
            //if buton.text is 'remove' change the text property of textbox and enabled property of the button based on the text in the textbox below or above
            else if (button4.Text == "Remove")
            {
                button4.Text = "File 4";
                textBox4.Text = "Choose the File 4";
                button4.Enabled = true;
                if (textBox5.Text == "Choose the File 5")
                {
                    button5.Enabled = false;
                }
                if (textBox3.Text == "Choose the File 3")
                {
                    button4.Enabled = false;
                }
                if (textBox1.Text == "Choose the File 1")
                    button4.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //if text property is not 'remove' then assign the filepath to the textbox.text andchange buton.text to 'remove'
            if (button5.Text != "Remove")
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK && button5.Text != "Remove") // Test result.
                {
                    filePath = openFileDialog1.FileName;
                    if (0 != filePath.Length)
                    {
                        textBox5.Text = filePath;
                        button5.Text = "Remove";
                    }
                }
            }
            //if buton.text is 'remove' change the text property of textbox and enabled property of the button based on the text in the textbox below or above
            else if (button5.Text == "Remove")
            {
                button5.Text = "File 5";
                textBox5.Text = "Choose the File 5";
                button5.Enabled = true;
                if (textBox4.Text == "Choose the File 4")
                {
                    button5.Enabled = false;
                }
            }
        }


        //methods to change the enabled property of the buttons based on the text property of the previous textboxes
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button4.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }
        
    }
}
