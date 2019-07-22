using BechmarkingFramework.Util;
using System;
using System.Configuration;

namespace BechmarkingFramework
{
    public class BenchmarkConfiguration
    {
        public Provider Provider { get; internal set; }
        public string cacheName { get; internal set; }
        public int numberOfThreads { get; internal set; }
        public int numberOfItems { get; internal set; }
        public int updateRatio { get; internal set; }
        public int fetchRatio { get; internal set; }

        public static BenchmarkConfiguration FromAppConfig()
        {
            var benchConfig = new BenchmarkConfiguration();

            var providerVal = ConfigurationManager.AppSettings["Provider"];
            benchConfig.Provider = (Provider)Enum.Parse(typeof(Provider), providerVal);
            benchConfig.cacheName = ConfigurationManager.AppSettings["cache-name"];
            benchConfig.numberOfThreads = Int32.Parse(ConfigurationManager.AppSettings["num-threads-per-application"]);
            benchConfig.numberOfItems = Int32.Parse(ConfigurationManager.AppSettings["total-entries"]);
            benchConfig.updateRatio = Int32.Parse(ConfigurationManager.AppSettings["put-ratio"]);
            benchConfig.fetchRatio = Int32.Parse(ConfigurationManager.AppSettings["get-ratio"]);

            return benchConfig;
        }
    }
}
