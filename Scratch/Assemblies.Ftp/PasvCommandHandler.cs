using System;

namespace Assemblies.Ftp.FtpCommands
{
    class PasvCommandHandler : FtpCommandHandler
    {
        const int m_nPort = 20;

        public PasvCommandHandler(FtpConnectionObject connectionObject)
            : base("PASV", connectionObject)
        {

        }

        protected override string OnProcess(string sMessage)
        {
            if (ConnectionObject.PasvSocket == null)
            {
                /*System.Net.Sockets.TcpListener listener = Assemblies.General.SocketHelpers.CreateTcpListener(m_nPort);

                if (listener == null)
                {
                    return GetMessage(550, string.Format("Couldn't start listener on port {0}", m_nPort));
                }*/

                SendPasvReply();

                /*listener.Start();

                ConnectionObject.PasvSocket = listener.AcceptTcpClient();

                listener.Stop();
                return "";*/



            }
            else
            {
                SendPasvReply();
            }
            return "";
        }

        private void SendPasvReply()
        {
            string sIpAddress = Assemblies.General.SocketHelpers.GetLocalAddress().ToString();
            sIpAddress = sIpAddress.Replace('.', ',');
            sIpAddress += ',';
            sIpAddress += "0";
            sIpAddress += ',';
            sIpAddress += m_nPort.ToString();
            //Assemblies.General.SocketHelpers.Send(ConnectionObject.Socket, string.Format("227 ={0}\r\n", sIpAddress));
            Console.WriteLine(string.Format("227 ={0}\r\n", sIpAddress));
        }
    }
}
