using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    public class DclLocalStatement : DclStatement
    {
        public string TypeName { get; private set; }

        public DclLocalStatement(int Line, int Col, string Name, string TypeName) : base(Line, Col, Name, "local")
        {
            this.TypeName = TypeName;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Local: [{Name}] {TypeName} {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            return builder.ToString();
        }

    }
}