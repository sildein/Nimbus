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
        public async Task<FileResult> Download(string filename, string pwd)
        {
            string FilePath = pwd + '/' + filename;
            MemoryStream Mem = new MemoryStream();
            using (FileStream FStream = new FileStream(Shared.Prefix + FilePath, FileMode.Open))
            {
                await FStream.CopyToAsync(Mem);
            }
            Mem.Position = 0;
            return File(Mem, Shared.GetContentType(FilePath), Path.GetFileName(FilePath));
        }

        [HttpPost]
        public async Task<IActionResult> Explorer(ICollection<IFormFile> NewFiles)
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
            //return PartialView("Explorer", new Models.Explorer(Encoding.ASCII.GetString(HttpContext.Session.Get("pwd"))));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Explorer(string folder)
        {
            if (folder.EndsWith(".."))
            {
                string NewFolder;
                try
                {
                    int index = folder.LastIndexOf('/');
                    NewFolder = folder.Substring(0, index);
                }
                catch (ArgumentOutOfRangeException)
                {
                    NewFolder = "/";
                }
                HttpContext.Session.SetString("pwd", NewFolder);
                ViewData["pwd"] = NewFolder;
                return PartialView("Explorer", new Models.Explorer(NewFolder));
            }

            else if (Directory.Exists(Shared.Prefix + folder))
            {
                HttpContext.Session.SetString("pwd", folder);
                ViewData["pwd"] = folder;
                return PartialView("Explorer", new Models.Explorer(folder));
            }

            // If control reaches this we have a problem
            return NotFound();
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("pwd", "/");
            return View();
        }
    }
}