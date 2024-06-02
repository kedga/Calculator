using System.Reflection;

namespace Calculator.RpnCalculatorV2;

public record Operator(string Symbol, int RequiredOperands, Func<List<Operand>, double> Operation, Func<OperationUnit, string> GetOperationString) : CalculatorItem
{
	public static Operator Add { get; } = new("+", 2, (operands) =>
	{
		return operands.Select(o => o.Value).Sum();
	}, GetStandardOperationString);

	public static Operator Subtract { get; } = new("-", 2, (operands) =>
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc - number);
	}, GetStandardOperationString);

	public static Operator Divide { get; } = new("/", 2, (operands) =>
	{
		var result = operands.Select(o => o.Value).Aggregate((acc, number) => acc / number);
		if (double.IsInfinity(result)) throw new DivideByZeroException("Cannot divide by zero");
		return result;
	}, GetStandardOperationString);

	public static Operator Multiply { get; } = new("*", 2, (operands) =>
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc * number);
	}, GetStandardOperationString);

	public static Operator Exponential { get; } = new("exp", 2, (operands) =>
	{
		return operands.Select(selector: o => o.Value).Aggregate(Math.Pow);
	}, (operationUnit) => GetStandardOperationString(operationUnit, "^"));

	public static Operator Sine { get; } = new("sin", 1, (operands) =>
	{
		return Math.Sin(operands.First().Value);
	}, GetOperationAsFunctionString);

	public static List<Operator> GetAllOperators() =>
		typeof(Operator)
			.GetProperties(BindingFlags.Static | BindingFlags.Public)
			.Where(p => p.PropertyType == typeof(Operator))
			.Select(p => p.GetValue(null))
			.Cast<Operator>()
			.ToList();

	private static string GetStandardOperationString(OperationUnit operationUnit)
	{
		return string.Join($" {operationUnit.Operator.Symbol} ", operationUnit.Operands.Select(o => o.Value))
			+ " = " + $"{operationUnit.GetResult():G3}";
	}
	private static string GetStandardOperationString(OperationUnit operationUnit, string customOperationSymbol)
	{
		var newOperator = operationUnit.Operator with { Symbol = customOperationSymbol };
		var newOperationUnit = operationUnit with { Operator = newOperator };
		return GetStandardOperationString(newOperationUnit);
	}
	private static string GetOperationAsFunctionString(OperationUnit operationUnit)
	{
		return $"{operationUnit.Operator.Symbol}({operationUnit.Operands.FirstOrDefault()})"
			+ " = " + $"{operationUnit.GetResult():G3}";
	}
	public override string ToString()
	{
		return Symbol;
	}
}