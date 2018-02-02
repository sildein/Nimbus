/*
 * ServerSettings.cs
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

using System;
using System.Collections.Generic;

namespace Nimbus.Models
{
    public class ServerSettings
    {
        public string Title;
        public string Prefix;
        public string Port;


        public ServerSettings()
        {
            this.Title = Shared.Title;
            this.Prefix = Shared.Prefix;
            this.Port = Shared.Port.ToString();
        }
    }
}
