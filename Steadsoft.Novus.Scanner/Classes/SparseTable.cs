namespace Steadsoft.Novus.Scanner.Classes
{
    /// <summary>
    /// Represents a 2D "array" of objects where space is only used as items are added.
    /// </summary>
    /// <typeparam name="T">Type of the 1st 'subscript'.</typeparam>
    /// <typeparam name="U">Type of the 2nd 'subscript'.</typeparam>
    /// <typeparam name="F">Type of objects stored in the table.</typeparam>
    internal class SparseTable<T, U, F> where F : Entry
    {
        // TODO This class has degenarted a bit and needs retidying...its a bit hacky...

        private readonly Dictionary<T, Dictionary<U, F>> table = new();
        public void Add(T t, U u, F f)
        {
            if (table.ContainsKey(t) == false)
                table.Add(t, new Dictionary<U, F>());

            if (table[t].ContainsKey(u) == false)
                table[t].Add(u, f);
        }
        public void Add<V>(T t, V u, F f) where V : Enum
        {
            Add(t, (U)Convert.ChangeType(u, typeof(U)), f);
        }
        public bool TryGet(T t, U u, out F f)
        {
            f = default;

            if (table.ContainsKey(t) && table[t].ContainsKey(u))
            {
                f = table[t][u];
                return true;
            }

            return false;
        }
        public bool TryGet<V>(T t, V u, out F f) where V : Enum
        {
            return TryGet(t, (U)Convert.ChangeType(u, typeof(U)), out f);
        }
        public void RemoveAllEntriesFor(T initial, params T[] matches) //where T : Enum
        {
            foreach (var t in table[initial])
            {
                if (t.Value.State.Equals(Enums.State.DELIMITER0))
                {
                    table[initial].Remove(t.Key);
                }
            }

            foreach (var key in table.Keys)
            {
                foreach (var match in matches)
                {
                    if (key.Equals(match))
                    {
                        table.Remove(key);
                    }
                }
            }
        }
    }
}