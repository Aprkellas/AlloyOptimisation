namespace AlloyOptimisation.Domain.Alloy
{
    public interface IAlloyEnumerator
    {
        IEnumerable<AlloyComposition> Enumerate(AlloySystem system);
        public void AddRange(ElementRange range)
    }
}
