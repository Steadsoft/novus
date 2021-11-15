using System;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of a namespace.
    /// </summary>
    public class DclNamespaceStatement : DclStatement, IBlockContainer
    {
        public BlockStatement Block { get; private set; }

        public DclNamespaceStatement(int Line, int Col, string Name) : base(Line, Col, Name, "namespace")
        {
            Block = new BlockStatement(Line, Col);
        }

        public void AddBlock(BlockStatement Stmt)
        {
            if (Stmt == null) throw new ArgumentNullException(nameof(Stmt));

            Block = Stmt;
            Block.Parent = this;
        }

        public override string ToString()
        {
            return Name;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Namespace: [{Name}]");

            if (Block != null)
                builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
    public class DclDelimiterStatement : DclStatement
    {
        public string DelimitText { get; private set; }
        public DclDelimiterStatement(int Line, int Col, string DelimitText) : base(Line, Col, "none", "delimiter")
        {
            this.DelimitText = DelimitText;
        }
    }
}

