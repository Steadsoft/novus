using Steadsoft.Novus.Parser.Enums;

namespace Steadsoft.Novus.Parser.Statements
{
    public class Parameter
    {
        public PassBy PassBy { get; private set; }
        public string Name { get; private set; }
        /// <summary>
        /// This is a bit simple for time being, just the name of the param type...
        /// </summary>
        public GenericName TypeName { get; private set; }
        public Parameter(string Name, GenericName TypeName, PassBy PassBy)
        {
            this.Name = Name;
            this.TypeName = TypeName;
            this.PassBy = PassBy;
        }
    }
}
