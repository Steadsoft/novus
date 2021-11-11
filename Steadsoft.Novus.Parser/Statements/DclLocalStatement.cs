using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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