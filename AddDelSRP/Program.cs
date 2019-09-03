using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
//using System.ComponentModel;

namespace AddDelSRP
{
    class Program
    {
        static void Main(string[] args) {

            string PathCheckFileName = Path.Combine( Environment.GetEnvironmentVariable("windir"),"AddDelSRP");
            
            List<RegValue> regvalue = new List<RegValue> {
                new RegValue("{209b1b45-6c84-42bb-861d-d2a187cea34c}","Full access to disk C:", "C:\\","130888527003079690","0"),
                new RegValue("{c2c2cf69-41c0-496e-aafb-06604377a666}","Full access to disk D:", "D:\\","130888527003079690","0")
            };

           
            string reg_path = @"SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers\262144\Paths";

            if (File.Exists(PathCheckFileName)) {
                DelKeys(reg_path, regvalue, PathCheckFileName);
            } else {
                AddKeys(reg_path, regvalue, PathCheckFileName);
            }


            Console.WriteLine("Press any key....");   
            Console.ReadKey();
        }




        public static void AddKeys(string regpath, List<RegValue> regvalue, string filecheck) {
            
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regpath, true)) {
                int flag = 0;

                foreach (RegValue _regvalue in regvalue) {
                    try {
                        using (RegistryKey subkey = key.CreateSubKey(_regvalue.Path)) {
                            subkey.SetValue("Description", _regvalue.Description);
                            subkey.SetValue("ItemData", _regvalue.ItemData);
                            subkey.SetValue("LastModified", _regvalue.LastModified, RegistryValueKind.QWord);
                            subkey.SetValue("SaferFlags", _regvalue.SaferFlags, RegistryValueKind.QWord);
                            Console.WriteLine("-------------------------------------------");
                            Console.WriteLine($"        Adding {_regvalue.Description}");
                            Console.WriteLine("-------------------------------------------");
                            flag++;
                        }
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Error while Adding {_regvalue.Description}");
                        Console.WriteLine(ex.Message);
                    }
                }

                if (flag == 2) {
                    File.Create(filecheck);
                    Cmd();
                }
                
            }
        }


        public static void DelKeys(string regpath, List<RegValue> regvalue, string filecheck) {
            
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regpath, true)) {
                int flag = 0;                
                foreach (RegValue _regvalue in regvalue) {
                    
                    try {
                        key.DeleteSubKey(_regvalue.Path);
                        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine($"        Deleting {_regvalue.Description}");
                        Console.WriteLine("-------------------------------------------");
                        flag++;
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Error while deleting {_regvalue.Description}");
                        Console.WriteLine(ex.Message);
                    }
                }

                if (flag == 2) {

                    File.Delete(filecheck);
                    Cmd();
                }

            }
        }

        public static void Cmd() {
            Process.Start(new ProcessStartInfo { FileName = "gpupdate.exe", Arguments = $" /target:computer /force"}).WaitForExit();
        }


    }
}
