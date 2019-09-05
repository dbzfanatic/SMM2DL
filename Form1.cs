using Microsoft.VisualBasic; //needed for inputbox to get level overwrite,makes things easier
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; //image display/manipulation
using System.IO;
using System.IO.Compression; //for working with zip files
using System.Linq;
using System.Net;
using System.Resources; //for working with resource files
using System.Text;
using System.Text.RegularExpressions; //for working with regex, how I get the proper data from the webpage
using System.Threading.Tasks; //to enable sleep
using System.Windows.Forms;
using System.Windows;

namespace SMM2DL
{
    public partial class frmSMM2 : Form
    {
        //Declare variables
        public static int iLasttoLoad = 706; //integers
        public bool bFastLoad = false; //bools
        public String sText, appdatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), sSaveFolder = "\\yuzu\\nand\\user\\save\\0000000000000000\\D89D632B98FD70C3A30916A6BF4138F6\\01009B90006DC000"; //strings
        public String[] sTOD, sCreator, sRD, sWorld, sGT, sName; //string array
        public ResXResourceSet Resources; //Resource file to read required data (02.BMP)
        public IniFile settingsfile = null; //inifile to read settings

        public frmSMM2()
        {
            InitializeComponent(); //setup everything
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //lstMarioLevels.Sorting = SortOrder.Descending; //set start sorting order
            if (!Directory.Exists(appdatadir + "\\MM2")) //if required directory doesn't exist
            {
                Directory.CreateDirectory(appdatadir + "\\MM2"); //create it
            }

            if (!Directory.Exists(appdatadir + "\\MM2\\Thumbs")) //if thumbnail dir doesn't exist
            {
                Directory.CreateDirectory(appdatadir + "\\MM2\\Thumbs"); //create it
            }

            if (!Directory.Exists(appdatadir + "\\MM2\\Levels")) //if level download dir doesn't exist
            {
                Directory.CreateDirectory(appdatadir + "\\MM2\\Levels"); //create it
            }

            if (!File.Exists(appdatadir + "\\MM2\\Thumbs\\02.bmp")) //if 02.bmp doesn't exist
            {
                Bitmap image = new Bitmap(Properties.Resources._02); //read from resource file
                image.Save(appdatadir + "\\MM2\\Thumbs\\02.bmp"); //copy to Thumbs directory
            }

            if(!File.Exists(appdatadir + "\\MM2\\settings.ini")) //if there's no settings file
            {
                Console.WriteLine("Settings.ini not found!"); //debug message about not having a file
                iLasttoLoad = Int32.Parse(txtLLevel.Text); //read default last load value
                sSaveFolder = txtFolder.Text; //read default savedir value
            }
            else //if settingsfile exists
            {
                Console.WriteLine("Settings file found, loading data..."); //debug message saying file is found
                settingsfile = new IniFile(appdatadir + "\\MM2\\settings.ini"); //open file reference for reading
                iLasttoLoad = Int32.Parse(settingsfile.Read("UserPrefs", "LvlLoad")); //load last level as an int
                sSaveFolder = settingsfile.Read("UserPrefs", "SaveFolder"); //load save folder string
                txtFolder.Text = sSaveFolder; //set save folder in GUI
                txtLLevel.Text = iLasttoLoad.ToString(); //set last level in GUI
            }

            sTOD = new String[iLasttoLoad + 1]; //set array length
            sName = new String[iLasttoLoad + 1]; //set array length
            sCreator = new String[iLasttoLoad + 1]; //set array length
            sRD = new String[iLasttoLoad + 1]; //set array length
            sWorld = new String[iLasttoLoad + 1]; //set array length
            sGT = new String[iLasttoLoad + 1]; //set array length
        }

        private void lstMarioLevels_Sort(object sender, ColumnClickEventArgs e) //function to sort columns by click
        {
            SortOrder cur = lstMarioLevels.Sorting; //get current sort order
            if (cur == SortOrder.Ascending){ //if it is ascending
                lstMarioLevels.Sorting = SortOrder.Descending; //set to descending
            }
            else //if not
            {
                lstMarioLevels.Sorting = SortOrder.Ascending; //set ascending
            }
        }

