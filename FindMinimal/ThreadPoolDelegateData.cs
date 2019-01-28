using System.Collections.Generic;
using System.Threading;

namespace FindMinimal
{
    internal class ThreadPoolDelegateData
    {
        public int[] SourceArray { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public List<int> ResultsStorage { get; set; }
        public CountdownEvent CountdownEvent { get; set; }
    }
}
