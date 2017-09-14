using IISLogAnalyzer.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IISLogAnalyzer.Parser
{
    public class W3CExtendedLogFile : ILogFileFormat
    {        
        private string[] fileLines;
       
        public W3CExtendedLogFile(string[] lines)
        {
            fileLines = lines;
        }

        public List<LogEntryAggregate> Analyze()
        {
            Dictionary<string, int> clientCalls = new Dictionary<string, int>();

            List<LogEntryAggregate> result = new List<LogEntryAggregate>();

            int clientIPIndex = -1;

            for (int i = 0; i < fileLines.Length; i++)
            {
                clientIPIndex = FindNextClietIPIndex(i, clientIPIndex);
                if (clientIPIndex == -1 || IsMetadataRow(i))
                {
                    continue;
                }

                string[] fields = fileLines[i].Split(' ');
                if (clientCalls.ContainsKey( fields[clientIPIndex]) == false)
                {
                    clientCalls.Add(fields[clientIPIndex], 1);
                }
                else
                {
                    clientCalls[fields[clientIPIndex]]++;
                }              
            }

            result = IncludeDomainName(clientCalls);

            return result;
        }

        private bool IsMetadataRow(int rowNum)
        {
            bool isMetadata = false;
            if (fileLines[rowNum].StartsWith("#Software:") ||
                fileLines[rowNum].StartsWith("#Version:") ||
                fileLines[rowNum].StartsWith("#Date:") ||
                fileLines[rowNum].StartsWith("#Fields:")
                )
            {
                isMetadata = true;
            }
            return isMetadata;
        }

        private List<LogEntryAggregate> IncludeDomainName(Dictionary<string, int> clientCalls)
        {
            List<LogEntryAggregate> result = new List<LogEntryAggregate>();

            foreach (var ipAddress in clientCalls.Keys)
            {
                string hostName = string.Empty;
                try
                {
                    var hostEntry = Dns.GetHostEntry(ipAddress);
                    hostName = hostEntry.HostName;
                }
                catch (Exception e)
                {
                    hostName = e.Message;
                }                

                result.Add(new LogEntryAggregate()
                    { CallCount = clientCalls[ipAddress],
                      IPAddress = ipAddress,
                      DomainName = hostName
                    }
                );
            }

            return result;
        }        

        private int FindNextClietIPIndex(int rowNum, int oldClientIPIndex)
        {
            if (fileLines[rowNum].StartsWith("#Fields:"))
            {
                string[] fields = fileLines[rowNum].Split(' ');
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i] == "c-ip")
                    {
                        // subtract 1 because the header line starts with '#Fields:'
                        return i - 1;
                    }
                }
            }

            return oldClientIPIndex;
        }
    }
}
