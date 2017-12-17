using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Tachyon.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        public async Task<FileResult> Download(string file)
        {
            string FilePath = Encoding.ASCII.GetString(HttpContext.Session.Get("pwd")) + '/' + file;
            FileStream FStream = new FileStream(Shared.Prefix + FilePath, FileMode.Open);
            return File(FStream, Shared.GetContentType(FilePath), Path.GetFileName(FilePath));
        }

        /*[HttpPost]
        public async Task Explorer(ICollection<IFormFile> NewFiles)
        {
            foreach (IFormFile FormFile in NewFiles)
            {
                string FilePath = Shared.Prefix + Encoding.ASCII.GetString(HttpContext.Session.Get("pwd"));
                string FileName = FormFile.FileName.Trim('"');
                using (FileStream FStream = new FileStream(FilePath + '/' + FileName, FileMode.Create))
                {
                    await FormFile.CopyToAsync(FStream);
                }
            }
            string pwd = BitConverter.ToString(HttpContext.Session.Get("pwd"));
            //return PartialView("Explorer", new Models.Explorer(pwd));
            //return RedirectToAction("Index");
        }*/

        [HttpPost]
        public async Task Explorer()
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
        }

        [HttpGet]
        public async Task<IActionResult> Explorer(string folder)
        {
            string CurrentWorkingDirectory = Encoding.ASCII.GetString(HttpContext.Session.Get("pwd"));
            string NewFolder;

            if (folder == "..")
            {
                // We use this exception to keep the user from trying to escape our chroot
                try
                {
                    int index = CurrentWorkingDirectory.LastIndexOf('/');
                    NewFolder = CurrentWorkingDirectory.Substring(0, index);
                }
                catch (ArgumentOutOfRangeException)
                {
                    NewFolder = "/";
                }
                HttpContext.Session.SetString("pwd", NewFolder);
                ViewData["pwd"] = NewFolder;
                return PartialView("Explorer", new Models.Explorer(NewFolder));
            }

            // For refreshing the Explorer view after uploading files
            else if (folder == String.Empty)
            {
                ViewData["pwd"] = CurrentWorkingDirectory;
                return PartialView("Explorer", new Models.Explorer(CurrentWorkingDirectory));
            }

            else if (Directory.Exists(Shared.Prefix + CurrentWorkingDirectory + '/' + folder))
            {
                NewFolder = CurrentWorkingDirectory + '/' + folder;
                HttpContext.Session.SetString("pwd", NewFolder);
                ViewData["pwd"] = NewFolder;
                return PartialView("Explorer", new Models.Explorer(NewFolder));
            }

            // If control reaches this we have a problem
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("pwd", "/");
            return View();
        }
    }
}