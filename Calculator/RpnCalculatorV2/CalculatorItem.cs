using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.RpnCalculatorV2;

public abstract record CalculatorItem;

public record Operand(double Value) : CalculatorItem
{
	public override string ToString()
	{
		return $"{Value}";
	}
}

public record Operator(string Symbol, int RequiredOperands) : CalculatorItem
{
	public static Operator Add { get; } = new("+", 2);
	public static Operator Subtract { get; } = new("-", 2);
	public static Operator Divide { get; } = new("/", 2);
	public static Operator Multiply { get; } = new("*", 2);

	public static List<Operator> GetAllOperators() => [Add, Subtract, Divide, Multiply];
	public static Operator? TryGetOperator(string symbol) =>
		GetAllOperators().FirstOrDefault(s => s.Symbol == symbol);

	public override string ToString()
	{
		return Symbol;
	}

	public static Func<List<Operand>, double> GetOperation(Operator @operator) => @operator switch
	{
		var o when o == Add => PerformAddition,
		var o when o == Subtract => PerformSubtraction,
		var o when o == Divide => PerformDivision,
		var o when o == Multiply => PerformMultiplication,
		_ => throw new NotImplementedException("Operator not implemented in OperationUnit.GetOperation"),
	};

	private static double PerformAddition(List<Operand> operands)
	{
		return operands.Select(o => o.Value).Sum();
	}

	private static double PerformDivision(List<Operand> operands)
	{
		var result = operands.Select(o => o.Value).Aggregate((acc, number) => acc / number);
		if (double.IsInfinity(result)) throw new DivideByZeroException("Cannot divide by zero");
		return result;
	}

	private static double PerformMultiplication(List<Operand> operands)
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc * number);
	}

	private static double PerformSubtraction(List<Operand> operands)
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc - number);
	}
}