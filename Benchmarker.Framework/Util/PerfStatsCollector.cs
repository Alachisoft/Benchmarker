using System;
using System.Diagnostics;

namespace Benchmarker.Framework.Util
{
    public class PerfStatsCollector
    {
        private const string PC_CATEGORY = "Benchmarker";
        private const string PC_CATEGORY_HELP = "Cache benchmarking to measure the NCache perforance.";

        private const string REQUESTS_PER_SEC = "Requests/sec";
        private const string UPDATES_PER_SEC = "Updates/sec";
        private const string FETCHES_PER_SEC = "Fetches/sec";

        private PerformanceCounter _reqsPerSec = null;
        private PerformanceCounter _updatesPerSec = null;
        private PerformanceCounter _fetchesPerSec = null;

        public PerfStatsCollector()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (!PerformanceCounterCategory.Exists(PC_CATEGORY))
                {
                    PerformanceCounterCategory customCategory = new PerformanceCounterCategory(PC_CATEGORY);
                    CounterCreationData reqs = new CounterCreationData(REQUESTS_PER_SEC, REQUESTS_PER_SEC, PerformanceCounterType.RateOfCountsPerSecond32);
                    CounterCreationData updates = new CounterCreationData(UPDATES_PER_SEC, UPDATES_PER_SEC, PerformanceCounterType.RateOfCountsPerSecond32);
                    CounterCreationData fetches = new CounterCreationData(FETCHES_PER_SEC, FETCHES_PER_SEC, PerformanceCounterType.RateOfCountsPerSecond32);

                    CounterCreationDataCollection countersCollection = new CounterCreationDataCollection();
                    countersCollection.Add(reqs);
                    countersCollection.Add(updates);
                    countersCollection.Add(fetches);

                    PerformanceCounterCategory.Create(PC_CATEGORY, PC_CATEGORY_HELP, PerformanceCounterCategoryType.SingleInstance, countersCollection);
                }
                _reqsPerSec = new PerformanceCounter(PC_CATEGORY, REQUESTS_PER_SEC, false);
                _updatesPerSec = new PerformanceCounter(PC_CATEGORY, UPDATES_PER_SEC, false);
                _fetchesPerSec = new PerformanceCounter(PC_CATEGORY, FETCHES_PER_SEC, false); 
            }
        }

        public void IncrementReqs()
        {
            _reqsPerSec?.Increment();
        }

        public void IncrementUpdates()
        {
            _updatesPerSec?.Increment();
        }

        public void IncrementFetches()
        {
            _fetchesPerSec?.Increment();
        }
    }
}
