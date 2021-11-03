using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class TypeStatement : Statement
    {
        public string Name { get; private set; }

        public TypeStatement(int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
        }

    }
}
