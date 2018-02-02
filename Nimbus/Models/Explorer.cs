/*
 * Explorer.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nimbus.Models
{
    public class Explorer
    {
        public List<string> Files;
        public List<string> Folders;
        public string CurrentDirectory;

        public Explorer(string Username, string Folder)
        {
            this.Files = new List<string>();
            this.Folders = new List<string>();

            this.CurrentDirectory = Folder;

            string FinalDirectory = Path.Combine(Shared.Prefix,
                                                 "Files",
                                                 Username,
                                                 Folder.Substring(1));

            //string[] Files = Directory.GetFiles(FinalDirectory);
            foreach (string file in Directory.GetFiles(FinalDirectory))
                this.Files.Add(file.Split('/', '\\').Last());

            //string[] Folders = Directory.GetDirectories(FinalDirectory);
            foreach (string folder in Directory.GetDirectories(FinalDirectory))
                this.Folders.Add(folder.Split('/', '\\').Last());
        }
    }
}
