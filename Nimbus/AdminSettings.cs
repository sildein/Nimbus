/*
 * AdminSettings.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.IO;
using System.Security.Cryptography;

namespace Nimbus
{
    public class AdminSettings
    {
        private string AdminHash;
        private string ValidSession;


        public string ValidateLogin(string Password)
        {
            if (Shared.GetHash(Password) == this.AdminHash)
            {
                this.ValidSession = Shared.GetRandomHash();
                return this.ValidSession;
            }
            return null;
        }


        public bool ValidateSession(string Session)
        {
            if (Session == this.ValidSession) return true;
            return false;
        }


        public void ChangePassword(string Password)
        {
            if (Password == "_UNCHANGED_" || Password == "_MISMATCH_")
                return;
            else
            {
                this.AdminHash = Shared.GetHash(Password);
                File.WriteAllText("admin.ini", String.Format("{0}\n", 
                this.AdminHash));
                this.ValidSession = "";
            }
        }


        public AdminSettings()
        {
            this.AdminHash = File.ReadAllLines("admin.ini")[0];
        }
    }
}
