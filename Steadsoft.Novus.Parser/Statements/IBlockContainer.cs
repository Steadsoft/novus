namespace Steadsoft.Novus.Parser
{
    
    /// <summary>
    /// Defines the interface that must be implemented by statements that can contain subordinate statements blocks.
    /// </summary>
    public interface IBlockContainer
    {
        public string ShortTypeName { get; }
        BlockStatement Block { get; }
        string Name { get; }
        int Line { get; }
        int Col { get; }
    }
}
