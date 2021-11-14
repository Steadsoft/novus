using Steadsoft.Novus.Parser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of a method or function within a type.
    /// </summary>
    public class DclMethodStatement : DclStatement, IBlockContainer
    {
        public DclTypeStatement Parent { get; private set; }
        public BlockStatement Block { get; private set; }
        public List<Parameter> Parameters { get; private set; }
        public string Returns { get; internal set; }
        public bool HasBody { get; internal set; }
        public DclMethodStatement(int Line, int Col, string Name, DclTypeStatement Parent) : base(Line, Col, Name, "method")
        {
            this.Parameters = new List<Parameter>();
            this.Returns = null;
            this.Parent = Parent;
        }

        public void AddParameter(Parameter Parameter)
        {
            Parameters.Add(Parameter);
        }

        public void AddBody(BlockStatement Stmt)
        {
            Block = Stmt ?? throw new ArgumentNullException(nameof(Stmt));
            Block.Parent = this;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Method: [{Name}] {ParametersText} {ReturnsText} {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            if (HasBody)
                builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
        private string ReturnsText
        {
            get
            {
                return Returns == null ? "" : $"({Returns})";
            }
        }
        private string ParametersText
        {
            get
            {
                StringBuilder builder = new();

                builder.Append('(');

                foreach (var p in Parameters)
                {
                    if (p.PassBy == PassBy.Value)
                        builder.Append($"{p.Name} {p.TypeName}, ");
                    else
                        builder.Append($"{p.Name} {p.TypeName} {p.PassBy}, ");
                }

                return builder.ToString().Trim(' ').Trim(',') + ")";
            }
        }

    }
}
