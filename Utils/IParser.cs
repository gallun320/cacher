using System;
using System.Collections.Generic;
using System.Text;

namespace RealTimeCacheApp.Utils
{
    public interface IParser
    {
        T Parse<T>(string path);
    }
}
