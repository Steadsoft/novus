using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Statements
{
    public interface IContainer
    {
        IContainer Parent { get; }
        List<IContainer> Children { get; }
        void AddChild(IContainer child);
        string Dump(int nesting);
    }
}