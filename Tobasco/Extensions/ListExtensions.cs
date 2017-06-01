using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Extensions
{
    public static class ListExtensions
    {
        public static void Add<T>(this List<T> list, T value, Func<T, bool> condition) where T : class
        {
            if (condition(value))
            {
                list.Add(value);
            }
        }
    }
}
