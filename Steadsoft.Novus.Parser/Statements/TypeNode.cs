using Steadsoft.Novus.Scanner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of a type like a class, struct, record or singlet
    /// </summary>
    public class TypeNode : DclStatement, IContainer
    {
        public GenericName GenericName { get; set; }
        //public BlockStatement Block { get; private set; }
        public override string Qualifier { get => DeclaredName; }
        public override string DecoratedName 
        {
            get
            {
                return DeclaredName + GenericName.GenericArgList.DecoratedName;
            }
        }
        public override string LiteralDecoratedName
        {
            get
            {
                return DeclaredName + GenericName.GenericArgList.LiteralDecoratedName;
            }
        }
        public Keywords TypeKind { get; internal set; }

        public IContainer Parent { get; private set; }

        public List<IContainer> Children { get; private set; }

        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public TypeNode(IContainer Parent, int Line, int Col, GenericName Name) : base(Line, Col, Name.Name, "type")
        {
            this.Parent = Parent;
            this.GenericName = Name;
            //this.Block = new BlockStatement(Line, Col);
            this.Children = new List<IContainer>();
        }
        //public void AddBody(BlockStatement Stmt)
        //{
        //    Block = Stmt ?? throw new ArgumentNullException(nameof(Stmt));
        //    Block.Parent = this;
        //}
        public override string ToString()
        {
            return $"{DeclaredName}";
        }
        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Type: [{LiteralDecoratedName}] {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            foreach (var child in Children)
            {
                builder.Append(child.Dump(nesting));
            }

            return builder.ToString();
        }

        public void AddChild(IContainer child)
        {
            throw new NotImplementedException();
        }
    }
}