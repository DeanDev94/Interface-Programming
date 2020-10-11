
//DeanDevereaux/R00116198/ELX3/Interfacing Programming Assignment 
using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Windows.Threading;
using System.IO;
using System.Windows.Controls.DataVisualization.Charting;


namespace Interface_Assignment_V1._2_DeanDevereaux
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

        public partial class MainWindow : Window
        {
            //Global Variables
            string MixedFiles = @"F:\ELX3 Semester 2\Interface Programming\Assignment\DeanDevereaux_InterfaceProgramming_ELX3_R00116198\Assignment Folders\MixedFiles";         //Mixed Files Folder Address
            string BigDatFiles = @"F:\ELX3 Semester 2\Interface Programming\Assignment\DeanDevereaux_InterfaceProgramming_ELX3_R00116198\Assignment Folders\BigDatFiles";       //BigDatFiles Folder Address
            string SmallDatFiles = @"F:\ELX3 Semester 2\Interface Programming\Assignment\DeanDevereaux_InterfaceProgramming_ELX3_R00116198\Assignment Folders\SmallDatFiles";   //SmallDatFiles Folder Address
            string BigInfoFiles = @"F:\ELX3 Semester 2\Interface Programming\Assignment\DeanDevereaux_InterfaceProgramming_ELX3_R00116198\Assignment Folders\BigInfoFiles";     //BigInfoFiles Folder Address
            string SmallInfoFiles = @"F:\ELX3 Semester 2\Interface Programming\Assignment\DeanDevereaux_InterfaceProgramming_ELX3_R00116198\Assignment Folders\SmallInfoFiles"; //SmallInfoFiles Folder Address

            public MainWindow()
            {
                InitializeComponent();
                getdatfiles();                                                      //Retrieve .dat extension files from MixedFiles Folder
                getinfofiles();                                                     //Retrieve .info extension files from MixedFiles Folder
            }

            public void getdatfiles()
            {
                string[] dat_filePaths = Directory.GetFiles(MixedFiles, "*.dat");   //Store dat files in string
                foreach (string dat_file in dat_filePaths)                          
                {
                    FileInfo dat = new FileInfo(dat_file);                          //Collect File info
                    string dat_name = dat.Name;                                     //Place file name into a string
                    datbox.Items.Add(dat_name);                                     //Add name to list box 'datbox' 

                }
            }

            public void getinfofiles()
            {
                string[] info_filePaths = Directory.GetFiles(MixedFiles, "*.info"); //Store info files in string
                foreach (string info_file in info_filePaths)                        //Place each file's name into the list box 'infobox'
                {
                    FileInfo info = new FileInfo(info_file);                        //Collect File info
                    string info_name = info.Name;                                   //Place file name into a string
                    infobox.Items.Add(info_name);                                   //Add name to list box 'infobox' 
                }
            }

            public void graph(ref string[] filePaths)
            {
                mainchart.DataContext = null;
                mainchart.Series[0].LegendItems[0] = "File Size(bytes)";                            
                ArrayList chartList = new ArrayList();
                foreach (string file in filePaths)                                                  //Place each file's name and size into chart
                {
                    FileInfo infofile = new FileInfo(file);                                         
                    chartList.Add(new KeyValuePair<string, long>(infofile.Name, infofile.Length));  //Collect Name and Size in Arraylist
                }
                mainchart.DataContext = chartList;                                                  //Place Data into Chart
                mainstack.Visibility = System.Windows.Visibility.Visible;                           // Trick to allow chart to display without crashing the program
            }

            public void movefile(ref string FileName, ref string sourcePath, ref string BigtargetPath, ref string SmalltargetPath)
            {
                string MixedFile = System.IO.Path.Combine(sourcePath, FileName);                    //Convert to string choosen file within sourcePath folder  
                FileInfo Check_Size = new FileInfo(MixedFile);                                      

                if (Check_Size.Length > 10)                                                         //Checks file size to decide folder to place in
                {
                    string OrganisedFolder = System.IO.Path.Combine(BigtargetPath, FileName);       //Create file in Big Folder
                    System.IO.File.Copy(MixedFile, OrganisedFolder, true);                          //Copy file data to new file
                    System.IO.File.Delete(MixedFile);                                               //Delete Original file
                    displaybox.Items.Clear();
                    displaybox.Items.Add("Send to Big Folder");                                     
                }
                else
                {
                    string OrganisedFolder = System.IO.Path.Combine(SmalltargetPath, FileName);     //Create file in Small Folder
                    System.IO.File.Copy(MixedFile, OrganisedFolder, true);                          //Copy file data to new file
                    System.IO.File.Delete(MixedFile);                                               //Delete Original file
                    displaybox.Items.Clear();
                    displaybox.Items.Add("Send to Small Folder");                                   
                }
            }

            public void WriteList(ref string[] Big_filePaths, ref string[] Small_filePaths, ref string WriteListPath)
            {
                string GatherFiles = "";                                                //Declare string before foreach loops
                foreach (string GetFiles in Big_filePaths)                              //This Foreach loop goes through each file in Big info/dat folder
                {
                    StreamReader BigFileData = new StreamReader(GetFiles);              //Converts File to a StreamReader
                    GatherFiles = GatherFiles + BigFileData.ReadToEnd();                //File Data into a string along with any data already inside the string 
                }
                foreach (string GetFiles in Small_filePaths)                            //This Foreach loop goes through each file in Small info/dat folder
                {
                    StreamReader SmallFileData = new StreamReader(GetFiles);            //Converts File to a StreamReader
                    GatherFiles = GatherFiles + SmallFileData.ReadToEnd();              //File Data into a string along with any data already inside the string
                }

                using (StreamWriter AllFiles = new StreamWriter(WriteListPath, true))   //Allows Desired File to be Written on
                {
                    AllFiles.WriteLine(GatherFiles);                                    //Write Data on string to desired File
                }
            }

            //Show Time and Date
            private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            }

            private void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                txtbox.Items.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                CommandManager.InvalidateRequerySuggested();
                txtbox.Items.RemoveAt(0);
                txtbox.SelectedItem = txtbox.Items.CurrentItem;
            }

            //Double click listbox and copy selected file to another folder. should remove file from listbox
            private void datbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
                if (datbox.SelectedValue != null)                                                                       //Checks if an item was picked
                {
                    string datFileName = datbox.SelectedValue.ToString();                                               //Convert selected item to string
                    string dat_sourcePath = MixedFiles;                                                                 //MixedFolder Address
                    string dat_BigtargetPath = BigDatFiles;                                                             //BigFolder Address
                    string dat_SmalltargetPath = SmallDatFiles;                                                         //SmallFolder Address
                    movefile(ref datFileName, ref dat_sourcePath, ref dat_BigtargetPath, ref dat_SmalltargetPath);      //Move selected file to a secondary folder
                    datbox.Items.Clear();                                                                               //Clear list box 'datbox'
                    getdatfiles();                                                                                      //Refresh list box 'datbox'
                    if(datbox.Items.Count == 0)
                    {
                        string[] Dat_BfilePaths = Directory.GetFiles(BigDatFiles);                                                                  //Retrieve Big Folder Address                                   
                        string[] Dat_SfilePaths = Directory.GetFiles(SmallDatFiles);                                                                //Retrieve Small Folder Address
                        string dat_targetPath = @"F:\ELX3 Semester 2\Interface Programming\Assignment\Interface Assignment\DatWriteList.dat";       //Retrieve Target File Address

                        WriteList(ref Dat_BfilePaths, ref Dat_SfilePaths, ref dat_targetPath);                                                      //Writes Data of Files all into One File
                    }
                }
            }

            //Double click listbox and copy selected file to another folder. should remove file from listbox
            private void infobox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
                if (infobox.SelectedValue != null)                                                                      //Checks if an item was picked
                {
                    string infoFileName = infobox.SelectedValue.ToString();                                             //Convert selected item to string
                    string info_sourcePath = MixedFiles;                                                                //MixedFolder Address
                    string info_BigtargetPath = BigInfoFiles;                                                           //BigFolder Address
                    string info_SmalltargetPath = SmallInfoFiles;                                                       //SmallFolder Address
                    movefile(ref infoFileName, ref info_sourcePath, ref info_BigtargetPath, ref info_SmalltargetPath);  //Move selected file to a secondary folder
                    infobox.Items.Clear();                                                                              //Clear list box 'infobox'
                    getinfofiles();                                                                                     //Refresh list box 'infobox'
                    if(infobox.Items.Count == 0)
                    {
                        string[] Info_BfilePaths = Directory.GetFiles(BigInfoFiles);                                                                //Retrieve Big Folder Address
                        string[] Info_SfilePaths = Directory.GetFiles(SmallInfoFiles);                                                              //Retrieve Small Folder Address
                        string Info_targetPath = @"F:\ELX3 Semester 2\Interface Programming\Assignment\Interface Assignment\InfoWriteList.info";    //Retrieve Target File Address

                        WriteList(ref Info_BfilePaths, ref Info_SfilePaths, ref Info_targetPath);                                                   //Writes Data of Files all into One File
                    }
                }
            }

            //Click Info button and graph data on a chart
            private void infobutton_Click(object sender, RoutedEventArgs e)
            {
                mainchart.Title = "Info Files";                                          //Create title for chart
                string[] info_filePaths = Directory.GetFiles(MixedFiles, "*.info");     //Place info files in string array
                graph(ref info_filePaths);                                              //Get File info and plot data
            }

            //Click Dat button and graph data on a chart
            private void datbutton_Click(object sender, RoutedEventArgs e)
            {
                mainchart.Title = "Dat Files";                                          //Create title for chart
                string[] dat_filePaths = Directory.GetFiles(MixedFiles, "*.dat");       //Place dat files in string array
                graph(ref dat_filePaths);                                               //Get File info and plot data
            }

            private void writelist_Click(object sender, RoutedEventArgs e)
            {
                string[] Dat_BfilePaths = Directory.GetFiles(BigDatFiles);                                                                  //Retrieve Big Folder Address                                   
                string[] Dat_SfilePaths = Directory.GetFiles(SmallDatFiles);                                                                //Retrieve Small Folder Address
                string dat_targetPath = @"F:\ELX3 Semester 2\Interface Programming\Assignment\Interface Assignment\DatWriteList.dat";       //Retrieve Target File Address

                WriteList(ref Dat_BfilePaths, ref Dat_SfilePaths, ref dat_targetPath);                                                      //Writes Data of Files all into One File

                string[] Info_BfilePaths = Directory.GetFiles(BigInfoFiles);                                                                //Retrieve Big Folder Address
                string[] Info_SfilePaths = Directory.GetFiles(SmallInfoFiles);                                                              //Retrieve Small Folder Address
                string Info_targetPath = @"F:\ELX3 Semester 2\Interface Programming\Assignment\Interface Assignment\InfoWriteList.info";    //Retrieve Target File Address

                WriteList(ref Info_BfilePaths, ref Info_SfilePaths, ref Info_targetPath);                                                   //Writes Data of Files all into One File
            }

            private void exit_Click(object sender, RoutedEventArgs e)
            {
                Application.Current.Shutdown();     //End Program
            }
        }
}