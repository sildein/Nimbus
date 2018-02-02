/*
 * UserSettings.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.Collections.Generic;

namespace Nimbus.Models
{
    public class UserSettings
    {
        public List<string> Users;


        public UserSettings()
        {
            this.Users = new List<string>();
            foreach (string User in Shared.Users.GetUserList())
                this.Users.Add(User);
        }
    }
}
