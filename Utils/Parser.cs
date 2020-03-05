using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RealTimeCacheApp.Utils
{
    public class Parser : IParser
    {
        public T Parse<T>(string path)
        {
            var text = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<T>(text);
            return result;
        }
    }
}
