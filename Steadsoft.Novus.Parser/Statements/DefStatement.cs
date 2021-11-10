namespace Steadsoft.Novus.Parser
{
    public class DefStatement : Statement
    {
        public string Name { get; private set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public List<NovusKeywords> Options { get; private set; }
        public DefStatement(int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
            this.Options = new List<NovusKeywords>();
        }

        public void AddOption(NovusKeywords Option)
        {
            Options.Add(Option);
        }
        public override string Dump(int nesting)
        {
            throw new NotImplementedException();
        }
    }
}
