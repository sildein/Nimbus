using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tachyon.Models
{
    public class Explorer
    {
        public List<string> Files;
        public List<string> Folders;
        public string CurrentDirectory;

        public Explorer(string Folder)
        {
            this.Files = new List<string>();
            this.Folders = new List<string>();

            this.CurrentDirectory = Folder;

            string[] Files = Directory.GetFiles(Shared.Prefix + Folder);
            foreach (string file in Files) this.Files.Add(file.Split('/', '\\').Last());

            string[] Folders = Directory.GetDirectories(Shared.Prefix + Folder);
            foreach (string folder in Folders) this.Folders.Add(folder.Split('/', '\\').Last());
            Directory.GetCurrentDirectory();
        }
    }
}
