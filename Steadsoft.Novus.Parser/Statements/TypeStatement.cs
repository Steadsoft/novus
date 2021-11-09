using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser
{
    public class TypeStatement : Statement
    {
        public NovusKeywords DeclaredKind { get; internal set; }
        public string Name { get; private set; }
        public BlockStatement Body { get; private set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public List<NovusKeywords> Options { get; private set; }
        public TypeStatement(int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
            this.Body = null;
            this.Options = new List<NovusKeywords>();
        }
        public void AddBody(BlockStatement Stmt)
        {
            Body = Stmt ?? throw new ArgumentNullException(nameof(Stmt));
            Body.Parent = this;
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

            builder.Append(Body.Dump(nesting));

            return builder.ToString();
        }
    }
}
