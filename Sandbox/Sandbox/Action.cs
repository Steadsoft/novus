using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class Action
    {
        public Step Step { get; private set; }
        public States State { get; private set; }
        public TokenType TokenType { get; private set; }
        public Action (Step Step, States State, TokenType TokenType = TokenType.Undecided)
        {
            this.Step = Step;
            this.State = State;
            this.TokenType = TokenType;
        }
    }
}