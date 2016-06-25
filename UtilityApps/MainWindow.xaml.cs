using System.Windows;

namespace UtilityApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool excludeMode = true;
        bool deleteMode = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNewDialogue_Click(object sender, RoutedEventArgs e)
        {
            LaunchDialogue(sender);
        }

        private void LaunchDialogue(object sender)
        {
            if (sender == btnCopyFileAndPasteToMany || sender == btnCopyFileAndPasteToMany_ButNotThisOne)
            {
                deleteMode = false;
                excludeMode = !(sender == btnCopyFileAndPasteToMany);
            }
            else if (sender == btnDeleteFileFromMany || sender == btnDeleteFileFromMany_ButNotThisOne)
            {
                deleteMode = true;
                excludeMode = !(sender == btnDeleteFileFromMany);
            }
            try
            {
                CopyOnePasteToMany windowFile = new CopyOnePasteToMany(excludeMode, deleteMode);
                windowFile.ShowDialog();
            }
            catch
            {
                ErrorFailedToOpenWindow();
            }
        }
        private static void ErrorFailedToOpenWindow()
        {
            MessageBox.Show("Failed to load window. Please restart the application.", "Advice", MessageBoxButton.OK);
        }
    }
}