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

        public bool Login(string sPassword)
        {
            FileSystem.IFileSystem fileSystem = m_fileSystemClassFactory.Create(this.User, sPassword);

            if (fileSystem == null)
            {
                return false;
            }

            SetFileSystemObject(fileSystem);
            return true;
        }

        private void LoadCommands()
        {
            AddCommand(new FtpCommands.UserCommandHandler(this));
            AddCommand(new FtpCommands.PasswordCommandHandler(this));
            AddCommand(new FtpCommands.PwdCommandHandler(this));
            AddCommand(new FtpCommands.ListCommandHandler(this));
            AddCommand(new FtpCommands.CwdCommandHandler(this));

            AddCommand(new FtpCommands.MakeDirectoryCommandHandler(this));
            AddCommand(new FtpCommands.RemoveDirectoryCommandHandler(this));
            AddCommand(new FtpCommands.XMkdCommandHandler(this));
            AddCommand(new FtpCommands.XPwdCommandHandler(this));
            AddCommand(new FtpCommands.XRmdCommandHandler(this));

            AddCommand(new FtpCommands.NlstCommandHandler(this));
            AddCommand(new FtpCommands.NoopCommandHandler(this));
            AddCommand(new FtpCommands.QuitCommandHandler(this));
            AddCommand(new FtpCommands.SizeCommandHandler(this));
            AddCommand(new FtpCommands.AlloCommandHandler(this));

            AddCommand(new FtpCommands.RenameStartCommandHandler(this));
            AddCommand(new FtpCommands.RenameCompleteCommandHandler(this));
            AddCommand(new FtpCommands.DeleCommandHandler(this));
            //AddCommand(new FtpCommands.PortCommandHandler(this));
            //AddCommand(new FtpCommands.PasvCommandHandler(this));

            AddCommand(new FtpCommands.TypeCommandHandler(this));
            //AddCommand(new FtpCommands.RetrCommandHandler(this));
            //AddCommand(new FtpCommands.StoreCommandHandler(this));
            //AddCommand(new FtpCommands.AppendCommandHandler(this));
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
