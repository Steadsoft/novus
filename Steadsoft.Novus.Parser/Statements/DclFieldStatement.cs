using System.Linq;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of a member field within a type.
    /// </summary>
    public class DclFieldStatement : DclStatement
    {
        public TypeNode Parent { get; private set; }
        public string TypeName { get; private set; }

        public override string DecoratedName => DeclaredName;

        public override string Qualifier => throw new System.NotImplementedException();

        public override string LiteralDecoratedName => throw new System.NotImplementedException();

        public DclFieldStatement(int Line, int Col, string Name, string TypeName, TypeNode Parent) : base(Line, Col, Name, "field")
        {
            this.TypeName = TypeName;
            this.Parent = Parent;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Field: [{DeclaredName}] {TypeName} {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            return builder.ToString();
        }

    }


}