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
        [HttpPost]
        public IActionResult Explorer()
        {
            // Check for a valid auth cookie
            string Username =
                Shared.Users.ValidateCookie(Request.Cookies["Auth"]);
            if (Username == null) return Forbid();

            string Folder = Request.Form["Folder"];

            if (Folder == "..")
            {
                string ThisFolder = Directory.GetParent(
                    HttpContext.Session.GetString("pwd")).ToString();
                HttpContext.Session.SetString("pwd", ThisFolder);
                return PartialView("Explorer",
                                   new Models.Explorer(Username, ThisFolder));
            }

            else
            {
                string ThisFolder = Path.Combine(
                    HttpContext.Session.GetString("pwd"), Folder);
                HttpContext.Session.SetString("pwd", ThisFolder);
                string ayylomao = HttpContext.Session.GetString("pwd");
                return PartialView("Explorer",
                                   new Models.Explorer(Username, ThisFolder));
            }
        }


        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("pwd", "/");

            // Send users to the login page if they don't have a valid auth
            // cookie
            if (Request.Cookies.ContainsKey("Auth"))
            {
                string User = Shared.Users.ValidateCookie(
                    Request.Cookies["Auth"]);
                if (User != null)
                {
                    ViewData["Username"] = User;
                    return View(new Models.Main());
                }
            }
            return Redirect("/Auth/");
        }
    }
}