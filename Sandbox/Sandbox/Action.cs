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
        public int State { get; private set; }

        public Action (Step Step, int State)
        {
            this.Step = Step;
            this.State = State;
        }
    }
}