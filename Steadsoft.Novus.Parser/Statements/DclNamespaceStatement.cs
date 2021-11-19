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
            return DecalredName;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Namespace: [{DecalredName}]");

            if (Block != null)
                builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}