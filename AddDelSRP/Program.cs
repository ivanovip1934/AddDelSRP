using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Diagnostics;
using Microsoft.Win32;
//using System.ComponentModel;

namespace AddDelSRP
{
    class Program
    {
        static void Main(string[] args) {

            
            //if (!IsAdministrator()) {
            //    string PathToApp = System.Reflection.Assembly.GetEntryAssembly().Location;
            //    var startinfo = new ProcessStartInfo(PathToApp) { Verb = "runas" };
            //    try {
            //        Process.Start(startinfo);
            //        Environment.Exit(0);
            //    }
            //    catch (Exception ex) {
            //        Console.WriteLine("Could not start process.");
            //        Console.WriteLine(ex);
            //    }
                

            //} else  {
            //    Console.WriteLine("YA ADMIN");
            //}

            List<RegValue> regvalue = new List<RegValue> {
                new RegValue("{209b1b45-6c84-42bb-861d-d2a187cea34c}","Full access to disk C:", "C:\\","130888527003079690","0"),
                new RegValue("{c2c2cf69-41c0-496e-aafb-06604377a666}","Full access to disk D:", "D:\\","130888527003079690","0")
            };

            foreach (RegValue _regvalue in regvalue) {
               // Console.WriteLine(_regvalue.Description);
            }
            string reg_path = @"SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers\262144\Paths";
            //string reg_path6432 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\safer\codeidentifiers\262144\Paths";
            AddKeys(reg_path, regvalue);
            //AddKeys(reg_path6432, regvalue);

            //DelKeys(reg_path, regvalue); 
            //DelKeys(reg_path6432, regvalue);


            Cmd("gpupdate /force");

                //Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly().Location);
                Console.ReadKey();
        }




        public static void AddKeys(string path, List<RegValue> regvalue) {
            // int appcount = 0;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true)) {
                Console.WriteLine();
                Console.WriteLine("************************************************************");
                //Console.WriteLine("Key Path: " + key.Name);
                Console.WriteLine("************************************************************");
                Console.WriteLine();


                foreach (RegValue _regvalue in regvalue) {
                    //Console.WriteLine(_regvalue);
                    try {
                        using (RegistryKey subkey = key.CreateSubKey(_regvalue.Path)) {
                            subkey.SetValue("Description", _regvalue.Description);
                            subkey.SetValue("ItemData", _regvalue.ItemData);
                            subkey.SetValue("LastModified", _regvalue.LastModified, RegistryValueKind.QWord);
                            subkey.SetValue("SaferFlags", _regvalue.SaferFlags, RegistryValueKind.QWord);
                        }
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }

                }
                
            }
        }


        public static void DelKeys(string path, List<RegValue> regvalue) {
            // int appcount = 0;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true)) {
                Console.WriteLine();
                Console.WriteLine("************************************************************");
                //Console.WriteLine("Key Path: " + key.Name);
                Console.WriteLine("************************************************************");
                Console.WriteLine();


                foreach (RegValue _regvalue in regvalue) {
                    //Console.WriteLine(_regvalue);
                    try {
                        key.DeleteSubKey(_regvalue.Path);
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        public static void Cmd(string line) {
            Process.Start(new ProcessStartInfo { FileName = "cmd", Arguments = $"/c {line}" }).WaitForExit();
        }





        public static bool IsAdministrator() {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent()) {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);                    
            }
        }

    }
}
