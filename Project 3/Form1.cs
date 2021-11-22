using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Project_3
{
    public partial class Form1 : Form
    {
        int id = 0;
        string fileSearch = "";
        bool caseSensitive = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxLocation.Items.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                comboBoxLocation.Items.Add(drive.Name);
            }
            comboBoxLocation.Items.Add(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            comboBoxLocation.Items.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            comboBoxLocation.Items.Add("All");
            labelError.Text = "";
            buttonSearch.Enabled = false;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            id = 0;
            dataGrid.Rows.Clear();
            labelError.Text = "";
            fileSearch = textBoxFileName.Text;
            if (!(caseSensitive = checkCaseSensitive.Checked))
            {
                fileSearch = fileSearch.ToLower();
            }
            if (textBoxFileName.Text == "") return;
            dataGrid.Rows.Clear();
            try
            {
                if (comboBoxLocation.SelectedItem.ToString() == "All")
                {
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        displayFiles(drive.Name);
                    }
                }
                else
                {
                    displayFiles(comboBoxLocation.SelectedItem.ToString());
                }
                if (id == 0)
                {
                    labelError.Text = "No Matching File Name Found";
                }
            }
            catch (ArgumentException ex)
            {
                labelError.Text = $"Invalid file path: {ex.Message}";
            }
        }

        public void displayFiles(string path)
        {
            string[] paths;
            try
            {
                paths = GetDirectoriesInFolder(path);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unhandled Exception:\n{ex}");
                return;
            }
            if (paths.Length == 0)
            {
                foreach (string item in GetItemsInFolder(path))
                {
                    //MessageBox.Show(item);
                    ShowFileInfo(item);
                }
                return;
            }
            foreach (string directory in paths)
            {
                displayFiles(directory);
            }
        }
        private string[] GetItemsInFolder(string path)
        {
            return Directory.GetFiles(path);
        }
        private string[] GetDirectoriesInFolder(string path)
        {
            return Directory.GetDirectories(path);
        }
        private void ShowFileInfo(string path)
        {
            FileInfo file = new FileInfo(path);
            if ((caseSensitive ? file.Name : file.Name.ToLower()) == fileSearch)
            {
                dataGrid.Rows.Add(++id, file.Name, formatSize(file.Length), file.FullName);
            }
        }
        private string formatSize(long size)
        {
            long kb, mb, gb;
            if ((kb = size / 1000) > 0)
            {
                if ((mb = size / 1000000) > 0)
                {
                    if ((gb = size / 1000000000) > 0)
                    {
                        return gb + " GB";
                    }
                    return mb + " MB";
                }
                return kb + " KB";
            }
            return size + " bytes";
        }

        private void comboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLocation.SelectedItem == null || textBoxFileName.Text == "")
            {
                buttonSearch.Enabled = false;
                return;
            }
            buttonSearch.Enabled = true;
        }

        private void textBoxFileName_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxLocation.SelectedItem == null || textBoxFileName.Text == "")
            {
                buttonSearch.Enabled = false;
                return;
            }
            buttonSearch.Enabled = true;
        }
    }
}

/*
 * 
    public partial class Form1 : Form
    {
        private int TotalFiles = 0;
        private double totalTime = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //dialog. = "c:\\";
            //dialog.Filter = "All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.SelectedPath;
                //MessageBox.Show(filePath);
                textBoxFilePath.Text = filePath;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (textBoxFilePath.Text == "") return;
            TotalFiles = 0;
            totalTime = 0;
            double epoch = (double)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

            dataGrid.Rows.Clear();
            try
            {
                displayFiles(textBoxFilePath.Text);

                labelTotalFiles.Text = TotalFiles.ToString();

                totalTime = ((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds - epoch);
                //MessageBox.Show(totalTime.ToString());
                //MessageBox.Show(formatTime(totalTime));
                labelSearchTime.Text = formatTime(totalTime);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Invalid file path: {ex}");
            }
        }

        
    }
}
*/