        public void ExtractEvent(object sender, MouseEventArgs e) //function to DL and extract levels
        {
            string sLevelDLNum = lstMarioLevels.SelectedItems[0].SubItems[5].Text, sLvlNum = Interaction.InputBox("Enter Level Number", "Please enter the level number you are replacing","000"); //set variables selected level to DL and level to overwrite/save to
            WebClient oIE = new WebClient(); //create webclient for downloading levels
            Console.WriteLine("Downloading level: " + sLevelDLNum); //display status in debug message
            stsStatus.Text = "Downloading level: " + sLevelDLNum; //display status in statusbar
            if (!File.Exists(appdatadir + "\\MM2\\Levels\\" + sLevelDLNum + ".zip")) //if the file isn't already downloaded
            {
                oIE.DownloadFile("http://tinfoil.io/MarioMaker/Download/" + sLevelDLNum, appdatadir + "\\MM2\\Levels\\" + sLevelDLNum + ".zip"); //download and save it to the DL folder for use
            }
            ZipFile.ExtractToDirectory(appdatadir + "\\MM2\\Levels\\" + sLevelDLNum + ".zip", appdatadir + "\\MM2\\Levels\\tmp"); //extract the files from the downloaded level
            if(!Directory.Exists(sSaveFolder)) //if the save folder doesn't exist
            {
                Directory.CreateDirectory(sSaveFolder); //create it
            }
            if (File.Exists(sSaveFolder + "\\course_data_" + sLvlNum + ".bcd")) //if the file already exists for a level
            {
                File.Delete(sSaveFolder + "\\course_data_" + sLvlNum + ".bcd"); //delete it
            }
            File.Move(appdatadir + "\\MM2\\Levels\\tmp\\course_data_000.bcd", sSaveFolder + "\\course_data_" + sLvlNum + ".bcd"); //and replace with the new one
            try //not every level will have replay data so we try instead of assuming it's there
            {
                File.Move(appdatadir + "\\MM2\\Levels\\tmp\\course_replay_000.dat", sSaveFolder + "\\course_replay_" + sLvlNum + ".dat"); //move replay data for level
            }
            catch
            {
                //if there's an issue ignore it
            }
            if(File.Exists(sSaveFolder + "\\course_thumb_" + sLvlNum + ".btl")) //if the file already exists for a level
            {
                File.Delete(sSaveFolder + "\\course_thumb_" + sLvlNum + ".btl"); //delete it
            }
            File.Move(appdatadir + "\\MM2\\Levels\\tmp\\course_thumb_000.btl", sSaveFolder + "\\course_thumb_" + sLvlNum + ".btl"); //and replace with the new one
            Directory.Delete(appdatadir + "\\MM2\\Levels\\tmp"); //remove the temp dir we created during extraction
            stsStatus.Text = "Extraction complete!"; //change statusbar text to inform the user the extraction is complete
        }

        private void MouseCheck(object sender, MouseEventArgs e) //check if an item in the listview is right-clicked
        {
            if(e.Button == MouseButtons.Right) //if the mouse button is the right-click
            {
                mnuRightClick.Show(Cursor.Position); //display the context menu
            }
        }

        private void MouseExtract(object sender, EventArgs e) //function for extracting from the menu
        {
            ExtractEvent(lstMarioLevels,null); //call the ExtractEvent function to DL/Extract a level
        }

        private void SaveLChanged(object sender, EventArgs e) //if a change is detected for save folder
        {
            if(settingsfile != null) //if settingsfile exists
            {
                settingsfile.Write("UserPrefs", "SaveFolder", txtFolder.Text); //write the new value
            }
            else //if not
            {
                File.Create(appdatadir + "\\MM2\\settings.ini"); //create the file
                settingsfile = new IniFile(appdatadir + "\\MM2\\settings.ini"); //open the file reference
                settingsfile.Write("UserPrefs", "SaveFolder", txtFolder.Text); //write the new value
            }
        }

        private void LevelChanged(object sender, EventArgs e) //if a change is detected for last level to load
        {
            if (settingsfile != null) //if settingsfile exists
            {
                settingsfile.Write("UserPrefs", "LvlLoad", txtLLevel.Text); //write the new value
            }
            else //if not
            {
                File.Create(appdatadir + "\\MM2\\settings.ini"); //create the file
                settingsfile = new IniFile(appdatadir + "\\MM2\\settings.ini"); //open the file reference
                settingsfile.Write("UserPrefs", "LvlLoad", txtLLevel.Text); //write the new value
            }
        }

        private byte[] Image2ByteArray(System.Drawing.Image imageIn) //return an image as a byte array
        {
            using (var ms = new MemoryStream()) //open a byte stream
            {
                imageIn.Save(ms, imageIn.RawFormat); //save the image in raw format into memory
                return ms.ToArray(); //return the converted byte array
            }
        }

