using System.Linq;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of a member field within a type.
    /// </summary>
    public class DclFieldStatement : DclStatement
    {
        public DclTypeStatement Parent { get; private set; }
        public string TypeName { get; private set; }
        public DclFieldStatement(int Line, int Col, string Name, string TypeName, DclTypeStatement Parent) : base(Line, Col, Name, "field")
        {
            this.TypeName = TypeName;
            this.Parent = Parent;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Field: [{Name}] {TypeName} {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            return builder.ToString();
        }

    }
}