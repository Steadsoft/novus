using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser.Statements
{

    public class GenericArgList
    {
        public List<GenericArg> GenericArgs { get; set; }

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

        public string DecoratedName
        {
            get
            {
                return ToString();
            }
        }

        public string LiteralDecoratedName
        {
            get
            {
                if (GenericArgs.Count == 0)
                    return "";

                StringBuilder text = new StringBuilder();

                text.Append("<");

                for (int I = 0; I < GenericArgs.Count; I++)
                {
                    text.Append(GenericArgs[I].LiteralDecoratedName);
                    text.Append(',');
                }

                return text.ToString().Trim(',') + ">";
            }
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

        public string LiteralDecoratedName
        {
            get
            {
                if (GenericArgs.Any())
                {
                    return ArgName + GenericArgs.LiteralDecoratedName;
                }
                else
                {
                    return ArgName;
                }
            }
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
