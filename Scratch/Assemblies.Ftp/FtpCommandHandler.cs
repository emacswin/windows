using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    public class FtpCommandHandler
    {
        #region Member Variables

        private string m_sCommand;
        private FtpConnectionObject m_theConnectionObject = null;

        #endregion

        #region Construction

        public FtpCommandHandler(string sCommand, FtpConnectionObject connectionObject)
        {
            m_sCommand = sCommand;
            m_theConnectionObject = connectionObject;
        }

        #endregion

        public string Command
        {
            get
            {
                return m_sCommand;
            }
        }

        public FtpConnectionObject ConnectionObject
        {
            get
            {
                return m_theConnectionObject;
            }
        }

        public void Process(string sMessage)
        {
            SendMessage(OnProcess(sMessage));
        }

        protected virtual string OnProcess(string sMessage)
        {
            System.Diagnostics.Debug.Assert(false, "FtpCommandHandler::OnProcess base called");
            return "";
        }

        protected string GetMessage(int nReturnCode, string sMessage)
        {
            return string.Format("{0} {1}\r\n", nReturnCode, sMessage);
        }

        protected string GetPath(string sPath)
        {
            if (sPath.Length == 0)
            {
                return m_theConnectionObject.CurrentDirectory;
            }

            sPath = sPath.Replace('/', '\\');

            return Path.Combine(m_theConnectionObject.CurrentDirectory, sPath);
        }

        private void SendMessage(string sMessage)
        {
            if (sMessage.Length == 0)
            {
                return;
            }

            int nEndIndex = sMessage.IndexOf('\r');

            Console.WriteLine(sMessage);

            /*
             * if (nEndIndex < 0)
            {
                FtpServerMessageHandler.SendMessage(m_theConnectionObject.Id, sMessage);
            }
            else
            {
                FtpServerMessageHandler.SendMessage(m_theConnectionObject.Id, sMessage.Substring(0, nEndIndex));
            }

            Assemblies.General.SocketHelpers.Send(ConnectionObject.Socket, sMessage);
            */
        }
    }
}
