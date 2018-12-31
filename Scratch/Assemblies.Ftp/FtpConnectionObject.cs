﻿using System;
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
