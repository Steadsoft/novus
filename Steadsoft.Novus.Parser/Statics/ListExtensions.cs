using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Statics
{
    public static class ListExtensions
    {
        public static bool DoesntContain<T>(this List<T> List, T Item)
        {
            return ! List.Contains(Item);
        }
        /// <summary>
        /// Returns true if any of the values supplied in 'Set' appear more than once in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <param name="Set"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true (and sets the out Item) if only one of the items appears and it appears only once in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <param name="Item"></param>
        /// <param name="Set"></param>
        /// <returns></returns>
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