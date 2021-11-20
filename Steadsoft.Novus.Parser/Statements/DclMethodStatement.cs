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
        public GenericName GenericName { get; set; }
        public DclTypeStatement Parent { get; private set; }
        public BlockStatement Block { get; private set; }
        public override string Qualifier => throw new NotImplementedException();
        public override string DecoratedName 
        {
            get
            {
                StringBuilder namebuilder = new StringBuilder();

                namebuilder.Append(base.DeclaredName).Append(GenericName.GenericArgList.DecoratedName);

                foreach (var t in Parameters)
                {
                    namebuilder.Append($".{t.TypeName}({t.PassBy})");
                }

                return namebuilder.ToString();
            }
        }
        public override string LiteralDecoratedName
        {
            get
            {
                StringBuilder namebuilder = new StringBuilder();

                namebuilder.Append(base.DeclaredName).Append(GenericName.GenericArgList.LiteralDecoratedName);

                foreach (var t in Parameters)
                {
                    namebuilder.Append($".{t.TypeName}({t.PassBy})");
                }

                return namebuilder.ToString();
            }
        }
        public List<Parameter> Parameters { get; private set; }
        public string Returns { get; internal set; }
        public bool HasBody { get; internal set; }
        public DclMethodStatement(int Line, int Col, GenericName Name, DclTypeStatement Parent) : base(Line, Col, Name.Name, "method")
        {
            this.Parameters = new List<Parameter>();
            this.Returns = null;
            this.Parent = Parent;
            this.GenericName = Name; ;
        }
        /// <summary>
        /// The Name in the case of a method, has two forms, one is the friendly name devoid of any embellishments arising from signatures
        /// and the other is that expanded name with signature embellishments. Because name is a contrived string used only within the
        /// compiler it is unhelpful to expose to users.
        /// </summary>
        public void AddParameter(Parameter Parameter)
        {
            Parameters.Add(Parameter);
        }
        public void AddBody(BlockStatement Stmt)
        {
            Block = Stmt ?? throw new ArgumentNullException(nameof(Stmt));
            Block.Parent = this;
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
        private string KindName
        {
            get { return Returns == null ? "Method:  " : "Function:"; }
        }
        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}{KindName} [{LiteralDecoratedName}] {ReturnsText} {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            if (HasBody)
                builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}