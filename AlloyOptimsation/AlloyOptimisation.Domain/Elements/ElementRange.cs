namespace AlloyOptimisation.Domain.Elements
{
    public class ElementRange
    {
        public ElementDefinition Element { get; }
        public double Min { get; }
        public double Max { get; }
        public double Step { get; }

        public ElementRange(
            ElementDefinition element, 
            double min, 
            double max, 
            double step)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
            Min = min;
            Max = max;
            Step = step;
        }
    }
}
