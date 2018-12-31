using Assemblies.Ftp.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FileSystem
{
    public class StandardFileSystemClassFactory : IFileSystemClassFactory
    {
        public StandardFileSystemClassFactory()
        {

        }

        #region IFileSystemClassFactory Members

        public IFileSystem Create(string sUser, string sPassword)
        {
            if (UserData.Get().HasUser(sUser) && UserData.Get().GetUserPassword(sUser) == sPassword)
            {
                return new StandardFileSystemObject(UserData.Get().GetUserStartingDirectory(sUser));
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
