using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    public class ListCommandHandler : ListCommandHandlerBase
    {
        public ListCommandHandler(FtpConnectionObject connectionObject)
            : base("LIST", connectionObject)
        {

        }

        protected override string BuildReply(string sMessage, string[] asFiles)
        {
            return BuildLongReply(asFiles);
        }
    }
}
