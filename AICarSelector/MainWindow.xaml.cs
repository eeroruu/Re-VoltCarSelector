using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace AICarSelector
{
    public partial class MainWindow : Window
    {
        // List containing all possible folders
        private List<string> carFolders = new List<string>();
        // List containing folders currently chosen for AI
        private List<string> carsForAI = new List<string>();

        public MainWindow()
        {            
            Setup();
            InitializeComponent();
            UpdateFolderCounts();
        }

        private void Setup()
        {
            // Check that a separate directory for AI cars exists
            string root = "./carsForAI";
            if (!Directory.Exists(root)){
                // currently folder will be created to AICarSelector/bin/Debug/net6.0windows
                Directory.CreateDirectory(root);
            }
            //currently folder AICarSelector/bin/Debug/net6.0windows/cars contains 29 example empty folders
            foreach (string dirName in Directory.GetDirectories("./cars"))
            {

                carFolders.Add(dirName.Split(@"\")[1]);
            }
            //Check how many already have been chosen
            foreach (string dirName in Directory.GetDirectories("./carsForAI"))
            {
                carsForAI.Add(dirName.Split(@"\")[1]);
            }
        }

        //Text values that update when folder counts change
        public string CarFolderCount
        {
            get { return (string)GetValue(FullFolderCount); }
            set { SetValue(FullFolderCount, value); }
        }

        public static readonly DependencyProperty FullFolderCount =
        DependencyProperty.Register("CarFolderCount", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public string AICarFolderCount
        {
            get { return (string)GetValue(AIFolderCount); }
            set { SetValue(AIFolderCount, value); }
        }

        public static readonly DependencyProperty AIFolderCount =
        DependencyProperty.Register("AICarFolderCount", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public void UpdateFolderCounts()
        {
            CarFolderCount = carFolders.Count.ToString();
            AICarFolderCount = carsForAI.Count.ToString();
        }

        // Add folder to the AI directory
        private void AddFolderForAI(object sender, RoutedEventArgs e)
        {
            // Check that input isn't empty
            if (folderToAdd.Text.Length == 0)
            {
                MessageBox.Show("Error: Empty input not accepted. Give an input first");
            }
            // Check that the folder isn't already there
            else if (carsForAI.Contains(folderToAdd.Text))
            {
                MessageBox.Show("Error: Folder already exists. Try a different input!");
            }
            // Check that the folder exists in the original directory
            else if (!carFolders.Contains(folderToAdd.Text))
            {
                MessageBox.Show("Error: Could not find folder. Try a different input!");
            }
            else
            {
                string root = "./carsForAI/" + folderToAdd.Text;
                Directory.CreateDirectory(root);
                carsForAI.Add(folderToAdd.Text);
                UpdateFolderCounts();
                MessageBox.Show($"Folder {folderToAdd.Text} added succesfully!");
            }            
        }

        // Remove folder from the AI directory
        private void RemoveFolderFromAI(object sender, RoutedEventArgs e)
        {
            string root = "./carsForAI/" + folderToRemove.Text;
            // Check that input isn't empty
            if (folderToRemove.Text.Length == 0)
            {
                MessageBox.Show("Error: Empty input not accepted. Give an input first");
            }
            else if (Directory.Exists(root))
            {
                Directory.Delete(root);
                carsForAI.Remove(folderToRemove.Text);
                UpdateFolderCounts();
                MessageBox.Show($"Folder {folderToRemove.Text} removed succesfully!");
            }
            // If the folder doesn't exist in the directory
            else
            {
                MessageBox.Show("Error: Folder doesn't exist! Try another one");
            }
        }

        // Show all possible folders
        private void ShowAllAvailableFolders(object sender, RoutedEventArgs e)
        {
            //Check that there is at least 1 folder in the directory
            if (carsForAI.Count == 0)
            {
                MessageBox.Show("Directory empty! Check the cars folder");
            }
            else
            {
                string allFolders = "";
                foreach (string folder in carFolders)
                {
                    allFolders += folder + Environment.NewLine;
                }
                MessageBox.Show($"{allFolders}");
            }            
        }

        // Show all folders currently in the AI directory
        private void ShowFoldersChosenForAI(object sender, RoutedEventArgs e)
        {
            if (carsForAI.Count == 0)
            {
                MessageBox.Show("Directory empty! Try adding folders first");
            }
            else
            {
                string allFolders = "";
                foreach (string folder in carsForAI)
                {
                    allFolders += folder + Environment.NewLine;
                }
                MessageBox.Show($"{allFolders}");
            }
        }
    }
}
