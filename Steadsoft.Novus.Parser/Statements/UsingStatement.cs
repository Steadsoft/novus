using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Statements
{

    public class UsingStatement : Statement, IContainer
    {
        public string Name { get; private set; }

        public IContainer Parent { get; private set; }

        public List<IContainer> Children => throw new System.NotImplementedException();

        public UsingStatement(IContainer Parent, int Line, int Col, string Name) : base(Line, Col)
        {
            this.Parent = Parent;
            this.Name = Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override string Dump(int nesting)
        {
            return "Using " + Name;
        }

        public void AddChild(IContainer child)
        {
            throw new System.NotImplementedException();
        }
    }
}
