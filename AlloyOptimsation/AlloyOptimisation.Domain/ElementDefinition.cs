namespace AlloyOptimisation.Domain
{
    public class ElementDefinition
    {
        public string Symbol { get; }
        public double Alpha { get; }
        public double CostPerKg { get; }

        public ElementDefinition(string symbol, double alpha, double costPerKg)
        {
            Symbol = symbol;
            Alpha = alpha;
            CostPerKg = costPerKg;
        }
    }
}
