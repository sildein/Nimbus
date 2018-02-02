/*
 * UserDatabase.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;

namespace Nimbus
{
    public class UserDatabase
    {
        private Dictionary<string, string> Users;
        private Dictionary<string, string> ValidAuthCookies;


        public void AddUser(string Username, string Password)
        {
            this.Users.Add(Username, Shared.GetHash(Password));
            Directory.CreateDirectory(String.Format("{0}/Files/{1}",
                Shared.Prefix, Username));
            // No need to duplicate the config generation, just feed EditUsers
            // an empty dictionary so the loop is skipped
            this.EditUsers(new Dictionary<string, string>());
        }


        public void DeleteUser(string Username)
        {
            this.Users.Remove(Username);
            // same deal as AddUser()
            this.EditUsers(new Dictionary<string, string>());
        }


        public void EditUsers(Dictionary<string, string> UserList)
        {
            foreach (string Username in UserList.Keys)
            {
                if (UserList[Username] != "_UNCHANGED_" &&
                    UserList[Username] != "_MISMATCH_")
                {
                    this.Users[Username] = Shared.GetHash(UserList[Username]);
                    if (this.ValidAuthCookies.ContainsKey(Username))
                        this.ValidAuthCookies.Remove(Username);
                }
            }

            string UserConfig = "";
            foreach (string Username in this.Users.Keys)
            {
                UserConfig += String.Format("{0}:{1}\n", Username, this.Users[Username]);
            }
            File.WriteAllText("users.ini", UserConfig);
        }


        public string NewCookie(string User)
        {
            string Cookie = Shared.GetRandomHash();
            this.ValidAuthCookies.Add(Cookie, User);
            return Cookie;
        }


        public void DeleteCookie(string Cookie)
        {
            this.ValidAuthCookies.Remove(Cookie);
        }


        public bool ValidateUser(string Username, string Password)
        {
            string PasswordHash;
            if (this.Users.ContainsKey(Username))
            {
                PasswordHash = Shared.GetHash(Password);
                if (PasswordHash == this.Users[Username]) return true;
            }
            return false;
        }


        public string ValidateCookie(string Cookie)
        {
            if (this.ValidAuthCookies.ContainsKey(Cookie))
                return this.ValidAuthCookies[Cookie];
            else return null;
        }


        public List<string> GetUserList()
        {
            List<string> UserList = new List<string>();
            foreach (string User in this.Users.Keys)
            {
                UserList.Add(User);
            }
            return UserList;
        }


        public UserDatabase()
        {
            this.Users = new Dictionary<string, string>();
            this.ValidAuthCookies = new Dictionary<string, string>();

            foreach (string UserAndHash in File.ReadAllLines("users.ini"))
            {
                string[] SplitLine = UserAndHash.Split(':');
                if (SplitLine.Length > 1) this.Users.Add(SplitLine[0], SplitLine[1]);
            }
        }
    }
}
