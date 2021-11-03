using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class TypeStatement : Statement
    {
        public string Name { get; private set; }
        public TypeBody Body { get; private set; }
        public Keyword TypeKind { get; private set; }
        public bool IsRecordClass { get; set; }
        public bool IsRecordStruct { get; set; }
        public TypeStatement(TypeBody Body, Keyword TypeKind, int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
            this.Body = Body;
            this.TypeKind = TypeKind;
        }

        public override string ToString()
        {
            string other = "";

            if (IsRecordClass)
                other = "(class)";

            if (IsRecordStruct)
                other = "(struct)";

            return $"{Name} {TypeKind.ToString().ToLower()} {other}";
        }

    }
}
