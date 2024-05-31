using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Utilities;

public static class CollectionToString
{
    public static string PrintCollection<T>(this IEnumerable<T> collection, string separator = ", ")
    {
		return $"{string.Join(separator, collection)}";
	}
}
