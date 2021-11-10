namespace Steadsoft.Novus.Parser
{
    public static class ListExtensions
    {
        public static bool ContainsMoreThanOneOf<T>(this List<T> List, params T[] Set) //where T : class
        {
            int count = 0;
            
            foreach (var member in Set)
            {
                if (List.Contains(member))
                {
                    count++;

                    if (count > 1)
                        return true;
                }
            }

            return false;
        }

        public static bool TryGetUnique<T>(this List<T> List, out T Item, params T[] Set) //where T : class
        {
            Item = default;

            if (List.ContainsMoreThanOneOf(Set))
                return false;

            foreach (var member in Set)
            {
                if (List.Contains(member))
                {
                    Item = member;
                    return true;
                }
            }

            return false;
        }
    }
}