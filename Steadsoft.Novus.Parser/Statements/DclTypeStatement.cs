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
    public class DclTypeStatement : DclStatement, IBlockContainer
    {
        public GenericArgList GenericArgs { get; set; }
        public override string Qualifier { get => DeclaredName; }
        public override string DecoratedName 
        {
            get
            {
                return DeclaredName + GenericArgs.DecoratedName;
            }
        }
        public Keywords TypeKind { get; internal set; }
        public BlockStatement Block { get; private set; }

        public override string LiteralDecoratedName
        {
            get
            {
                return DeclaredName + GenericArgs.LiteralDecoratedName;
            }
        }

        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>

        public DclTypeStatement(int Line, int Col, string Name) : base(Line, Col, Name, "type")
        {
            GenericArgs = new GenericArgList();
            Block = new BlockStatement(Line, Col);
        }
        public void AddBody(BlockStatement Stmt)
        {
            Block = Stmt ?? throw new ArgumentNullException(nameof(Stmt));
            Block.Parent = this;
        }

        public override string ToString()
        {
            return $"{DeclaredName}";
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Type: [{DeclaredName}] {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}
