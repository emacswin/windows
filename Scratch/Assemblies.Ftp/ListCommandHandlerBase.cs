﻿using Assemblies.Ftp.FtpCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Ftp.FtpCommands
{
    public abstract class ListCommandHandlerBase : FtpCommandHandler
    {
        public ListCommandHandlerBase(string sCommand, FtpConnectionObject connectionObject)
            : base(sCommand, connectionObject)
        {

        }

        protected override string OnProcess(string sMessage)
        {
            Console.WriteLine("150 Opening data connection for LIST\r\n");
            //Assemblies.General.SocketHelpers.Send(ConnectionObject.Socket, "150 Opening data connection for LIST\r\n");

            string[] asFiles = null;
            string[] asDirectories = null;

            sMessage = sMessage.Trim();

            string sPath = GetPath("");

            if (sMessage.Length == 0 || sMessage[0] == '-')
            {
                asFiles = ConnectionObject.FileSystemObject.GetFiles(sPath);
                asDirectories = ConnectionObject.FileSystemObject.GetDirectories(sPath);
            }
            else
            {
                asFiles = ConnectionObject.FileSystemObject.GetFiles(sPath, sMessage);
                asDirectories = ConnectionObject.FileSystemObject.GetDirectories(sPath, sMessage);
            }

            string[] asAll = Assemblies.General.ArrayHelpers.Add(asDirectories, asFiles) as string[];
            string sFileList = BuildReply(sMessage, asAll);

            /*FtpReplySocket socketReply = new FtpReplySocket(ConnectionObject);

            if (!socketReply.Loaded)
            {
                return GetMessage(550, "LIST unable to establish return connection.");
            }

            socketReply.Send(sFileList);
            socketReply.Close();
            */

            Console.WriteLine(sFileList);

            return GetMessage(226, "LIST successful.");
        }

        protected abstract string BuildReply(string sMessage, string[] asFiles);

        protected string BuildShortReply(string[] asFiles)
        {
            string sFileList = string.Join("\r\n", asFiles);
            sFileList += "\r\n";
            return sFileList;
        }

        protected string BuildLongReply(string[] asFiles)
        {
            string sDirectory = GetPath("");

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            for (int nIndex = 0; nIndex < asFiles.Length; nIndex++)
            {
                string sFile = asFiles[nIndex];
                sFile = Path.Combine(sDirectory, sFile);

                FileSystem.IFileInfo info = ConnectionObject.FileSystemObject.GetFileInfo(sFile);

                if (info != null)
                {
                    string sAttributes = info.GetAttributeString();
                    stringBuilder.Append(sAttributes);
                    stringBuilder.Append(" 1 owner group");

                    if (info.IsDirectory())
                    {
                        stringBuilder.Append("            1 ");
                    }
                    else
                    {
                        string sFileSize = info.GetSize().ToString();
                        stringBuilder.Append(General.TextHelpers.RightAlignString(sFileSize, 13, ' '));
                        stringBuilder.Append(" ");
                    }

                    System.DateTime fileDate = info.GetModifiedTime();

                    string sDay = fileDate.Day.ToString();

                    stringBuilder.Append(General.TextHelpers.Month(fileDate.Month));
                    stringBuilder.Append(" ");

                    if (sDay.Length == 1)
                    {
                        stringBuilder.Append(" ");
                    }

                    stringBuilder.Append(sDay);
                    stringBuilder.Append(" ");
                    stringBuilder.Append(string.Format("{0:hh}", fileDate));
                    stringBuilder.Append(":");
                    stringBuilder.Append(string.Format("{0:mm}", fileDate));
                    stringBuilder.Append(" ");

                    stringBuilder.Append(asFiles[nIndex]);
                    stringBuilder.Append("\r\n");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
