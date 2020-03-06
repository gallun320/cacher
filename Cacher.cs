using RealTimeCacheApp.Connections;
using RealTimeCacheApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp
{
    public class Cacher
    {
        private readonly List<IConnection> _connections;
        private readonly CacheWriter _cacheWriter;
        public TimeSpan CacheIntervalInMinutes { get; set; }
        public TimeSpan FileIntervalInMinutes { get; set; }

        public Cacher(int fileIntervalInMinutes, int cacheIntervalInMinutes, IEnumerable<IConnection> connections)
        {
            CacheIntervalInMinutes = new TimeSpan(0, 0, cacheIntervalInMinutes, 0, 0);
            FileIntervalInMinutes = new TimeSpan(0, 0, fileIntervalInMinutes, 0, 0);
            _cacheWriter = new CacheWriter(FileIntervalInMinutes);
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
                    cacheLocal.AddRange(await connection.GetTradeDataAsync());

                    await _cacheWriter.WriteToLogNew(cacheLocal, file);
                }
                else
                {
                    cacheLocal.AddRange(await connection.GetTradeDataAsync(connection.Connection.InstrumentName, DateTime.UtcNow - FileIntervalInMinutes, DateTime.UtcNow));

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
