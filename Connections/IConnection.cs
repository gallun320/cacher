using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp.Connections
{
    public interface IConnection
    {
        ConnectionData Connection { get; }
        Task<IEnumerable<TradeData>> GetTradeDataAsync(string instrument, DateTime start, DateTime end);
        Task<IEnumerable<TradeData>> GetTradeDataSocketAsync();
    }
}
