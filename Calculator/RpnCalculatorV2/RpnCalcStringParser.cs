using Calculator.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.RpnCalculatorV2;

public class RpnCalcStringParser(IBasicIO? io = null)
{
	public bool TryAddItem(string input, IRpnCalculator calculator)
	{
		if (double.TryParse(input, out var doubleValue))
		{
			calculator.AddOperand(doubleValue);
			return true;
		}
		else if (Operator.TryGetOperator(input) is Operator @operator)
		{
			calculator.AddOperator(@operator);
			return true;
		}
		io?.PushOutput("Unknown input: " + input);
		return false;
	}
}