        private void LoadList(object sender, EventArgs e) //load the data into the list view
        {
            WebClient oIE = new WebClient(); //create a webclient to get level data

            lstMarioLevels.Items.Clear(); //clear the "loading data..." placeholder text from the listview

            for (int i = 1; i <= iLasttoLoad; i++) //repeat until we reach the last level
            {
                txtFolder.Update(); //update to properly display while loading
                txtLLevel.Update(); //update to properly display while loading

                lblLastLoad.Update(); //update to properly display while loading
                lblSave.Update(); //update to properly display while loading

                lstMarioLevels.Update(); //update to show new data

                stsStatus.Text = "Loading data for level: " + i; //set statusbar text to inform users of the current level that is loading
                stsMainStatus.Update(); //update to properly display while loading
                if (i == 400 | i == 406 | i == 468 | i == 500 | i == 513 | i == 527 | i == 537 | i == 546 | i == 553 | i == 574 | i == 626 | i == 628 | i == 658 | i == 701 | i == 712) //skip loading images for these levels (bad images)
                {
                    MakePlaceholder(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"); //make a placeholder level icon for levels with bad images
                    i++; //go to next level
                }
                if (i < 10) //if less than 10 levels in we have to parse things differently
                {
                    string sData = ""; //set sData to a blank string for levels we can't use
                    try //not every level is available so we try
                    {
                        sData = oIE.DownloadString("https://tinfoil.io/MarioMaker/View/0" + i); //get source of the page
                        sData = Regex.Replace(sData, @"<.*?>", String.Empty); //remove html formatting
                    }
                    catch (WebException ex)
                    {
                        //skip the level if there's an error
                    }
                    if(sData.Contains("Time of Day")) //if we see "Time of Day" we have the proper data and can continue
                    {
                        //Console.WriteLine(sData); //debug/testing
                        sTOD[i] = Regex.Match(sData, @"(Time of Day)\s*(Day|Night)").Groups[2].Value; //set the array value using regex for Time of Day
                        sName[i] = Regex.Match(sData, @"(Log in)\s*(\b((?!=|\,).)+(.)\b)").Groups[2].Value; //set the array value using regex for level name
                        sCreator[i] = Regex.Match(sData, @"(Creator)\s*(\w*\s?\w+)").Groups[2].Value; //set the array value using regex for creator name
                        sRD[i] = Regex.Match(sData, @"(Release Date)\s*(\d{2}-\d{2}-\d{4})").Groups[2].Value; //set the array value using regex for release date
                        sWorld[i] = Regex.Match(sData, @"(World)\s*(\w+)").Groups[2].Value; //set the array value using regex for world type
                        sGT[i] = Regex.Match(sData, @"(Game\s*)(\w*(\s?\w+)*)").Groups[2].Value; //set the array value using regex for game type
                    }
                    
                    if (!File.Exists(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp")) //if the preview image doesnt exist
                    {
                        try //may be a bad image so try instead of assuming
                        {
                            oIE.DownloadFile(new Uri("https://tinfoil.io/MarioMaker/Thumb/0" + i), appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp"); //downlod the preview image
                        }
                        catch (WebException ex)
                        {
                            MakePlaceholder(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp"); //if the image is bad or can't be retrieved make a placeholder
                        }
                        if (File.Exists(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp")) //if the preview image exists now
                        {
                            Bitmap old = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp"); //load the image into memory
                            if (old.Width > 64) //if the width is too big (original size)
                            {
                                Bitmap newBMP = ResizeBitmap(old, 64, 36); //resize the preview to fit in the listview
                                old.Dispose(); //release the memory for the old file
                                System.Threading.Thread.Sleep(500); //give the system time to release resources
                                newBMP.Save(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp"); //save with the old image name
                                newBMP.Dispose(); //release resources for new,smaller image
                            }
                            
                        }
                        
                    }
                    Bitmap curImg = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp"); //load the preview image which should now exist
                    imageList1.Images.Add(curImg); //add to the image list
                    curImg.Dispose(); //release resources
                    AddLine(i); //adds a line to the listview with new data                 
                }
                else //if we're over 9 then we can parse everything the same
                {
                    string sData = ""; //set sData to a blank string for levels we can't use
                    try //not every level is available so we try
                    {
                        sData = oIE.DownloadString("https://tinfoil.io/MarioMaker/View/" + i); //get source of the page
                        sData = Regex.Replace(sData, @"<.*?>", String.Empty); //remove html formatting
                    }
                    catch (WebException ex)
                    {
                        //skip the level if there's an error
                    }
                    if (sData.Contains("Time of Day")) //if we see "Time of Day" we have the proper data and can continue
                    {
                        //Console.WriteLine(sData); //debug/testing
                        sTOD[i] = Regex.Match(sData, @"(Time of Day)\s*(Day|Night)").Groups[2].Value; //set the array value using regex for Time of Day
                        sName[i] = Regex.Match(sData, @"(Log in)\s*(\b((?!=|\,).)+(.)\b)").Groups[2].Value; //set the array value using regex for level name
                        sCreator[i] = Regex.Match(sData, @"(Creator)\s*(\w*\s?\w+)").Groups[2].Value; //set the array value using regex for creator name
                        sRD[i] = Regex.Match(sData, @"(Release Date)\s*(\d{2}-\d{2}-\d{4})").Groups[2].Value; //set the array value using regex for release date
                        sWorld[i] = Regex.Match(sData, @"(World)\s*(\w+)").Groups[2].Value; //set the array value using regex for world type
                        sGT[i] = Regex.Match(sData, @"(Game\s*)(\w*(\s?\w+)*)").Groups[2].Value; //set the array value using regex for game type
                    }

                    if (!File.Exists(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp")) //if the preview image doesnt exist
                    {
                        try //may be a bad image so try instead of assuming
                        {
                            oIE.DownloadFile(new Uri("https://tinfoil.io/MarioMaker/Thumb/" + i), appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"); //downlod the preview image
                        }
                        catch (WebException ex)
                        {
                            MakePlaceholder(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"); //if the image is bad or can't be retrieved make a placeholder
                        }
                        if (File.Exists(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp")) //if the preview image exists now
                        {
                            Bitmap old = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"); //load the image into memory
                            if (old.Width > 64) //if the width is too big (original size)
                            {
                                Bitmap newBMP = ResizeBitmap(old, 64, 36); //resize the preview to fit in the listview
                                old.Dispose(); //release the memory for the old file
                                System.Threading.Thread.Sleep(500); //give the system time to release resources
                                newBMP.Save(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"); //save with the old image name
                                newBMP.Dispose(); //release resources for new,smaller image
                            }

                        }

                    }
                    Bitmap curImg = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"); //load the preview image which should now exist
                    imageList1.Images.Add(curImg); //add to the image list
                    curImg.Dispose(); //release resources
                    AddLine(i); //adds a line to the listview with new data               
                }
            }

            stsStatus.Text = "Loading complete!"; //set statusbar text letting the user know everything is now loaded
            
        }

        private void MakePlaceholder(string sLocation) //create a placeholder for levels with bad/missing images
        {
            Bitmap placeholder = new Bitmap(64, 36); //create a new blank bitmap with proper size
            Graphics g = Graphics.FromImage(placeholder); //create the GDI graphics reference
            Rectangle rect = new Rectangle(0, 0, 64, 36); //create the rectangle with the right size
            SolidBrush sBrush = new SolidBrush(Color.Red); //create a red brush/color to use
            g.FillRectangle(sBrush, rect); //fill the rectangle with red
            placeholder.Save(sLocation); //save the placeholder to the given location/folder
            imageList1.Images.Add(placeholder); //add the placeholder to the image list
            g.Dispose(); //release GDI resources
            placeholder.Dispose(); //release resources for placeholder bitmap
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height) //function to change bitmap size
        {
            Bitmap result = new Bitmap(width, height); //set a new bitmap with proper size
            using (Graphics g = Graphics.FromImage(result)) //create a GDI graphics reference
            {
                g.DrawImage(bmp, 0, 0, width, height); //draw the image scaled to the new graphic
            }

            return result; //return the new graphic as a resized image
        }

        public void AddLine(int iIndex) //add a line to the listview
        {
            if (sName[iIndex] != null) //if the level at the given index isn't empty
            {
                //Console.WriteLine("Images total: " + imageList1.Images.Count.ToString()); //debug/test
                lstMarioLevels.StateImageList = imageList1; //set the imagelist for the listview in case it was lost/unset
                ListViewItem lstItem = lstMarioLevels.Items.Add(sName[iIndex]); //create a new listview item to work with
                lstItem.ImageIndex = iIndex - 1; //set the image index for the current line to be 1 less than the index so pictures match
                lstItem.SubItems.Add(sTOD[iIndex]); //add ToD to line
                lstItem.SubItems.Add(sWorld[iIndex]); //add world type to line
                lstItem.SubItems.Add(sRD[iIndex]); //add release date to line
                lstItem.SubItems.Add(sCreator[iIndex]); //add creator name to line
                if (iIndex < 10) //if index is less than 10 we add a leading 0
                {
                    lstItem.SubItems.Add("0" + iIndex); //display level number with leading 0
                }
                else //if greater than 9 no leading 0
                {
                    lstItem.SubItems.Add(iIndex.ToString()); //add index as a string for level number to line
                }
                lstItem.SubItems.Add(sGT[iIndex]); //add game time to line
                //Console.WriteLine("Name is: " + sName[iIndex] + "\nToD is: " + sTOD[iIndex] + "\nWorld is: " + sWorld[iIndex] + "\nRelease is: " + sRD[iIndex] + "\nCreator is: " + sCreator[iIndex] + "\nType is: " + sGT[iIndex] + "\nLevel Num is: " + iIndex); //debug/test
            }
        }
    }
    
}
