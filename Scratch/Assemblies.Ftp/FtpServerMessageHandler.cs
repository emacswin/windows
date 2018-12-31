using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp
{
    class FtpServerMessageHandler
    {
        public delegate void MessageEventHandler(int nId, string sMessage);
        static public event MessageEventHandler Message;

        protected FtpServerMessageHandler()
        {
        }

        public static void SendMessage(int nId, string sMessage)
        {
            if (Message != null)
            {
                Message(nId, sMessage);
            }
        }
    }
}
