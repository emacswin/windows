using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    public class PasswordCommandHandler : FtpCommandHandler
    {
        public PasswordCommandHandler(FtpConnectionObject connectionObject)
            : base("PASS", connectionObject)
        {

        }

        protected override string OnProcess(string sMessage)
        {
            if (ConnectionObject.Login(sMessage))
            {
                return GetMessage(220, "Password ok, FTP server ready");
            }
            else
            {
                return GetMessage(530, "Username or password incorrect");
            }
        }
    }
}
