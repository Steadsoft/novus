using Steadsoft.Novus.Parser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser.Statements
{
    public class AccessibilityBlock : Statement
    {
        public Accessibility Accessibility { get; private set; }
        public AccessibilityBlock(int Line, int Col, Accessibility Accessibility) : base(Line, Col)
        {
            this.Accessibility = Accessibility;
        }

        public override string Dump(int nesting)
        {
            throw new NotImplementedException();
        }
    }
}
