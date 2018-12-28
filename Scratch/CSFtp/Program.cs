using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFtp
{
    class Program
    {
        static void Main(string[] args)
        {
            string sMessage = "USER wudi\r\n";

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

            FtpCommands.FtpCommandHandler handler = m_theCommandHashTable[sCommand] as FtpCommands.FtpCommandHandler;

            if (handler == null)
            {
                Console.WriteLine("550 Unknown command\r\n");
            }
            else
            {
                handler.Process(sValue);
            }
        }
    }
}
