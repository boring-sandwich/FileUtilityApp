using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace UtilityApps
{
    /// <summary>
    /// Interaction logic for CopyOnePasteToMany.xaml
    /// </summary>
    public partial class CopyOnePasteToMany : Window
    {
        private FolderBrowserDialog fldBD = new FolderBrowserDialog();
        private OpenFileDialog file = new OpenFileDialog();


        int whatStep = 1;
        string sourcePath;
        string targetPath;
        string excludePath;

        bool excludeFolderMode;
        bool deleteFileMode;

        public CopyOnePasteToMany(bool excludeMode, bool deleteMode)
        {
            InitializeComponent();
            excludeFolderMode = excludeMode;
            deleteFileMode = deleteMode;
            WhatStep();
            FileWindow.Title = deleteFileMode ? "Delete File From Folders" : "Copy File To Foldlers";
        }
        private void WhatStep()
        {
            switch (whatStep)
            {
                case 1:
                    txbSteps.Text = $"Step {whatStep} of " + (excludeFolderMode ? "4." : "3.");
                    txbInstructions.Text = $"What file do you want to " + (deleteFileMode ? "delete?" : "copy?");
                    HiddenObjects();
                    btnNext.Content = "next";
                    break;
                case 2:
                    txbSteps.Text = $"Step {whatStep} of " + (excludeFolderMode ? "4." : "3.");
                    txbInstructions.Text = $"Where is the main folder?";
                    HiddenObjects();
                    if (!deleteFileMode)
                        cbxUserSelection.Visibility = Visibility.Visible;
                    break;
                case 3:
                    txbSteps.Text = $"Step {whatStep} of " + (excludeFolderMode ? "4." : "3.");
                    txbInstructions.Text = $"What folder do you want to exclude?";
                    txbInstructions.Visibility = Visibility.Visible;
                    HiddenObjects();
                    break;
                case 4:
                    txbSteps.Text = $"Complete.";
                    HiddenObjects();
                    btnNext.Visibility = Visibility.Visible;
                    btnNext.Content = "close";
                    btnBrowse.Visibility = Visibility.Hidden;
                    CopyFile();
                    break;
            }
        }

        private void CopyFile()
        {
            string[] directories = Directory.GetDirectories(targetPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string directory in directories)
            {
                if (directory != excludePath)
                {
                    string destinationPath = Path.Combine(directory, Path.GetFileName(sourcePath));
                    if (cbxUserSelection.IsChecked.Value == true)
                    {
                           File.Copy(sourcePath, destinationPath, true);
                    }
                    else
                    {
                        bool sourceExists = File.Exists(destinationPath);
                        if (deleteFileMode && sourceExists)
                            File.Delete(destinationPath);

                        if (!sourceExists)
                        {
                            if (deleteFileMode)
                                continue;
                            else
                                File.Copy(sourcePath, destinationPath);
                        }
                    }
                }                  
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (whatStep == 1)
            {
                file.ShowDialog();
                sourcePath = file.FileName;
                txbInput.Text = sourcePath;
                VisibleObjects();
            }
            else if (whatStep == 2)
            {
                if (fldBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    targetPath = fldBD.SelectedPath;
                    txbInput.Text = targetPath;
                    VisibleObjects();
                }
            }
            else if (whatStep == 3)
            {
                if (fldBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    excludePath = fldBD.SelectedPath;
                    txbInput.Text = excludePath;
                    VisibleObjects();
                }
            }
        }

        private void VisibleObjects()
        {
            if (whatStep != 1)
            {
                txbInstructions.Visibility = Visibility.Hidden;
            }
            txbInput.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;
        }
        private void HiddenObjects()
        {
            txbInput.Visibility = Visibility.Hidden;
            btnNext.Visibility = Visibility.Hidden;
            cbxUserSelection.Visibility = Visibility.Hidden;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (whatStep == 1)
            {
                whatStep = 2;
            }
            else if(whatStep == 2)
            {
                if (excludeFolderMode)
                {
                    whatStep = 3;
                }
                else
                {
                    whatStep = 4;
                }
            }
            else if (whatStep == 3)
            {
                whatStep = 4;
            }
            else if (whatStep == 4)
            {
                this.Close();
                return;
            }
            WhatStep();
        }
    }
}