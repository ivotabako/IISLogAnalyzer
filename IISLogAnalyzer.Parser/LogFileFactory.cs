using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogAnalyzer.Parser
{
    /// <summary>
    /// This factory is necessary since there are three types of log file
    /// I have implemented only one of the three.
    /// for more info: https://msdn.microsoft.com/en-us/library/ms525807(v=vs.90).aspx
    /// </summary>
    public class LogFileFactory
    {
        public ILogFileFormat Get(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("#Software:"))
                {
                    return new W3CExtendedLogFile(lines);
                }
            }

            return null;
        }
    }
}
