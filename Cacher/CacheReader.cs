using RealTimeCacheApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RealTimeCacheApp.Cacher
{
    public class CacheReader
    {
        public TimeWithData ReadLastFromLog(string fileName)
        {
            var path = ConfigPaths.DataPath + fileName + ".txt";
            var txt = File.ReadLines(path).Last();
            var data = ConvertToTimeWithData(txt);
            return data;
        }

        private TimeWithData ConvertToTimeWithData(string data)
        {
            var arrStr = data.Split(":");
            return new TimeWithData { Time = Convert.ToInt64(arrStr[0]), Data = arrStr[1] };
        }

    }
}
