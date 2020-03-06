using RealTimeCacheApp.Connections;
using RealTimeCacheApp.Utils;
using RealTimeCacheApp.Cacher;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp
{
    public class Startup
    {
        private readonly IParser _parser;
        private readonly List<IConnection> _connections;
        private Cache _cacher;
        public Startup()
        {
            _parser = new Parser();
            _connections = new List<IConnection>();
        }

        public void Init()
        {
            Logger.Log("Start init");
            var config = _parser.Parse<Config>(ConfigPaths.ConfigPath);

            foreach(var connection in config.ConnectionsList)
            {
                _connections.Add(new ConnectorDummy(connection));
            }

            _cacher = new Cache(config.FileIntervalInMinutes, config.CacheIntervalInMinutes, _connections);
            Logger.Log("Init complete");
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                await _cacher.Init();
                await _cacher.Caching();
            }).Wait();
        }
    }
}
