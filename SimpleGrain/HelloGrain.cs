using System;
using System.Threading.Tasks;
using Orleans;
using SimpleGrainInterface;

namespace SimpleGrain
{
    public class HelloGrain : Grain, IHelloGrain
    {
        public Task<string> Hello(string who)
        {
            return Task.FromResult($"Hello, {who}");
        }
    }
}
