using System;
using System.Collections.Generic;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of a namespace.
    /// </summary>
    public class DclNamespaceStatement : DclStatement, IBlockContainer, IContainer
    {
        //public DclNamespaceStatement Parent { get; set; }
        public BlockStatement Block { get; private set; }

        public override string DecoratedName => DeclaredName;

        public override string Qualifier => null;

        public override string LiteralDecoratedName => DecoratedName;

        public IContainer Parent { get; set; }

        public List<IContainer> Children { get; private set; }

        public DclNamespaceStatement(IContainer Parent, int Line, int Col, string Name) : base(Line, Col, Name, "namespace")
        {
            this.Parent = Parent;
            this.Block = new BlockStatement(Line, Col);
            this.Children = new List<IContainer>();
        }

        public void AddBlock(BlockStatement Stmt)
        {
            if (Stmt == null) throw new ArgumentNullException(nameof(Stmt));

            Block = Stmt;
            Block.Parent = this;
        }

        public override string ToString()
        {
            return DeclaredName;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Namespace: [{DeclaredName}]");

            if (Block != null)
                builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }

        public void AddChild(IContainer child)
        {
            Children.Add(child);
        }
    }
}