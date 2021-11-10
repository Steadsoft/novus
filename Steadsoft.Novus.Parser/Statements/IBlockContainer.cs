namespace Steadsoft.Novus.Parser
{
    public interface IBlockContainer
    {
        public string ShortTypeName { get; }
        BlockStatement Block { get; }
        string Name { get; }
        int Line { get; }
        int Col { get; }
    }
}
