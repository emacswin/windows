﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    class XRmdCommandHandler : RemoveDirectoryCommandHandlerBase
    {
        public XRmdCommandHandler(FtpConnectionObject connectionObject)
            : base("XRMD", connectionObject)
        {

        }
    }
}
