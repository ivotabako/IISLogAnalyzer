using IISLogAnalyzer.Parser.Models;
using System.Collections.Generic;

namespace IISLogAnalyzer.Parser
{
    public interface ILogFileFormat
    {
        List<LogEntryAggregate> Analyze();
    }
}