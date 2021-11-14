﻿using System.Linq;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of an enum member within an enum.
    /// </summary>

    public class DclEnumMemberStatement : DclStatement
    {
        public DclTypeStatement Parent { get; private set; }
        public DclEnumMemberStatement(int Line, int Col, string Name, DclTypeStatement Parent) : base(Line, Col, Name, "member")
        {
            this.Parent = Parent;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Member: [{Name}] {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            return builder.ToString();
        }

    }


}