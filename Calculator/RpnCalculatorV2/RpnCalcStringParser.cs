using Calculator.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.RpnCalculatorV2;

public class RpnCalcStringParser
{
	public bool TryAddItem(string input, IRpnCalculator calculator)
	{
		if (double.TryParse(input, out var doubleValue))
		{
			calculator.AddOperand(doubleValue);
			return true;
		}
		else if (TryGetOperator(input) is Operator @operator)
		{
			calculator.AddOperator(@operator);
			return true;
		}
		calculator.PrintErrorMessage($"Unknown input, \"{input}\"");
		return false;
	}

	private static Operator? TryGetOperator(string symbol) =>
		Operator.GetAllOperators().FirstOrDefault(s => s.Symbol == symbol);
}