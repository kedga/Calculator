using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculator.Program;

namespace Calculator.Utilities;

public static class EnumUtilities
{
	public static T[] GetEnumArray<T>() where T : Enum
	{
		return (T[])Enum.GetValues(typeof(T));
	}
}