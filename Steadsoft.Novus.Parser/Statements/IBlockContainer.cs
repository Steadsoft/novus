using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Statements
{

    /// <summary>
    /// Defines the interface that must be implemented by statements that can contain subordinate statements blocks.
    /// </summary>
    public interface IBlockContainer
    {
        public string ShortStatementTypeName { get; }
        BlockStatement Block { get; }
        string DeclaredName { get; }
        int Line { get; }
        int Col { get; }
    }

    public interface IContainer
    {
        IContainer Parent { get; }
        List<IContainer> Children { get; }
        void AddChild(IContainer child);
    }
}