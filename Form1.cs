using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace SMM2DL
{
    public partial class frmSMM2 : Form
    {
        public static int iLasttoLoad = 706;
        public bool bFastLoad = false;
        public String sText, appdatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), sSaveFolder = "\\yuzu\\nand\\user\\save\\0000000000000000\\D89D632B98FD70C3A30916A6BF4138F6\\01009B90006DC000";
        public String[] sTOD, sCreator, sRD, sWorld, sGT, sName;
        public ResXResourceSet Resources;
        public IniFile settingsfile = null;
        public frmSMM2()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //lstMarioLevels.Sorting = SortOrder.Descending;
            if (!Directory.Exists(appdatadir + "\\MM2"))
            {
                Directory.CreateDirectory(appdatadir + "\\MM2");
            }

            if (!Directory.Exists(appdatadir + "\\MM2\\Thumbs"))
            {
                Directory.CreateDirectory(appdatadir + "\\MM2\\Thumbs");
            }

            if (!Directory.Exists(appdatadir + "\\MM2\\Levels"))
            {
                Directory.CreateDirectory(appdatadir + "\\MM2\\Levels");
            }

            if (!File.Exists(appdatadir + "\\MM2\\Thumbs\\02.bmp"))
            {
                Bitmap image = new Bitmap(Properties.Resources._02);
                image.Save(appdatadir + "\\MM2\\Thumbs\\02.bmp");
            }

            if(!File.Exists(appdatadir + "\\MM2\\settings.ini"))
            {
                Console.WriteLine("Settings.ini not found!");
            }
            else
            {
                Console.WriteLine("Settings file found, loading data...");
                settingsfile = new IniFile(appdatadir + "\\MM2\\settings.ini");
                iLasttoLoad = Int32.Parse(settingsfile.Read("UserPrefs", "LvlLoad"));
                sSaveFolder = settingsfile.Read("UserPrefs", "SaveFolder");
                txtFolder.Text = sSaveFolder;
                txtLLevel.Text = iLasttoLoad.ToString();
                sTOD = new String[iLasttoLoad + 1];
                sName = new String[iLasttoLoad + 1];
                sCreator = new String[iLasttoLoad + 1];
                sRD = new String[iLasttoLoad + 1];
                sWorld = new String[iLasttoLoad + 1];
                sGT = new String[iLasttoLoad + 1];
            }
        }

        private void lstMarioLevels_Sort(object sender, ColumnClickEventArgs e)
        {
            SortOrder cur = lstMarioLevels.Sorting;
            if (cur == SortOrder.Ascending){
                lstMarioLevels.Sorting = SortOrder.Descending;
            }
            else
            {
                lstMarioLevels.Sorting = SortOrder.Ascending;
            }
        }

        public void ExtractEvent(object sender, MouseEventArgs e)
        {
            string sLevelDLNum = lstMarioLevels.SelectedItems[0].SubItems[5].Text, sLvlNum = Interaction.InputBox("Enter Level Number", "Please enter the level number you are replacing","000");
            WebClient oIE = new WebClient();
            Console.WriteLine("Downloading level: " + sLevelDLNum);
            if(!File.Exists(appdatadir + "\\MM2\\Levels\\" + sLevelDLNum + ".zip"))
            {
                oIE.DownloadFile("http://tinfoil.io/MarioMaker/Download/" + sLevelDLNum, appdatadir + "\\MM2\\Levels\\" + sLevelDLNum + ".zip");
            }
            ZipFile.ExtractToDirectory(appdatadir + "\\MM2\\Levels\\" + sLevelDLNum + ".zip", appdatadir + "\\MM2\\Levels\\tmp");
            if(!Directory.Exists(sSaveFolder))
            {
                Directory.CreateDirectory(sSaveFolder);
            }
            if (File.Exists(sSaveFolder + "\\course_data_" + sLvlNum + ".bcd"))
            {
                File.Delete(sSaveFolder + "\\course_data_" + sLvlNum + ".bcd");
            }
            File.Move(appdatadir + "\\MM2\\Levels\\tmp\\course_data_000.bcd", sSaveFolder + "\\course_data_" + sLvlNum + ".bcd");
            try
            {
                File.Move(appdatadir + "\\MM2\\Levels\\tmp\\course_replay_000.dat", sSaveFolder + "\\course_replay_" + sLvlNum + ".dat");
            }
            catch
            {

            }
            if(File.Exists(sSaveFolder + "\\course_thumb_" + sLvlNum + ".btl"))
            {
                File.Delete(sSaveFolder + "\\course_thumb_" + sLvlNum + ".btl");
            }
            File.Move(appdatadir + "\\MM2\\Levels\\tmp\\course_thumb_000.btl", sSaveFolder + "\\course_thumb_" + sLvlNum + ".btl");
            Directory.Delete(appdatadir + "\\MM2\\Levels\\tmp");
        }

        private void MouseCheck(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                mnuRightClick.Show(Cursor.Position);
            }
        }

        private void MouseExtract(object sender, EventArgs e)
        {
            ExtractEvent(lstMarioLevels,null);
        }

        private void SaveLChanged(object sender, EventArgs e)
        {
            if(settingsfile != null)
            {
                settingsfile.Write("UserPrefs", "SaveFolder", txtFolder.Text);
            }
            else
            {
                File.Create(appdatadir + "\\MM2\\settings.ini");
                settingsfile = new IniFile(appdatadir + "\\MM2\\settings.ini");
                settingsfile.Write("UserPrefs", "SaveFolder", txtFolder.Text);
            }
        }

        private void LevelChanged(object sender, EventArgs e)
        {
            if (settingsfile != null)
            {
                settingsfile.Write("UserPrefs", "LvlLoad", txtLLevel.Text);
            }
            else
            {
                File.Create(appdatadir + "\\MM2\\settings.ini");
                settingsfile = new IniFile(appdatadir + "\\MM2\\settings.ini");
                settingsfile.Write("UserPrefs", "LvlLoad", txtLLevel.Text);
            }
        }

        private byte[] Image2ByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private void LoadList(object sender, EventArgs e)
        {
            WebClient oIE = new WebClient();

            lstMarioLevels.Items.Clear();

            for (int i = 1; i <= iLasttoLoad; i++)
            {
                txtFolder.Update();
                txtLLevel.Update();

                lblLastLoad.Update();
                lblSave.Update();

                lstMarioLevels.Update();

                stsStatus.Text = "Loading image: " + i;
                stsMainStatus.Update();
                if(i == 400 | i == 406 | i == 468 | i == 500 | i == 513 | i == 527 | i == 537 | i == 546 | i == 553 | i == 574 | i == 626 | i == 628 | i == 658 | i == 701 | i == 712)
                {
                    MakePlaceholder(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp");
                    i++;
                }
                if (i < 10)
                {
                    string sData = "";
                    try
                    {
                        sData = oIE.DownloadString("https://tinfoil.io/MarioMaker/View/0" + i);
                        sData = Regex.Replace(sData, @"<.*?>", String.Empty);
                    }
                    catch (WebException ex)
                    {

                    }
                    if(sData.Contains("Time of Day"))
                    {
                        //Console.WriteLine(sData);
                        sTOD[i] = Regex.Match(sData, @"(Time of Day)\s*(Day|Night)").Groups[2].Value;
                        sName[i] = Regex.Match(sData, @"(Log in)\s*(\b((?!=|\,).)+(.)\b)").Groups[2].Value;
                        sCreator[i] = Regex.Match(sData, @"(Creator)\s*(\w*\s?\w+)").Groups[2].Value;
                        sRD[i] = Regex.Match(sData, @"(Release Date)\s*(\d{2}-\d{2}-\d{4})").Groups[2].Value;
                        sWorld[i] = Regex.Match(sData, @"(World)\s*(\w+)").Groups[2].Value;
                        sGT[i] = Regex.Match(sData, @"(Game\s*)(\w*(\s?\w+)*)").Groups[2].Value;
                    }
                    
                    if (!File.Exists(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp"))
                    {
                        try
                        {
                            oIE.DownloadFile(new Uri("https://tinfoil.io/MarioMaker/Thumb/0" + i), appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp");
                        }
                        catch (WebException ex)
                        {
                            MakePlaceholder(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp");
                        }
                        if (File.Exists(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp"))
                        {
                            Bitmap old = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp");
                            if (old.Width > 64)
                            {
                                Bitmap newBMP = ResizeBitmap(old, 64, 36);
                                old.Dispose();
                                System.Threading.Thread.Sleep(500);
                                newBMP.Save(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp");
                                newBMP.Dispose();
                            }
                            
                        }
                        
                    }
                    Bitmap curImg = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\0" + i + ".bmp");
                    imageList1.Images.Add(curImg);
                    curImg.Dispose();
                    AddLine(i);                
                }
                else
                {
                    string sData = "";
                    try
                    {
                        sData = oIE.DownloadString("https://tinfoil.io/MarioMaker/View/" + i);
                        sData = Regex.Replace(sData, @"<.*?>", String.Empty);
                    }
                    catch (WebException ex)
                    {

                    }
                    if (sData.Contains("Time of Day"))
                    {
                        //Console.WriteLine(sData);
                        sName[i] = Regex.Match(sData, @"(Log in)\s*(\b((?!=|\,).)+(.)\b)").Groups[2].Value;
                        sTOD[i] = Regex.Match(sData, @"(Time of Day)\s*(Day|Night)").Groups[2].Value;
                        sCreator[i] = Regex.Match(sData, @"(Creator)\s*(\w*\s?\w+)").Groups[2].Value;
                        sRD[i] = Regex.Match(sData, @"(Release Date)\s*(\d{2}-\d{2}-\d{4})").Groups[2].Value;
                        sWorld[i] = Regex.Match(sData, @"(World)\s*(\w+)").Groups[2].Value;
                        sGT[i] = Regex.Match(sData, @"(Game\s*)(\w*(\s?\w+)*)").Groups[2].Value;
                    }
                    if (!File.Exists(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"))
                    { 
                        try
                        {
                            oIE.DownloadFile(new Uri("https://tinfoil.io/MarioMaker/Thumb/" + i), appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp");
                        }
                        catch (WebException ex)
                        {
                            MakePlaceholder(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp");
                        }
                        if (File.Exists(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp"))
                        {
                            Bitmap old = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp");
                            if (old.Width > 64)
                            {
                                Bitmap newBMP = ResizeBitmap(old, 64, 36);
                                old.Dispose();
                                newBMP.Save(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp");
                                newBMP.Dispose();
                            }
                            
                        }
                    }
                    Bitmap curImg = (Bitmap)Bitmap.FromFile(appdatadir + "\\MM2\\Thumbs\\" + i + ".bmp");
                    imageList1.Images.Add(curImg);
                    curImg.Dispose();
                    AddLine(i);
                }
            }

            stsStatus.Text = "Loading complete!";
            
        }

        private void MakePlaceholder(string sLocation)
        {
            Bitmap placeholder = new Bitmap(64, 36);
            Graphics g = Graphics.FromImage(placeholder);
            Rectangle rect = new Rectangle(0, 0, 64, 36);
            SolidBrush sBrush = new SolidBrush(Color.Red);
            g.FillRectangle(sBrush, rect);
            placeholder.Save(sLocation);
            imageList1.Images.Add(placeholder);
            g.Dispose();
            placeholder.Dispose();
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        public void AddLine(int iIndex)
        {
            if (sName[iIndex] != null)
            {
                Console.WriteLine("Images total: " + imageList1.Images.Count.ToString());
                lstMarioLevels.StateImageList = imageList1;
                ListViewItem lstItem = lstMarioLevels.Items.Add(sName[iIndex]);
                lstItem.ImageIndex = iIndex - 1;
                lstItem.SubItems.Add(sTOD[iIndex]);
                lstItem.SubItems.Add(sWorld[iIndex]);
                lstItem.SubItems.Add(sRD[iIndex]);
                lstItem.SubItems.Add(sCreator[iIndex]);
                if (iIndex < 10)
                {
                    lstItem.SubItems.Add("0" + iIndex);
                }
                else
                {
                    lstItem.SubItems.Add(iIndex.ToString());
                }
                lstItem.SubItems.Add(sGT[iIndex]);
                Console.WriteLine("Name is: " + sName[iIndex] + "\nToD is: " + sTOD[iIndex] + "\nWorld is: " + sWorld[iIndex] + "\nRelease is: " + sRD[iIndex] + "\nCreator is: " + sCreator[iIndex] + "\nType is: " + sGT[iIndex] + "\nLevel Num is: " + iIndex);
            }
        }
    }
    
}
