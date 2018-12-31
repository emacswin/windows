using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    public class NoopCommandHandler : FtpCommandHandler
    {
        public NoopCommandHandler(FtpConnectionObject connectionObject)
            : base("NOOP", connectionObject)
        {

        }

        protected override string OnProcess(string sMessage)
        {
            return GetMessage(200, "");
        }
    }
}
