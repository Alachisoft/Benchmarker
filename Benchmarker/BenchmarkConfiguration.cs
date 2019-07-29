using System;
using System.Configuration;

namespace Benchmarker
{
    public class BenchmarkConfiguration
    {
        public static string ProviderFQN { get; internal set; }
        public static string CacheName { get; internal set; }
        public static int NumberOfThreads { get; internal set; }
        public static long NumberOfItems { get; internal set; }
        public static int FetchRatio { get; internal set; } = 80;
        public static int UpdateRatio { get; internal set; } = 20;
        public static int Payload { get; internal set; } = 100;

        static BenchmarkConfiguration()
        {
            Load();
        }

        private static void Load()
        {
            ProviderFQN = ConfigurationManager.AppSettings["provider-fqn"];
            CacheName = ConfigurationManager.AppSettings["cache-name"];
            NumberOfThreads = Int32.Parse(ConfigurationManager.AppSettings["num-threads-per-application"]);
            NumberOfItems = long.Parse(ConfigurationManager.AppSettings["total-entries"]);
            FetchRatio = Int32.Parse(ConfigurationManager.AppSettings["get-ratio"]);
            UpdateRatio = Int32.Parse(ConfigurationManager.AppSettings["put-ratio"]);
            if (ConfigurationManager.AppSettings["payload"] != null)
                Payload = Int32.Parse(ConfigurationManager.AppSettings["payload"]);
        }
    }
}
