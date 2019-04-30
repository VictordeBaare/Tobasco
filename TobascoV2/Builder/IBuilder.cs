using System.Collections.Generic;

namespace TobascoV2.Builder
{
    public interface IBuilder
    {
        void Build(IDictionary<string, string> keyValuePairs);
    }
}