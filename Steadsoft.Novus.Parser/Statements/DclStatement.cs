using Steadsoft.Novus.Parser.Enums;
using System;
using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Statements
{
    public class DclStatement : Statement
    {
        public string Name { get; private set; }
        public string ShortTypeName { get; private set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public List<NovusKeywords> Options { get; private set; }
        public DclStatement(int Line, int Col, string Name) : this(Line, Col, Name, "undefined")
        {

        }
        public DclStatement(int Line, int Col, string Name, string ShortTypeName) : base(Line, Col)
        {
            this.Name = Name;
            this.ShortTypeName = ShortTypeName;
            Options = new List<NovusKeywords>();
        }

        public void AddOption(NovusKeywords Option)
        {
            Options.Add(Option);
        }
        public override string Dump(int nesting)
        {
            throw new NotImplementedException();
        }
    }
}
