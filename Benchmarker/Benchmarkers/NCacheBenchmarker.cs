using Alachisoft.NCache.Client;
using BechmarkingFramework.Util;
using System;
using System.Diagnostics;
using System.Threading;

namespace BechmarkingFramework.Benchmarkers
{
    internal class NCacheBenchmarker : IBenchmarker
    {
        ICache _cache;

        BenchmarkConfiguration config;
        string[] keys;
        PerfStatsCollector _perfStats = new PerfStatsCollector();
        Random random = new Random();

        //-------------------------------------------------------
        public NCacheBenchmarker(BenchmarkConfiguration benchmarkConfiguration)
        {
            config = benchmarkConfiguration;

            _cache = CacheManager.GetCache(config.cacheName);
        }
        //-------------------------------------------------------
        public void AddToCache(object input)
        {
            var inputPair = (Tuple<int, int>)input;

            for (int i = inputPair.Item1; i <= inputPair.Item2; i++)
            {
                _cache.Insert(keys[i], GetPayload());
            }
        }
        //-------------------------------------------------------
        public byte[] GetPayload()
        {
            return new byte[100];
        }
        //-------------------------------------------------------
        public void PopulateCache()
        {
            PopulateKeys();

            if (_cache.Count == config.numberOfItems)
                return;

            var itemsPerThread = config.numberOfItems / config.numberOfThreads;

            int lowerIndex = 0;
            int upperIndex = itemsPerThread;

            for (int i = 0; i < config.numberOfThreads; i++)
            {
                new Thread(AddToCache).Start(new Tuple<int, int>(lowerIndex, upperIndex - 1));

                lowerIndex = upperIndex;
                upperIndex = itemsPerThread + upperIndex;
            }
        }
        //-------------------------------------------------------
        public void PopulateKeys()
        {
            keys = new string[config.numberOfItems];
            KeyGenerator keyGen = new KeyGenerator();

            for (int i = 0; i < config.numberOfItems; i++)
            {
                keys[i] = keyGen.generateKey(i);
            }
        }
        //-------------------------------------------------------
        public void RunFetchWork()
        {
            var rand = new Random();
            while (true)
            {
                try
                {
                    _cache.Get<byte[]>(keys[rand.Next(0, keys.Length)]);

                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    { 
                        _perfStats.IncrementFetches();
                        _perfStats.IncrementReqs();
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                }
            }
        }
        //-------------------------------------------------------
        public void RunUpdateWork()
        {
            var rand = new Random();
            while (true)
            {
                _cache.Insert(keys[rand.Next(0, keys.Length)], GetPayload());

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                { 
                    _perfStats.IncrementUpdates();
                    _perfStats.IncrementReqs();
                }
            }
        }
        //-------------------------------------------------------
        public void StartBenchmarking()
        {
            int threadsForFetches;
            int threadsForUpdates;

            if (config.updateRatio != 0 && config.fetchRatio != 0)
            {
                threadsForUpdates = (int)Math.Floor((((double)config.updateRatio / 100) * config.numberOfThreads));
                threadsForFetches = (int)Math.Ceiling((((double)config.fetchRatio / 100) * config.numberOfThreads));

                Console.WriteLine("Number of Threads for Updates: {0}", threadsForUpdates);
                Console.WriteLine("Number of Threads for Fetches: {0}", threadsForFetches);

                Thread[] updateThreads = updateThreads = new Thread[threadsForUpdates];
                Thread[] fetchThreads = fetchThreads = new Thread[threadsForFetches];

                PopulateCache();
                
                //Initiating Update Threads
                for (int i = 0; i < threadsForUpdates; i++)
                {
                    updateThreads[i] = new Thread(() => { RunUpdateWork(); });
                    updateThreads[i].Start();
                }

                //Initiating Fetch Threads
                for (int i = 0; i < threadsForFetches; i++)
                {
                    fetchThreads[i] = new Thread(() => { RunFetchWork(); });
                    fetchThreads[i].Start();
                }
            }
            else if(config.fetchRatio != 0)
            {
                threadsForFetches = config.numberOfThreads;

                PopulateCache();

                for (int i = 0; i < threadsForFetches; i++)
                {
                    new Thread(RunFetchWork).Start();
                }
            }
            else if(config.updateRatio != 0)
            {
                threadsForUpdates = config.numberOfThreads;

                PopulateCache();

                for (int i = 0; i < threadsForUpdates; i++)
                    new Thread(RunUpdateWork).Start();
            }
        }
        //-------------------------------------------------------
    }
}
