﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class TypeStatement : Statement
    {
        public string Name { get; private set; }
        public BlockStatement Body { get; private set; }
        public Keyword TypeKind { get; private set; }
        public AccessType AccessType { get; set; }
        /// <summary>
        /// Indicates optional keywords encountered while parsing.
        /// These are blindly added during parsing and checked for
        /// consistency and applicability at a later step.
        /// </summary>
        public List<Keyword> Options { get; private set; }
        public TypeStatement(int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
            this.Body = Body;
            this.TypeKind = TypeKind;
            this.Options = new List<Keyword>();
        }
        public void AddBody(BlockStatement Stmt)
        {
            if (Stmt == null) throw new ArgumentNullException(nameof(Stmt));

            Body = Stmt;
            Body.Parent = this;
        }

        public void AddOption(Keyword Option)
        {
            Options.Add(Option);
        }

        public override string ToString()
        {
            return $"{Name} {TypeKind.ToString().ToLower()}";
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"{Prepad(nesting)}Type: {Name}");

            builder.Append(Body.Dump(nesting));

            return builder.ToString();
        }
    }
}
