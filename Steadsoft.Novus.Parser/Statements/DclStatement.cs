using Steadsoft.Novus.Scanner.Enums;
using System;
using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Statements
{
    public abstract class DclStatement : Statement
    {
        /// <summary>
        /// This the identifer of a declared entity devoid of any generic and/or signature information.
        /// </summary>
        public string DeclaredName { get; }
        /// <summary>
        /// This is the name of a declared entity that includes generic argument and/or signature information.
        /// </summary>
        public abstract string DecoratedName { get; }
        /// <summary>
        /// This the fully qualified container name that includes namespaces, container types etc.
        /// </summary>
        public abstract string Qualifier { get; }
        public string ShortStatementTypeName { get; private set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public List<Keywords> Options { get; private set; }
        public DclStatement(int Line, int Col, string DeclaredName) : this(Line, Col, DeclaredName, "undefined")
        {

        }
        public DclStatement(int Line, int Col, string DeclaredName, string ShortStatementTypeName) : base(Line, Col)
        {
            this.DeclaredName = DeclaredName;
            this.ShortStatementTypeName = ShortStatementTypeName;
            Options = new List<Keywords>();
        }

        public void AddOption(Keywords Option)
        {
            Options.Add(Option);
        }
        public override string Dump(int nesting)
        {
            throw new NotImplementedException();
        }
    }
}
