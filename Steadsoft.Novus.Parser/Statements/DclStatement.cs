using Steadsoft.Novus.Scanner.Enums;
using System;
using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Statements
{
    public abstract class DclStatement : Statement
    {
        public virtual string DecalredName { get; private set; }
        public virtual string FullName { get; private set; }
        public virtual string QualifiedName { get; private set; }
        /// <summary>
        /// This is the name with any embellishments stripped away.
        /// </summary>
        /// <remarks>
        /// Some declarations are for things like methods where their unique name include signature detail.
        /// The FriendlyName is used to report diagnostic messages and so on to avoid confusing users.
        /// For the majority of items the Name and the FriendlyName are identical.
        /// </remarks>
        public virtual string FriendlyName 
        { 
            get { return DecalredName; }
        }
        public string ShortStatementTypeName { get; private set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public List<Keywords> Options { get; private set; }
        public DclStatement(int Line, int Col, string Name) : this(Line, Col, Name, "undefined")
        {

        }
        public DclStatement(int Line, int Col, string Name, string ShortStatementTypeName) : base(Line, Col)
        {
            this.DecalredName = Name;
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
