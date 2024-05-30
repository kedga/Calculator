using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Utilities;

public class ListUtilities
{
	public static (List<T> leftItems, List<T> middleItems, List<T> rightItems) SplitList<T>(List<T> items, int index, int leftOffset)
	{
		if (index < leftOffset)
		{
			throw new Exception("leftOffset larger than index");
		}
		if (leftOffset < 0)
		{
			throw new Exception("leftOffset cannot be negative");
		}

		var leftItems = items[..(index - leftOffset)];
		var middleItems = items[(index - leftOffset)..(index + 1)];
		var rightItems = items[(index + 1)..];

		return (leftItems, middleItems, rightItems);
	}
}
