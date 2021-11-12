namespace Steadsoft.Novus.Scanner.Classes
{
    /// <summary>
    /// Represents a 2D "array" of objects where space is only used as items are added.
    /// </summary>
    /// <typeparam name="T">Type of the 1st 'subscript'.</typeparam>
    /// <typeparam name="U">Type of the 2nd 'subscript'.</typeparam>
    /// <typeparam name="F">Type of objects stored in the table.</typeparam>
    public class SparseTable<T, U, F>
    {
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
    }
}