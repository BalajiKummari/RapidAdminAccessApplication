﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Management.Automation;
using System.Security.Principal;
using System.Diagnostics;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace RAAA
{



    [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lbuser.Text = WindowsIdentity.GetCurrent().Name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Get File Path---------------//
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var fullPathIncludingFileName = ofd.FileName;
            FileInfo fInfo = new FileInfo(fullPathIncludingFileName);
            FileAttributes attributes = File.GetAttributes(fullPathIncludingFileName);


            //Get File Path--------------//

            List<string> arrHeaders = new List<string>();
            Shell32.Shell shell = new Shell32.Shell();
            var strFileName = fullPathIncludingFileName;
            Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(fullPathIncludingFileName));
            Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(fullPathIncludingFileName));
            for (int i = 0; i < short.MaxValue; i++)
            {
                string header = objFolder.GetDetailsOf(null, i);
                if (String.IsNullOrEmpty(header))
                    break;
                arrHeaders.Add(header);
            }
            string metadata = "";
            for (int i = 0; i < arrHeaders.Count; i++)
            {
                metadata = metadata + i + "." + arrHeaders[i] + ":" + objFolder.GetDetailsOf(folderItem, i) + "\n";
                LvData.Items.Add(metadata);
            }
            LbFileMetaData.Content = "selected Path : " + fInfo.FullName;
        }

        // Start Process as a Administrator
        private void btInstall_Click(object sender, RoutedEventArgs e)
        {
            DeployApplications();
        }

        public void DeployApplications()
        {
            PowerShell powerShell = null;

            try
            {

               
                //string UAC_key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
                //Registry.SetValue(UAC_key, "EnableLUA", 1);

                lbuser.Text = WindowsIdentity.GetCurrent().Name;
                var myFile = "C:\\NORD.exe";//"C:\\Users\\Bobby\\Downloads\\notion.exe";
                var pInfo = new ProcessStartInfo
                {
                    FileName = myFile,
                    WorkingDirectory = System.IO.Path.GetDirectoryName(myFile),
                    Arguments = "",
                    LoadUserProfile = true,
                    UseShellExecute = false,
                    UserName = "Bobby",
                    Domain = "WORKGROUP",
                    Verb = "runas",
                    PasswordInClearText = "5456",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true

                };
                Process.Start(pInfo);
                // Check the identity.  
                MessageBox.Show("During impersonation: " + WindowsIdentity.GetCurrent().Name);



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: {0}", ex.InnerException);
                //throw;  
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (powerShell != null)
                    powerShell.Dispose();
            }

        }
    }
}




////Process.Start("C:\\WINDOWS\\system32\\mspaint.exe");
//using (powerShell = PowerShell.Create())
//{
//    //here “executableFilePath” need to use in place of “  
//    //'C:\\ApplicationRepository\\FileZilla_3.14.1_win64-setup.exe'”  
//    //but I am using the path directly in the script.  

//   // var credentials = new UserCredentials("WORKGROUP", "testuser", "123456");
//    //Impersonation.RunAsUser(credentials, LogonType.Batch, () =>
//    //{



//        // Process.Start("C:\\Users\\Bobby\\Downloads\\NordVPNSetup.exe");
//        //System.Diagnostics.Process.Start("C:\\WINDOWS\\system32\\mspaint.exe");

//        // powerShell.AddScript("$setup=Start-Process 'C:\\Users\\Bobby\\Downloads\\Notion.exe ' -ArgumentList ' / S ' -Wait -PassThru");
//        //powerShell.Invoke();
//    //});

//    /*


//    System.Diagnostics.Process.Start("C:\\Users\\Bobby\\Downloads\\NordVPNSetup.exe");

//    //powerShell.Invoke();


//    Collection < PSObject > PSOutput = powerShell.Invoke(); foreach (PSObject outputItem in PSOutput)
//    {

//        if (outputItem != null)
//        {

//            Console.WriteLine(outputItem.BaseObject.GetType().FullName);
//            Console.WriteLine(outputItem.BaseObject.ToString() + "\n");
//        }
//    }

//    if (powerShell.Streams.Error.Count > 0)
//    {
//        string temp = powerShell.Streams.Error.First().ToString();
//        Console.WriteLine("Error: {0}", temp);

//    }
//    else
//        Console.WriteLine("Installation has completed successfully."); */

//}