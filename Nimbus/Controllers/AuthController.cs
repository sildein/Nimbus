/*
 * AuthController.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nimbus.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult TryLogin()
        {
            if (Shared.Users.ValidateUser(Request.Form["Username"],
                Request.Form["Password"]))
            {
                string Username = Request.Form["Username"];
                Response.Cookies.Append("Auth",
                    Shared.Users.NewCookie(Username));
                return Ok();
            }
            else return Forbid();
        }


        [HttpGet]
        public IActionResult Logout()
        {
            Shared.Users.DeleteCookie(Request.Cookies["Auth"]);
            Response.Cookies.Delete("Auth");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}