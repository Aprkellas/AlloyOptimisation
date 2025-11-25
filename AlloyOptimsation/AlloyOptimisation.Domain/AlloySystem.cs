namespace AlloyOptimisation.Domain
{
    public class AlloySystem
    {
        public ElementDefinition BaseElement { get; }
        public IReadOnlyList<ElementDefinition> VariableElements { get; }

        public AlloySystem(ElementDefinition baseElement,
                           IReadOnlyList<ElementDefinition> variableElements)
        {
            BaseElement = baseElement;
            VariableElements = variableElements;
        }
    }
}
