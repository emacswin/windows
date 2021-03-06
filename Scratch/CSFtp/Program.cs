﻿using System;
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

            sMessage = "Type A\r\n";
            p.ftpCommand(sMessage);

            sMessage = "PORT 127,0,0,1,28,233\r\n";
            p.ftpCommand(sMessage);

            sMessage = "APPE test.txt\r\n";
            p.ftpCommand(sMessage);

            sMessage = "PORT 127,0,0,1,28,233\r\n";
            p.ftpCommand(sMessage);

            sMessage = "STOR test.txt\r\n";
            p.ftpCommand(sMessage);

            sMessage = "PORT 127,0,0,1,28,233\r\n";
            p.ftpCommand(sMessage);

            sMessage = "RETR test.txt\r\n";
            p.ftpCommand(sMessage);

            /*
             *  ### PORT 127,0,0,1,26,117
                ### NLST

                ### PORT 127,0,0,1,28,233
                ### APPE test123.txt

                ### PORT 127,0,0,1,28,228
                ### STOR test123.txt

                ### PORT 127,0,0,1,26,126
                ### RETR test.txt
             */

            sMessage = "PASV\r\n";
            p.ftpCommand(sMessage);

            sMessage = "SIZE VTK20096952.msg\r\n";
            p.ftpCommand(sMessage);

            sMessage = "DELE del.txt\r\n";
            p.ftpCommand(sMessage);

            sMessage = "RNFR test.txt\r\n";
            p.ftpCommand(sMessage);

            sMessage = "RNTO test2.txt\r\n";
            p.ftpCommand(sMessage);

            sMessage = "RNFR test2.txt\r\n";
            p.ftpCommand(sMessage);

            sMessage = "RNTO test.txt\r\n";
            p.ftpCommand(sMessage);

            sMessage = "CWD test\r\n";
            p.ftpCommand(sMessage);

            sMessage = "MKD subdir\r\n";
            p.ftpCommand(sMessage);

            sMessage = "RMD subdir\r\n";
            p.ftpCommand(sMessage);

            sMessage = "QUIT\r\n";
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
