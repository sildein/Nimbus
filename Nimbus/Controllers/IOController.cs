using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Nimbus.Controllers
{
    public class IOController : Controller
    {
        // Used by IOController to see if all chunks of the uploaded file are present
        public bool CheckChunks(string PartialFileName)
        {
            foreach (string f in Directory.GetFiles(Shared.Prefix + "/Temp"))
            {
                int ThisChunk = Int32.Parse(PartialFileName.Split('_').Last().Split('.').First());
                int TotalChunks = Int32.Parse(PartialFileName.Split('_').Last().Split('.').Last());
                if (ThisChunk == TotalChunks)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet]
        public async Task<FileResult> Download(string file)
        {
            string FilePath = Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) + '/' + file;
            FileStream FStream = new FileStream(Shared.Prefix + "/Files" + FilePath, FileMode.Open);
            return File(FStream, Shared.GetContentType(FilePath), Path.GetFileName(FilePath));
        }

        // Uploads
        [HttpPost]
        public async Task Upload()
        {
            IFormFile IncomingFile = Request.Form.Files[0];
            string FilePath = Shared.Prefix + "/Files" + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd"));
            string TempFilePath = Shared.Prefix + "/Temp";
            string PartialFileName = IncomingFile.FileName.Trim('"');

            string FileName = PartialFileName.Substring(0, PartialFileName.LastIndexOf('_'));
            FileName = FileName.Substring(0, FileName.LastIndexOf('.'));

            using (FileStream FStream = new FileStream(TempFilePath + '/' + PartialFileName, FileMode.Create))
            {
                await IncomingFile.CopyToAsync(FStream);
                FStream.Close();
            }

            if (CheckChunks(PartialFileName))
            {
                FileStream FStream = new FileStream(FilePath + '/' + FileName, FileMode.Create);
                foreach (string f in Directory.GetFiles(Shared.Prefix + "/Temp"))
                {
                    FileStream PartialFileStream = new FileStream(f, FileMode.Open);
                    PartialFileStream.CopyTo(FStream);
                    PartialFileStream.Close();
                    System.IO.File.Delete(f);
                }
                FStream.Close();
            }
        }

        /*// Uploads
        [HttpPost]
        public async Task Upload()
        {
            foreach (IFormFile IncomingFile in Request.Form.Files)
            {
                string FilePath = Shared.Prefix + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd"));
                string FileName = IncomingFile.FileName.Trim('"');

                using (FileStream FStream = new FileStream(FilePath + '/' + FileName, FileMode.Create))
                {
                    await IncomingFile.CopyToAsync(FStream);
                }
            }
        }*/

        // Create a new folder
        [HttpPost]
        public async Task<IActionResult> NewFolder(string name)
        {
            string Folder = Shared.Prefix + "/Files" + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) + name;
            Directory.CreateDirectory(Folder);
            return Ok();
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string thing_to_delete)
        {
            if (thing_to_delete.StartsWith("folder_"))
            {
                Directory.Delete(Shared.Prefix + "/Files" + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) +
                    thing_to_delete.Substring(7), true);
            } else if (thing_to_delete.StartsWith("file_"))
            {
                System.IO.File.Delete(Shared.Prefix + "/Files" +
                    Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) + thing_to_delete.Substring(5));
            }

            return Ok();
        }
    }
}