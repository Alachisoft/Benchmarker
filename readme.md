# NCache Benchmarker
NCache Benchmarker application is developed to Benchmark distributed caching solutions. This application works on a provider model. Currently only provider is available to Benchmark NCache but it can be extended by writing providers for other caching solutions also.
## Configuration
In App.config file we have to provide following information:
* Provider FQN. Fully Qualified Name of class that contains the provider implementation for caching solution that needs to be benchmark.
* Cache Name (Provide Cache name in case of NCache).
* Number of entries to add in cache.
* Total Number of threads that will perform operations on cache.
* Get-Ratio (Percentage of Threads that will perform Get operations).
* Put-Ratio (Percentage of Threads that will perform Insert operations) .
