using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    /// <summary>
	/// Delete command handler
	/// </summary>
	public class DeleCommandHandler : FtpCommandHandler
    {
        public DeleCommandHandler(FtpConnectionObject connectionObject)
            : base("DELE", connectionObject)
        {

        }

        protected override string OnProcess(string sMessage)
        {
            string sFile = GetPath(sMessage);

            if (!ConnectionObject.FileSystemObject.FileExists(sFile))
            {
                return GetMessage(550, "File does not exist.");
            }

            if (!ConnectionObject.FileSystemObject.Delete(sFile))
            {
                return GetMessage(550, "Couldn't delete file.");
            }

            return GetMessage(250, "File deleted successfully");
        }
    }
}
