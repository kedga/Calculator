using Calculator.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.RpnCalculatorV2;

public class StringRpnCalcV2(IBasicIO io) : RpnCalcV2(io)
{
	public bool TryAddItem(string input)
	{
		if (double.TryParse(input, out var doubleValue))
		{
			AddOperand(doubleValue);
			return true;
		}
		else if (Operator.TryGetOperator(input) is Operator @operator)
		{
			AddOperator(@operator);
			return true;
		}
		_io.PushOutput("Unknown input: " + input);
		return false;
	}
}
