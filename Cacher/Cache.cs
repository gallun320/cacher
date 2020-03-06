using RealTimeCacheApp.Connections;
using RealTimeCacheApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp.Cacher
{
    public class Cache
    {
        private readonly List<IConnection> _connections;
        private readonly CacheWriter _cacheWriter;
        private readonly CacheReader _cacheReader;
        public TimeSpan CacheIntervalInMinutes { get; set; }
        public TimeSpan FileIntervalInMinutes { get; set; }

        public Cache(int fileIntervalInMinutes, int cacheIntervalInMinutes, IEnumerable<IConnection> connections)
        {
            CacheIntervalInMinutes = new TimeSpan(0, 0, cacheIntervalInMinutes, 0, 0);
            FileIntervalInMinutes = new TimeSpan(0, 0, fileIntervalInMinutes, 0, 0);
            _cacheWriter = new CacheWriter(FileIntervalInMinutes);
            _cacheReader = new CacheReader();
            _connections = connections.ToList();
        }

        public async Task Init()
        {
            var cacheLocal = new List<TradeData>();
            foreach (var connection in _connections)
            {
                var file = $"{connection.Connection.Exchange}_{connection.Connection.InstrumentName}";
                if (!_cacheWriter.CheckIfFileExistForConnection(file))
                {
                    cacheLocal.AddRange(await connection.GetTradeDataAsync(connection.Connection.InstrumentName, DateTime.UtcNow - FileIntervalInMinutes, DateTime.UtcNow));

                    await _cacheWriter.WriteToLogNew(cacheLocal, file);
                }
                else
                {
                    var data = _cacheReader.ReadLastFromLog(file);
                    cacheLocal.AddRange(await connection.GetTradeDataAsync(connection.Connection.InstrumentName, new DateTime(data.Time), DateTime.UtcNow));

                    await _cacheWriter.WriteToLog(cacheLocal, file);
                }
                
            }
            
        }

        public async Task Caching()
        {
            var cacheLocal = new List<TradeData>();
            var timeMarker = DateTime.UtcNow + CacheIntervalInMinutes;
            while (true)
            {
                foreach (var connection in _connections)
                {
                    cacheLocal.AddRange(await connection.GetTradeDataSocketAsync());
                    if (timeMarker < DateTime.UtcNow)
                    {
                        await _cacheWriter.WriteToLog(cacheLocal, $"{connection.Connection.Exchange}_{connection.Connection.InstrumentName}");
                        timeMarker = DateTime.UtcNow + CacheIntervalInMinutes;
                        cacheLocal = new List<TradeData>();
                    }
                }
            }
        }
    }
}
