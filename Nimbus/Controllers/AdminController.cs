/*
 * AdminController.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Nimbus.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult TryLogin()
        {
            string SessionKey =
                Shared.Admin.ValidateLogin(Request.Form["Password"]);
            if (SessionKey != null)
            {
                HttpContext.Session.SetString("AdminSession", SessionKey);
                return RedirectToAction("Index");
            }
            return Forbid();
        }


        [HttpGet]
        public IActionResult ServerSettings()
        {
            if (Shared.Admin.ValidateSession(
                HttpContext.Session.GetString("AdminSession")))
                return PartialView(new Models.ServerSettings());
            else return Forbid();
        }


        [HttpGet]
        public IActionResult UserSettings()
        {
            if (Shared.Admin.ValidateSession(
                HttpContext.Session.GetString("AdminSession")))
                return PartialView(new Models.UserSettings());
            else return Forbid();
        }


        [HttpPost]
        public IActionResult SaveServerSettings()
        {
            if (Shared.Admin.ValidateSession(
                HttpContext.Session.GetString("AdminSession")))
            {
                Shared.Title = Request.Form["Title"];
                Shared.Prefix = Request.Form["Prefix"];
                Shared.Port = Request.Form["Port"];

                // write out new config
                string ConfigFile = String.Format("Title={0}\n" +
                                                  "Prefix={1}\n" +
                                                  "Port={2}\n", Shared.Title,
                                                  Shared.Prefix, Shared.Port);
                System.IO.File.WriteAllText("config.ini", ConfigFile);

                Shared.Admin.ChangePassword(Request.Form["AdminPassword"]);
            }
            return Forbid();
        }


        [HttpPost]
        public IActionResult SaveUserSettings()
        {
            Dictionary<string, string> TempUserList =
                new Dictionary<string, string>();

            if (Shared.Admin.ValidateSession(
                HttpContext.Session.GetString("AdminSession")))
            {
                foreach (string Username in Request.Form.Keys)
                    TempUserList.Add(Username, Request.Form[Username]);
                Shared.Users.EditUsers(TempUserList);
                return Ok();
            }
            return Forbid();
        }


        [HttpPost]
        public IActionResult NewUser()
        {
            if (Shared.Admin.ValidateSession(
                HttpContext.Session.GetString("AdminSession")))
            {
                string Username = Request.Form["Username"];
                string Password = Request.Form["Password"];
                Shared.Users.AddUser(Username, Password);
            }
            return Forbid();
        }


        [HttpPost]
        public IActionResult DeleteUser()
        {
            if (Shared.Admin.ValidateSession(
                HttpContext.Session.GetString("AdminSession")))
                Shared.Users.DeleteUser(Request.Form["Username"]);
            return Forbid();
        }


        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
                return RedirectToAction("Login");
            else if (Shared.Admin.ValidateSession(
                HttpContext.Session.GetString("AdminSession")))
                return View();
            return RedirectToAction("Login");
        }
    }
}
