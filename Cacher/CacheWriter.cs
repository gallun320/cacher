using RealTimeCacheApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp.Cacher
{
    public class CacheWriter
    {
        private TimeSpan Delay; 

        public CacheWriter(TimeSpan delay)
        {
            Delay = delay;
        }

        public bool CheckIfFileExistForConnection(string fileName)
        {
            var path = ConfigPaths.DataPath + fileName + ".txt";
            return File.Exists(path);
        }

        public async Task WriteToLogNew(IEnumerable<TradeData> dataset, string fileName)
        {
            var path = ConfigPaths.DataPath + fileName + ".txt";
            Logger.Log("Write to new file");
            using (var sw = File.CreateText(path))
            {
                foreach (var data in dataset)
                {
                    await sw.WriteLineAsync(data.ToString());
                }
            }
        }

        public async Task WriteToLog(IEnumerable<TradeData> dataset, string fileName)
        {
            var path = ConfigPaths.DataPath + fileName + ".txt";
            var lines = await File.ReadAllLinesAsync(path);
            File.Delete(path);
            var correctDataForFile = CorrectingFileData(lines, dataset);
            using (var sw = File.CreateText(path))
            {
                Logger.Log("Write to old file");
                foreach (var data in correctDataForFile)
                {
                    await sw.WriteLineAsync(data);
                }
            }
        }


        private IEnumerable<string> CorrectingFileData(IEnumerable<string> lines, IEnumerable<TradeData> dataset)
        {
            var lineList = lines.ToList();
            if(ConvertToTimeWithData(lineList.Last()).Time < new DateTimeOffset(DateTime.UtcNow - Delay).ToUnixTimeMilliseconds())
            {
                return dataset.Select(dt => dt.ToString());
            }

            var filteredList = lineList.Where(line => ConvertToTimeWithData(line).Time >= new DateTimeOffset(DateTime.UtcNow - Delay).ToUnixTimeMilliseconds())
                                       .Select(line => line)
                                       .ToList();

            filteredList.AddRange(dataset.Select(dt => dt.ToString()));

            return filteredList;
        }

        private TimeWithData ConvertToTimeWithData(string data)
        {
            var arrStr = data.Split(":");
            return new TimeWithData { Time = Convert.ToInt64(arrStr[0]), Data = arrStr[1] };
        }
    }
}
