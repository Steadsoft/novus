using System.Linq;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    public class DclLocalStatement : DclStatement
    {
        public string TypeName { get; private set; }

        public override string DecoratedName => DeclaredName;

        public override string Qualifier => null;

        public override string LiteralDecoratedName => throw new System.NotImplementedException();

        public DclLocalStatement(int Line, int Col, string Name, string TypeName) : base(Line, Col, Name, "local")
        {
            this.TypeName = TypeName;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Local: [{DeclaredName}] {TypeName} {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            return builder.ToString();
        }

    }
}