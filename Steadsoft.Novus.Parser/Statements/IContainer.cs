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

    public interface INamedElement
    {
        /// <summary>
        /// This the identifer of a declared entity devoid of any generic and/or signature information.
        /// </summary>
        string DeclaredName { get; }
        /// <summary>
        /// This is the name of a declared entity that includes generic argument and/or signature information but in numeric form.
        /// </summary>
        string DecoratedName { get; }
        /// <summary>
        /// This is the decorated name but with the original identifers used in the source rather than numeric codes.
        /// </summary>
        string LiteralDecoratedName { get; }
    }
}