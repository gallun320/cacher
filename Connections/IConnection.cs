using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp.Connections
{
    public interface IConnection
    {
        ConnectionData Connection { get; }
        Task<IEnumerable<TradeData>> GetTradeDataAsync();
        Task<IEnumerable<TradeData>> GetTradeDataSocketAsync();
    }
}
