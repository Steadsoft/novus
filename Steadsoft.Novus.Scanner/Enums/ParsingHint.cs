using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Scanner.Enums
{
    public enum ParsingHint
    {
        /// <summary>
        /// Instruct the scanner to return two > tokens whenever it sees a >> token.
        /// </summary>
        SplitRightShift
    }
}
