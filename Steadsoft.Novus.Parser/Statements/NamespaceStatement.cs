using System.Text;

namespace Steadsoft.Novus.Parser
{
    public class NamespaceStatement : Statement, IBlockContainer
    {
        public string Name { get; private set; }
        public BlockStatement Block {get; private set;}

        public string ShortTypeName => "namespace";

        public NamespaceStatement(int Line, int Col, string Name):base(Line, Col)
        {
            this.Name = Name;
            this.Block = new BlockStatement(Line, Col);
        }

        public void AddBlock (BlockStatement Stmt)
        {
            if (Stmt == null) throw new ArgumentNullException(nameof(Stmt));

            Block = Stmt;
            Block.Parent = this;
        }

        public override string ToString()
        {
            return Name;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Namespace: [{Name}]");
            
            if (Block != null)
               builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}
