using System;
using System.Threading.Tasks;
using Orleans;

namespace SimpleGrainInterface
{
    public interface IHelloGrain : IGrainWithIntegerKey
    {
        Task<string> Hello(string who);
    }
}
