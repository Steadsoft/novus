using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser.Statements
{
    public class TypeName
    {
        public string Name { get; init; }
        public List<TypeName> GenericArgNames { get; set; }
        public TypeName(string Name)
        {
            this.Name = Name;
            this.GenericArgNames = new List<TypeName>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Name);

            if (GenericArgNames.Count == 0)
                return builder.ToString();

            builder.Append("<");

            foreach (TypeName type in GenericArgNames)
            {
                builder.Append(type.ToString());
                builder.Append(",");
            }

            var result = builder.ToString().TrimEnd(',') + ">";

            return result;
        }
    }
}
