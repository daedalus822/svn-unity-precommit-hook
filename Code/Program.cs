using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pre_commit
{
    class Program
    {
        private static int Main(string[] args)
        {
            string transactionId = args[1];
            string repository_path = args[0];

            using (var checkforadd = new Process())
            {
                var pathWithEnv = @"%USERPROFILE%\AppData\Local\Temp\add.txt";
                var filepath2 = Environment.ExpandEnvironmentVariables(pathWithEnv);

                checkforadd.StartInfo.UseShellExecute = false;
                checkforadd.StartInfo.RedirectStandardOutput = true;
                checkforadd.StartInfo.FileName = @"svn.exe";
                checkforadd.StartInfo.Arguments = String.Format("status", repository_path);
                checkforadd.Start();

                string test = checkforadd.StandardOutput.ReadToEnd();
                string[] xlines = test.Split('\n');
                List<string> addList = new List<string>();
                
                for (int i=0; i < xlines.Length; i++)
                {
                    if (xlines[i].ToString().StartsWith("A"))
                    {
                        addList.Add(xlines[i].Substring(8));
                    }
                }

                File.WriteAllLines(filepath2, addList, Encoding.UTF8);
            }

            using (var process = new Process())
            {
                var pathWithEnv = @"%USERPROFILE%\AppData\Local\Temp\add.txt";
                var filepath2 = Environment.ExpandEnvironmentVariables(pathWithEnv);

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = @"svnlook.exe";
                process.StartInfo.Arguments = string.Format("log -t {0} \"{1}\"", transactionId, repository_path);
                process.Start();

                string content = File.ReadAllText(repository_path);
                string[] lines = content.Split('\n');
                List<string> sourceList = new List<string>();
                List<string> metaList = new List<string>();

                string addcontent = File.ReadAllText(filepath2);
                string[] addlines = addcontent.Split('\n');
                List<string> addList = new List<string>();

                for (int i = 0; i < addlines.Length; i++)
                {
                    if (addlines[i].Contains(".meta"))
                    {
                        metaList.Add(addlines[i].Trim());
                    }
                    else if (!string.IsNullOrEmpty(addlines[i]))
                    {
                        FileAttributes attr = File.GetAttributes(addlines[i]);

                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            //its a directory don't add to list
                        }else{
                            sourceList.Add(addlines[i].Trim());
                        }
                    }
                }

                int len = sourceList.Count;
                for (int i = len - 1; i >= 0; i--)
                {
                    string metaFileName = sourceList[i] + ".meta";

                    if (!metaList.Contains(metaFileName))
                    {
                        Console.Error.WriteLine("No meta file detected for this file" + sourceList[i]);
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}