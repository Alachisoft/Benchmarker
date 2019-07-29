using Benchmarker.Framework;
using Benchmarker.Framework.Util;
using System;
using System.Threading;

namespace Benchmarker
{
    public class Benchmarker
    {
        IBenchmarkableCache _cache;
        
        PerfStatsCollector _perfStats = new PerfStatsCollector();

        int threadsForFetches;
        int threadsForUpdates;
        
        public Benchmarker(IBenchmarkableCache cache)
        {
            _cache = cache;
        }

        private void PopulateCache(long numberOfItems, int numberOfThreads, int payLoad)
        {
            Console.WriteLine("Data is being loaded in cache.");

            var itemsPerThread = numberOfItems / numberOfThreads;
            
            long lowerIndex = 0;
            long upperIndex = itemsPerThread - 1;
            Thread[] threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(() => { AddToCache(lowerIndex, upperIndex, payLoad); });
                threads[i].Start();

                Thread.Sleep(200);

                lowerIndex = upperIndex;
                upperIndex = itemsPerThread + upperIndex;
            }

            foreach(Thread thread in threads)
            {
                thread.Join();
            }
        }

        private void Get(long numberOfItems)
        {
            var keyGenerator = new KeyGenerator();
            var rand = new Random();
            
            while (true)
            {
                try
                {
                    var keyIndex = rand.Next(0, Convert.ToInt32(numberOfItems));

                    _cache.Get(keyGenerator.Generate(keyIndex));

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

        private void Update(long numberOfItems, int payLoad)
        {
            var keyGenerator = new KeyGenerator();
            var rand = new Random();

            while (true)
            {
                var keyIndex = rand.Next(0, Convert.ToInt32(numberOfItems));
                _cache.Update(keyGenerator.Generate(keyIndex), GetPayload(payLoad));

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    _perfStats.IncrementUpdates();
                    _perfStats.IncrementReqs();
                }
            }
        }

        private void AddToCache(long lowerIndex, long upperIndex, int payLoad)
        {
            var keyGenerator = new KeyGenerator();
            
            for (long i = lowerIndex; i <= upperIndex; i++)
            {
                var key = keyGenerator.Generate(i);
                _cache.Update(key, GetPayload(payLoad));
            }
        }

        private byte[] GetPayload(int payLoad)
        {
            return new byte[payLoad];
        }

        public void Run(string cacheName ,long numberOfItems, int numberOfThreads, int payLoad, int fetchRatio = 80, int updateRatio = 20, int deleteRatio = 0)
        {
            InitializeCache(cacheName);
            
            PopulateCache(numberOfItems, numberOfThreads, payLoad);
            
            if (updateRatio != 0 && fetchRatio != 0)
            {
                threadsForUpdates = (int)Math.Floor((((double)updateRatio / 100) * numberOfThreads));
                threadsForFetches = (int)Math.Ceiling((((double)fetchRatio / 100) * numberOfThreads));

                Console.Clear();
                Console.WriteLine("Number of Threads for Updates: {0}", threadsForUpdates);
                Console.WriteLine("Number of Threads for Fetches: {0}", threadsForFetches);

                Thread[] updateThreads = new Thread[threadsForUpdates];
                Thread[] fetchThreads = new Thread[threadsForFetches];

                for (int i = 0; i < threadsForUpdates; i++)
                {
                    updateThreads[i] = new Thread(() => { Update(numberOfItems, payLoad); });
                    updateThreads[i].Start();
                }

                for (int i = 0; i < threadsForFetches; i++)
                {
                    fetchThreads[i] = new Thread(() => { Get(numberOfItems); });
                    fetchThreads[i].Start();
                }
            }

            else if (fetchRatio != 0)
            {
                threadsForFetches = numberOfThreads;
                Thread thread;

                for (int i = 0; i < threadsForFetches; i++)
                {
                    thread = new Thread(() => { Get(numberOfItems); });
                }
            }

            else if (updateRatio != 0)
            {
                threadsForUpdates = numberOfThreads;
                Thread thread;

                for (int i = 0; i < threadsForUpdates; i++)
                {
                    thread = new Thread(() => { Update(numberOfItems, payLoad); });
                }
            }
        }

        private void InitializeCache(string cacheName)
        {
            // Initialize the cache ...
            _cache.Init(cacheName);
        }
    }
}