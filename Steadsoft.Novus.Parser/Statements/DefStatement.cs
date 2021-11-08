using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            builder.AppendLine($"{Prepad(nesting)}Method: [{Name}] {String.Join<NovusKeywords>(", ", Options.OrderBy(op => op.ToString()))}");

            builder.Append(Body.Dump(nesting));

            return builder.ToString();
        }
    }
}
