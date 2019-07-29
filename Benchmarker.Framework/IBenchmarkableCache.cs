using System;

namespace Benchmarker.Framework
{
    public interface IBenchmarkableCache : IDisposable
    {
        //
        //Summary:
        //      Intialize the cache instance and this instace will be used to perform operations on cache.
        //Parameters:
        //  cacheName:
        //      The name of Cache that needs to be initialized.
        void Init(string cacheName);

        //
        //Summary:
        //      Performs Add operations on cache.
        //Parameters:
        //  key:
        //      Key of Cache Item that wil be added in cache.
        //  value:
        //      Value of the Cache Item that will be added in cache.
        void Add(string key, object value);

        //
        //Summary:
        //      Performs Get operations on cache.
        //Parameters:
        //  key:
        //      Key of Cache Item that wil be retrieved from cache.
        void Get(string key);

        //
        //Summary:
        //      Performs Add operations on cache.
        //Parameters:
        //  key:
        //      Key of Cache Item whose value will be updated.
        //  value:
        //      Value of the Cache Item that will be updated.
        void Update(string key, object value);

        //
        //Summary:
        //      Performs Get operations on cache.
        //Parameters:
        //  key:
        //      Key of Cache Item that wil be removed from cache.
        void Remove(string key);
    }
}
