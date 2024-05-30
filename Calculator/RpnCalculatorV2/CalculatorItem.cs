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

	//public override bool Equals(object? obj)
	//{
	//	return obj is Operator other && Symbol == other.Symbol;
	//}
}