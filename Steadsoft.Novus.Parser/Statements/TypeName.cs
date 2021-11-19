using System.Collections.Generic;
using System.Text;

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

            //builder.Append(Name);

            if (GenericArgNames.Count == 0)
                return builder.ToString();

            builder.Append("<");

            for  (int I=1; I <= GenericArgNames.Count; I++)
            {
                builder.Append(I + GenericArgNames[I-1].ToString());
                builder.Append(",");
            }

            var result = builder.ToString().TrimEnd(',') + ">";

            return result;
        }
    }
}
