using System;

namespace Steadsoft.Novus.Parser.Statements
{
    public class ComputeStatement : Statement
    {
        /// <summary>
        /// Represents statements that are not definitions or declarations.
        /// </summary>
        /// <param name="Line"></param>
        /// <param name="Col"></param>
        public ComputeStatement(int Line, int Col) : base(Line, Col)
        {

        }

        public override string Dump(int nesting)
        {
            throw new NotImplementedException();
        }
    }
}
