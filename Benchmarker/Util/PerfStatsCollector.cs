using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BechmarkingFramework.Util
{
    internal class PerfStatsCollector
    {
        private const string PC_CATEGORY = "Benchmarking";
        private const string categoryHelp = "category help description..";

        private const string REQ_PER_SEC = "Req/sec";
        private const string UPDATES_PER_SEC = "Updates/sec";
        private const string FETCHES_PER_SEC = "Fetches/sec";

        private PerformanceCounter _reqsPerSec = null;
        private PerformanceCounter _updatesPerSec = null;
        private PerformanceCounter _fetchesPerSec = null;

        internal PerfStatsCollector()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (!PerformanceCounterCategory.Exists(PC_CATEGORY))
                {
                    PerformanceCounterCategory customCategory = new PerformanceCounterCategory(PC_CATEGORY);
                    CounterCreationData reqs = new CounterCreationData(REQ_PER_SEC, REQ_PER_SEC, PerformanceCounterType.RateOfCountsPerSecond32);
                    CounterCreationData updates = new CounterCreationData(UPDATES_PER_SEC, UPDATES_PER_SEC, PerformanceCounterType.RateOfCountsPerSecond32);
                    CounterCreationData fetches = new CounterCreationData(FETCHES_PER_SEC, FETCHES_PER_SEC, PerformanceCounterType.RateOfCountsPerSecond32);

                    CounterCreationDataCollection collection = new CounterCreationDataCollection();
                    collection.Add(reqs);
                    collection.Add(updates);
                    collection.Add(fetches);

                    PerformanceCounterCategory.Create(PC_CATEGORY, categoryHelp, PerformanceCounterCategoryType.SingleInstance, collection);
                }
                _reqsPerSec = new PerformanceCounter(PC_CATEGORY, REQ_PER_SEC, false);
                _updatesPerSec = new PerformanceCounter(PC_CATEGORY, UPDATES_PER_SEC, false);
                _fetchesPerSec = new PerformanceCounter(PC_CATEGORY, FETCHES_PER_SEC, false); 
            }
        }

        internal void IncrementReqs()
        {
            _reqsPerSec?.Increment();
        }

        internal void IncrementUpdates()
        {
            _updatesPerSec?.Increment();
        }

        internal void IncrementFetches()
        {
            _fetchesPerSec?.Increment();
        }
    }
}
