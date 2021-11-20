namespace Steadsoft.Novus.Parser.Statements
{
    /// <summary>
    /// Represents an identifier followed by a (possibly empty) set of generic args, the args could be nested too.
    /// </summary>
    /// <remarks>
    /// Certain kinds of names can contain a generic argument list and this class is the means by which this is represented.
    /// Some types be named this way as can methods and also parameter lists in method defintions.
    /// </remarks>
    public class GenericName
    {
        public string Name { get; set; }
        public GenericArgList GenericArgList { get; set; }

        public GenericName(string Name)
        {
            this.Name = Name;
            this.GenericArgList = new GenericArgList();
        }

        public override string ToString()
        {
            return Name + GenericArgList.LiteralDecoratedName;
        }
    }
}