using System.Text;

namespace Steadsoft.Novus.Parser
{
    public class DefMethodStatement : DefStatement
    {
        public BlockStatement Body { get; private set; }

        public List<Parameter> Parameters { get; private set; }
        public DefMethodStatement(DefStatement Stmt) : base(Stmt.Line, Stmt.Col, Stmt.Name)
        {
            this.Parameters = new List<Parameter>();
        }

        public void AddParameter(Parameter Parameter)
        {
            Parameters.Add(Parameter);
        }

        public void AddBody(BlockStatement Stmt)
        {
            if (Stmt == null) throw new ArgumentNullException(nameof(Stmt));

            Body = Stmt;
            Body.Parent = this;
        }


        public override string Dump(int nesting)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"{Prepad(nesting)}Method: [{Name}] {ParametersText} {String.Join<NovusKeywords>(", ", Options.OrderBy(op => op.ToString()))}");

            builder.Append(Body.Dump(nesting));

            return builder.ToString();
        }

        private string ParametersText
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                builder.Append('(');

                foreach (var p in Parameters)
                {
                    builder.Append($"{p.Name} {p.TypeName}, ");
                }

                return builder.ToString().Trim(' ').Trim(',') + ")";
            }
        }
    }
}
