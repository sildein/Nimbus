/*
 * MainController.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Nimbus.Controllers
{
    public class MainController : Controller
    {
        // The file explorer that makes up most of this app
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
                    NewFolder = CurrentWorkingDirectory.Substring(0, index - 1);
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
            else if (folder == "")
            {
                ViewData["pwd"] = CurrentWorkingDirectory;
                return PartialView("Explorer", new Models.Explorer(CurrentWorkingDirectory));
            }

            else if (Directory.Exists(Shared.Prefix + "/Files" + CurrentWorkingDirectory + '/' + folder))
            {
                NewFolder = CurrentWorkingDirectory + folder;
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
            return View(new Models.Main());
        }
    }
}