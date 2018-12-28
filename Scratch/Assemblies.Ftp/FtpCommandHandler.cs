using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    class FtpCommandHandler
    {
        #region Member Variables

        private string m_sCommand;
        private FtpConnectionObject m_theConnectionObject = null;

        #endregion

        #region Construction

        protected FtpCommandHandler(string sCommand, FtpConnectionObject connectionObject)
        {
            m_sCommand = sCommand;
            m_theConnectionObject = connectionObject;
        }

        #endregion
    }
}
