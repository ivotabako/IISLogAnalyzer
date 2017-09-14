using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IISLogAnalyzer.Parser.Models
{
    public class LogEntryAggregate
    {
        public string IPAddress { get; set; }
        public string DomainName { get; set; }
        public int CallCount { get; set; }
    }
}