using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class NamespaceStatement : Statement
    {
        public string Name { get; private set; }
        public BlockStatement Block {get; private set;}

        public NamespaceStatement(int Line, int Col, string Name):this(null, Line, Col, Name)
        {
        }

        public NamespaceStatement(BlockStatement Block, int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
            this.Block = Block;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
