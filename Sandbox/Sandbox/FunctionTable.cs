using System;
using System.Collections.Generic;

namespace Sandbox
{
    public class FunctionTable<T, U, F>
    {
        private Dictionary<T, Dictionary<U, F>> table = new();

        public void Add(T t, U u, F f)
        {
            if (table.ContainsKey(t) == false)
                table.Add(t, new Dictionary<U, F>());

            if (table[t].ContainsKey(u) == false)
                table[t].Add(u, f);
        }

        public void Add<V>(T t, V u, F f) where V : System.Enum
        {
            Add(t, (U)Convert.ChangeType(u, typeof(U)), f);
        }


        public F GetHandler(T t, U u)
        {
            if (table.ContainsKey(t) && table[t].ContainsKey(u))
                return table[t][u];

            return default;
        }

        public F GetHandler<V>(T t, V u) where V : System.Enum
        {
            return GetHandler(t, (U)Convert.ChangeType(u, typeof(U)));
        }
    }
}