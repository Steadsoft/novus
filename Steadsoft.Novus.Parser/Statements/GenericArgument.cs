using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser.Statements
{

    public class GenericArgList
    {
        private List<GenericArg> GenericArgs { get; set; }

        public GenericArgList()
        {
            this.GenericArgs = new List<GenericArg>();
        }

        public void Add(GenericArg Arg)
        {
            GenericArgs.Add(Arg);
        }

        public bool Any()
        {
            return GenericArgs.Any();
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            text.Append("<");

            for(int I=0; I < GenericArgs.Count; I++)
            {
                text.Append(I);
                text.Append(GenericArgs[I].ToString());
                text.Append(',');
            }

            return text.ToString().Trim(',') + ">";
        }
    }

    public class GenericArg
    {
        public string ArgName { get; set; }
        public GenericArgList GenericArgs { get; set; }

        public GenericArg(string Name)
        {
            this.ArgName = Name;
            this.GenericArgs = new GenericArgList();
        }

        public override string ToString()
        {
            if (GenericArgs.Any())
            {
                return GenericArgs.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
