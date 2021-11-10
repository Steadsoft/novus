using System.Text;

namespace Steadsoft.Novus.Parser
{
    /// <summary>
    /// Represents the declaration of a member field within a type.
    /// </summary>
    public class DclFieldStatement : DclStatement
    {
        public string TypeName { get; private set; }
        public DclFieldStatement(int Line, int Col, string Name, string TypeName) : base(Line, Col, Name, "field")
        {
            this.TypeName = TypeName;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Field: [{Name}] {TypeName} {String.Join<NovusKeywords>(", ", Options.OrderBy(op => op.ToString()))}");

            return builder.ToString();
        }

    }
}