using Steadsoft.Novus.Parser.Enums;
using System.Text;

namespace Steadsoft.Novus.Parser.Statements
{
    public class AccessibilityBlock : BlockStatement
    {
        public BlockStatement Block { get; private set; }
        public Accessibility Accessibility { get; private set; }
        public AccessibilityBlock(int Line, int Col, Accessibility Accessibility) : base(Line, Col)
        {
            this.Block = new BlockStatement(Line, Col);
            this.Accessibility = Accessibility;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Access: [{Accessibility}]");

            builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}
