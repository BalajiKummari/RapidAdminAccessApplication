using System;
using System.Collections.Generic;
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
using System.IO;
using Microsoft.Win32;
using System.Management.Automation;
using System.Collections.ObjectModel;
using Microsoft.PowerShell.Commands;
using SimpleImpersonation;
using System.Security.Principal;
using System.Diagnostics;
using System.Security;
using System.Net;
using System.Security.Permissions;

namespace RAAA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
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
            //Get File Path---------------
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var fullPathIncludingFileName = ofd.FileName;
            FileInfo fInfo = new FileInfo(fullPathIncludingFileName);
            FileAttributes attributes = File.GetAttributes(fullPathIncludingFileName);
            
           
            //---------------------


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

            string metadata = "" ;
            for (int i = 0; i < arrHeaders.Count; i++)
            {
                 metadata = metadata + i +"." + arrHeaders[i] +":" + objFolder.GetDetailsOf(folderItem, i) +"\n";
                LvData.Items.Add(metadata);
            }
            LbFileMetaData.Content = "selected Path : " + fInfo.FullName; 
     
        }

        private void btInstall_Click(object sender, RoutedEventArgs e)
        {
            DeployApplications();
        }

        public  void DeployApplications()
        {
            PowerShell powerShell = null;
            //adde
            Console.WriteLine(" ");
            Console.WriteLine("Deploying application...");
            try
            {
                //Process.Start("C:\\WINDOWS\\system32\\mspaint.exe");
                using (powerShell = PowerShell.Create())
                {
                    //here “executableFilePath” need to use in place of “  
                    //'C:\\ApplicationRepository\\FileZilla_3.14.1_win64-setup.exe'”  
                    //but I am using the path directly in the script.  

                    var credentials = new UserCredentials("WORKGROUP", "testuser", "123456");
                    //Impersonation.RunAsUser(credentials, LogonType.Batch, () =>
                    //{

                        lbuser.Text = WindowsIdentity.GetCurrent().Name;
                    var myFile = "C:\\notion.exe";//"C:\\Users\\Bobby\\Downloads\\notion.exe";
                        var pInfo = new ProcessStartInfo
                        {
                            FileName = myFile,
                            WorkingDirectory = System.IO.Path.GetDirectoryName(myFile),
                            Arguments = "",
                            LoadUserProfile = true,
                            UseShellExecute = false,
                            UserName = "testuser",
                            Domain = "WORKGROUP",
                            Verb= "runas",
                            PasswordInClearText = "123456",
                           RedirectStandardOutput = true,
                           RedirectStandardError = true

                        };
                    Process.Start(pInfo);

                        // Process.Start("C:\\Users\\Bobby\\Downloads\\NordVPNSetup.exe");
                        //System.Diagnostics.Process.Start("C:\\WINDOWS\\system32\\mspaint.exe");

                        // powerShell.AddScript("$setup=Start-Process 'C:\\Users\\Bobby\\Downloads\\Notion.exe ' -ArgumentList ' / S ' -Wait -PassThru");
                        //powerShell.Invoke();
                    //});

                    /*

                   
                    System.Diagnostics.Process.Start("C:\\Users\\Bobby\\Downloads\\NordVPNSetup.exe");

                    //powerShell.Invoke();


                    Collection < PSObject > PSOutput = powerShell.Invoke(); foreach (PSObject outputItem in PSOutput)
                    {

                        if (outputItem != null)
                        {

                            Console.WriteLine(outputItem.BaseObject.GetType().FullName);
                            Console.WriteLine(outputItem.BaseObject.ToString() + "\n");
                        }
                    }

                    if (powerShell.Streams.Error.Count > 0)
                    {
                        string temp = powerShell.Streams.Error.First().ToString();
                        Console.WriteLine("Error: {0}", temp);

                    }
                    else
                        Console.WriteLine("Installation has completed successfully."); */

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: {0}", ex.InnerException);
                //throw;  
            }
            finally
            {
                if (powerShell != null)
                    powerShell.Dispose();
            }

        }
    }
}
