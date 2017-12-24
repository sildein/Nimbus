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
        [HttpGet]
        public async Task<FileResult> Download(string file)
        {
            string FilePath = Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) + '/' + file;
            FileStream FStream = new FileStream(Shared.Prefix + "/Files" + FilePath, FileMode.Open);
            return File(FStream, Shared.GetContentType(FilePath), Path.GetFileName(FilePath));
        }
        
        [HttpPost]
        public async Task Upload()
        {
            IFormFile IncomingFile = Request.Form.Files[0];
            string FilePath = Shared.Prefix + "/Files" + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd"));
            string TempFilePath = Shared.Prefix + "/Temp";
            string PartialFileName = IncomingFile.FileName.Trim('"');
            int ThisChunk = Int32.Parse(PartialFileName.Split('_').Last().Split('.').First());
            int TotalChunks = Int32.Parse(PartialFileName.Split('_').Last().Split('.').Last());

            string FileName = PartialFileName.Substring(0, PartialFileName.LastIndexOf('_'));
            FileName = FileName.Substring(0, FileName.LastIndexOf('.'));

            using (FileStream FStream = new FileStream(TempFilePath + '/' + PartialFileName, FileMode.Create))
            {
                await IncomingFile.CopyToAsync(FStream);
                FStream.Close();
            }

            if (ThisChunk == TotalChunks)
            {
                FileStream FStream = new FileStream(FilePath + '/' + FileName, FileMode.Create);
                for (int i = 1; i <= TotalChunks; i++)
                {
                    string FullFilePath = TempFilePath + '/' + FileName + String.Format(".part_{0}.{1}",
                        i, TotalChunks);
                    FileStream PartialFileStream = new FileStream(FullFilePath, FileMode.Open);
                    await PartialFileStream.CopyToAsync(FStream);
                    PartialFileStream.Close();
                    System.IO.File.Delete(FullFilePath);
                }
                FStream.Close();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> NewFolder(string name)
        {
            string Folder = Shared.Prefix + "/Files" + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) + name;
            Directory.CreateDirectory(Folder);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string thing_to_delete)
        {
            if (thing_to_delete.StartsWith("folder_"))
            {
                Directory.Delete(Shared.Prefix + "/Files" + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) +
                    thing_to_delete.Substring(7), true);
            } else if (thing_to_delete.StartsWith("file_"))
            {
                string DeletThis = Shared.Prefix + "/Files" +
                    Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) + '/' + thing_to_delete.Substring(5);
                System.IO.File.Delete(DeletThis);
            }

            return Ok();
        }
    }
}