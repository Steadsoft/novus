using Steadsoft.Novus.Scanner.Enums;
using System;
using System.Linq;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents the declaration of a type like a class, struct, record or singlet
    /// </summary>
    public class DclTypeStatement : DclStatement, IBlockContainer
    {
        public Keywords DeclaredKind { get; internal set; }
        public BlockStatement Block { get; private set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>

        public DclTypeStatement(int Line, int Col, string Name) : base(Line, Col, Name, "type")
        {
            Block = null;
        }
        public void AddBody(BlockStatement Stmt)
        {
            Block = Stmt ?? throw new ArgumentNullException(nameof(Stmt));
            Block.Parent = this;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Type: [{Name}] {string.Join(", ", Options.OrderBy(op => op.ToString()))}");

            builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}
