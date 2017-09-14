using IISLogAnalyzer.Parser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace IISLogAnalyzer.Parser
{
    public class ParseService
    {
        private static MemoryCache _cache = MemoryCache.Default;        

        public List<LogEntryAggregate> ParseLogFileType(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            if (_cache.Contains(fileName))
                return _cache.Get(fileName) as List<LogEntryAggregate>;
            else
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddDays(1);

                string[] lines = System.IO.File.ReadAllLines(filePath);

                List<LogEntryAggregate> result = new List<LogEntryAggregate>();

                LogFileFactory factory = new LogFileFactory();
                ILogFileFormat logFile = factory.Get(lines);                
                result = logFile.Analyze();                

                _cache.Add(fileName, result, cacheItemPolicy);

                return result;
            }                  
        }
    }
}
