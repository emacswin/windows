using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp
{
    public class FtpConnectionObject : FtpConnectionData
    {
        #region Member Variables

        private System.Collections.Hashtable m_theCommandHashTable = null;
        private FileSystem.IFileSystemClassFactory m_fileSystemClassFactory = null;

        #endregion

        public FtpConnectionObject(FileSystem.IFileSystemClassFactory fileSystemClassFactory, int nId, System.Net.Sockets.TcpClient socket)
            : base(nId, socket)
        {
            m_theCommandHashTable = new System.Collections.Hashtable();
            m_fileSystemClassFactory = fileSystemClassFactory;

            LoadCommands();
        }

        private void LoadCommands()
        {
            AddCommand(new FtpCommands.UserCommandHandler(this));
        }

        private void AddCommand(FtpCommands.FtpCommandHandler handler)
        {
            m_theCommandHashTable.Add(handler.Command, handler);
        }

        public FtpCommands.FtpCommandHandler getCommandHandler(string commandName)
        {
            FtpCommands.FtpCommandHandler handler = m_theCommandHashTable[commandName] as FtpCommands.FtpCommandHandler;
            return handler;
        }
    }
}
