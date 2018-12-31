using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Ftp;
using FtpCommands = Assemblies.Ftp.FtpCommands;

namespace CSFtp
{
    class Program
    {
        FtpConnectionObject ftpConnectionObject = new FtpConnectionObject(new Assemblies.Ftp.FileSystem.StandardFileSystemClassFactory(), 0, null);

        static void Main(string[] args)
        {
            Assemblies.Ftp.UserData.Get().Load();
            Assemblies.Ftp.UserData.Get().AddUser("wudi");
            Assemblies.Ftp.UserData.Get().SetUserPassword("wudi", "123");
            Assemblies.Ftp.UserData.Get().Save();

            Program p = new Program();

            string sMessage = "USER wudi\r\n";

            p.ftpCommand(sMessage);

            sMessage = "PASS 123\r\n";

            p.ftpCommand(sMessage);

            sMessage = "PWD\r\n";

            p.ftpCommand(sMessage);

            sMessage = "LIST .\r\n";

            p.ftpCommand(sMessage);

            sMessage = "CWD test\r\n";
            p.ftpCommand(sMessage);

            sMessage = "MKD subdir\r\n";
            p.ftpCommand(sMessage);
        }

        public void ftpCommand(string sMessage)
        {
            sMessage = sMessage.Substring(0, sMessage.IndexOf('\r'));

            string sCommand;
            string sValue;

            int nSpaceIndex = sMessage.IndexOf(' ');

            if (nSpaceIndex < 0)
            {
                sCommand = sMessage.ToUpper();
                sValue = "";
            }
            else
            {
                sCommand = sMessage.Substring(0, nSpaceIndex).ToUpper();
                sValue = sMessage.Substring(sCommand.Length + 1);
            }

            FtpCommands.FtpCommandHandler handler = ftpConnectionObject.getCommandHandler(sCommand);

            if (handler == null)
            {
                Console.WriteLine("550 Unknown command\r\n");
            }
            else
            {
                handler.Process(sValue);
            }

            Assemblies.Ftp.UserData.Get().Save();
        }
    }
}
