namespace FindMinimal
{
    internal class FindMinimalDelegateData
    {
        public int[] SourceArray { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int[] ResultsStorage { get; set; }
        public int ThreadIndex { get; set; }
    }
}
