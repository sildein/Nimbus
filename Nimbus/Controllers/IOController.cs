/*
 * IOController.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Nimbus.Controllers
{
    public class IOController : Controller
    {
        [HttpGet]
        public IActionResult Download(string file)
        {
            // Check the user's auth cookie (if any)
            string Username =
                Shared.Users.ValidateCookie(Request.Cookies["Auth"]);
            if (Username == null) return Forbid();

            // Self-explanatory
            FileStream FStream = new FileStream(Path.Combine(new string[] {
                Shared.Prefix,
                "Files",
                Username,
                HttpContext.Session.GetString("pwd").Substring(1),
                file
            }), FileMode.Open);
            return File(FStream, Shared.GetContentType(file),
                        Path.GetFileName(file));
        }
        
        [HttpPost]
        public async Task Upload()
        {
            // Check the user's auth cookie (if any)
            string Username =
                Shared.Users.ValidateCookie(Request.Cookies["Auth"]);
            if (Username == null) return;

            // This is our file object and the name of this file chunk
            IFormFile IncomingFile = Request.Form.Files[0];
            string PartialFileName = IncomingFile.FileName.Trim('"');

            // Which chunk is this?
            int ThisChunk = Int32.Parse(
                PartialFileName.Split('_').Last().Split('.').First());
            int TotalChunks = Int32.Parse(
                PartialFileName.Split('_').Last().Split('.').Last());

            // Get the name of the whole file
            string FileName = PartialFileName.Substring(
                0, PartialFileName.LastIndexOf('_'));
            FileName = FileName.Substring(0, FileName.LastIndexOf('.'));

            // Write the chunk to disk
            FileStream FStream = new FileStream(Path.Combine(new string[] {
                Shared.Prefix,
                "Temp",
                PartialFileName
            }), FileMode.Create);
            await IncomingFile.CopyToAsync(FStream);
            FStream.Close();

            // If we have all of the chunks, rejoin them
            if (ThisChunk == TotalChunks)
            {
                string EndFilePath = Path.Combine(new string[] {
                    Shared.Prefix,
                    "Files",
                    Username,
                    HttpContext.Session.GetString("pwd").Substring(1),
                    FileName
                });
                FileStream EndFileStream = new FileStream(EndFilePath,
                                                    FileMode.Create);
                for (int i = 1; i <= TotalChunks; i++)
                {
                    string ChunkPath = Path.Combine(new string[] {
                        Shared.Prefix,
                        "Temp",
                        String.Format("{0}.part_{1}.{2}", new string[] {
                            FileName,
                            i.ToString(),
                            TotalChunks.ToString()
                        })
                    });
                    FileStream PartialFileStream =
                        new FileStream(ChunkPath, FileMode.Open);
                    await PartialFileStream.CopyToAsync(EndFileStream);
                    PartialFileStream.Close();
                    System.IO.File.Delete(ChunkPath);
                }
                EndFileStream.Close();
            }
        }
        
        [HttpPost]
        public IActionResult NewFolder()
        {
            string Username =
                Shared.Users.ValidateCookie(Request.Cookies["Auth"]);
            if (Username == null) return Forbid();

            string FolderName = Request.Form["FolderName"];
            
            string Folder = Path.Combine(new string[] {
                Shared.Prefix,
                "Files",
                Username,
                HttpContext.Session.GetString("pwd").Substring(1),
                FolderName
            });
            Directory.CreateDirectory(Folder);
            return Ok();
        }
        
        [HttpPost]
        public IActionResult Delete()
        {
            string Username =
                Shared.Users.ValidateCookie(Request.Cookies["Auth"]);
            if (Username == null) return Forbid();

            string DeletThis = Request.Form["DeletThis"];

            if (DeletThis.StartsWith("folder_"))
            {
                Directory.Delete(Path.Combine(new string[] {
                    Shared.Prefix,
                    "Files",
                    Username,
                    HttpContext.Session.GetString("pwd").Substring(1),
                    DeletThis.Substring(7)
                }), true);
            }
            else if (DeletThis.StartsWith("file_"))
            {
                System.IO.File.Delete(Path.Combine(new string[] {
                    Shared.Prefix,
                    "Files",
                    Username,
                    HttpContext.Session.GetString("pwd").Substring(1),
                    DeletThis.Substring(5)
                }));
            }

            return Ok();
        }
    }
}