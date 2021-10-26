﻿using System;
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

        public Action (Step Step, States State)
        {
            this.Step = Step;
            this.State = State;
        }
    }
}