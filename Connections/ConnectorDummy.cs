using RealTimeCacheApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp.Connections
{
    public class ConnectorDummy : IConnection
    {
        private readonly ConnectionData _connect;
        private readonly List<TradeData> _trades = new List<TradeData>
        {
            new TradeData(Exchange.Binance, 1, 2, 3, SideType.Buy, DateTime.UtcNow, "1"),
            new TradeData(Exchange.Binance, 1, 2, 3, SideType.Buy, DateTime.UtcNow, "1"),
            new TradeData(Exchange.Binance, 1, 2, 3, SideType.Buy, DateTime.UtcNow, "1"),
            new TradeData(Exchange.Binance, 1, 2, 3, SideType.Buy, DateTime.UtcNow, "1"),
            new TradeData(Exchange.Binance, 1, 2, 3, SideType.Buy, DateTime.UtcNow, "1"),
            new TradeData(Exchange.Binance, 1, 2, 3, SideType.Buy, DateTime.UtcNow, "1")
        };

        public ConnectorDummy(ConnectionData connection)
        {
            _connect = connection;
        }

        public ConnectionData Connection => _connect;
        public async Task<IEnumerable<TradeData>> GetTradeDataAsync()
        {
            await Task.Delay(1000);
            Logger.Log("Recieve data from Rest");
            return await Task.FromResult(_trades);
        }

        public async Task<IEnumerable<TradeData>> GetTradeDataAsync(string instrument, DateTime start, DateTime end)
        {
            await Task.Delay(1000);
            Logger.Log("Recieve data from Rest");
            return await Task.FromResult(_trades);
        }

        public async Task<IEnumerable<TradeData>> GetTradeDataSocketAsync()
        {
            await Task.Delay(1500);
            Logger.Log("Recieve data from Ws");
            return await Task.FromResult(_trades);
        }
        
    }
}
