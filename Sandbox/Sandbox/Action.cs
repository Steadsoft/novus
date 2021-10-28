using System;

namespace Sandbox
{
    public class Action
    {
        public Step Step { get; private set; }
        public State State { get; private set; }
        public TokenType TokenType { get; private set; }
        public Action (Step Step, State State, TokenType TokenType = TokenType.Undecided)
        {
            this.Step = Step;
            this.State = State;
            this.TokenType = TokenType;

            if (Step == Step.AppendReturn || Step == Step.RestoreReturn)
                if (TokenType == TokenType.Undecided)
                    throw new ArgumentException("You must supply a token type when creating an action that halts tokenization.");
        }
    }
}