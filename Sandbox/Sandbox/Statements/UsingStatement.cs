using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public abstract class Statement
    {
        public int Line { get; private set; }
        public int Col { get; private set; }

        public Statement(int Line, int Col)
        {
            this.Line = Line;
            this.Col = Col;
        }
    }
    public class UsingStatement : Statement
    {
        public string Name { get; private set; }
        
        public UsingStatement(int Line, int Col, string Name):base(Line, Col)
        {
            this.Name = Name;
        }
    }
}
