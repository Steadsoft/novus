using System.Text;

namespace Steadsoft.Novus.Parser
{
    public class DefFieldStatement : DefStatement
    {
        public string TypeName { get; private set; }
        public DefFieldStatement(int Line, int Col, string Name, string TypeName) : base(Line, Col, Name)
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