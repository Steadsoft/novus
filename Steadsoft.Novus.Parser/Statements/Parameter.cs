﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser
{
    public class Parameter
    {
        public PassBy PassBy { get; private set; }
        public string Name { get; private set; }
        /// <summary>
        /// This is a bit simple for time being, just the name of the param type...
        /// </summary>
        public string TypeName { get; private set; }
        public Parameter (string Name, string TypeName, PassBy PassBy)
        {
            this.Name = Name;
            this.TypeName = TypeName;
            this.PassBy = PassBy;
        }
    }
}