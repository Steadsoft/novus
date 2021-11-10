using System.Text;

namespace Steadsoft.Novus.Parser
{
    public class TypeStatement : Statement, IBlockContainer
    {
        public NovusKeywords DeclaredKind { get; internal set; }
        public string Name { get; private set; }
        public BlockStatement Block { get; private set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public List<NovusKeywords> Options { get; private set; }

        public string ShortTypeName => "type";

        public TypeStatement(int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
            this.Block = null;
            this.Options = new List<NovusKeywords>();
        }
        public void AddBody(BlockStatement Stmt)
        {
            Block = Stmt ?? throw new ArgumentNullException(nameof(Stmt));
            Block.Parent = this;
        }

        public void AddOption(NovusKeywords Option)
        {
            Options.Add(Option);
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Type: [{Name}] {String.Join<NovusKeywords>(", ",Options.OrderBy(op => op.ToString()))}");

            builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}
