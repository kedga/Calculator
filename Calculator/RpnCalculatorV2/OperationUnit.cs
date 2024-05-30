using Calculator.UI;

namespace Calculator.RpnCalculatorV2;

public class OperationUnit(List<Operand> operands, Operator @operator)
{
	public List<Operand> Operands { get; set; } = operands;
	public Operator Operator { get; set; } = @operator;
	public static (OperationUnit? maybeOperationUnit, string errorMessage) TryCreate(List<CalculatorItem> items)
	{
		if (items.Count < 1)
		{
			return (null, "The items list must contain at least one item.");
		}
		if (items.Last() is not Operator @operator)
		{
			return (null, "The last item in the list must be an operator.");
		}

		var operands = items.OfType<Operand>().ToList();
		if (operands.Count < items.Count - 1)
		{
			return (null, "There are not enough operands for the operation.");
		}

		return (new OperationUnit(operands, @operator), "Success");
	}
	public double GetResult()
	{
		var operation = GetOperation();
		return operation();
	}

	private Func<double> GetOperation() => Operator switch
	{
		var o when o == Operator.Add => PerformAddition,
		var o when o == Operator.Subtract => PerformSubtraction,
		var o when o == Operator.Divide => PerformDivision,
		var o when o == Operator.Multiply => PerformMultiplication,
		_ => throw new NotImplementedException("Operator not implemented in OperationUnit.GetOperation"),
	};

	private double PerformAddition()
	{
		return Operands.Select(o => o.Value).Sum();
	}

	public double PerformDivision()
	{
		var result = Operands.Select(o => o.Value).Aggregate((acc, number) => acc / number);
		if (double.IsInfinity(result)) throw new DivideByZeroException("Cannot divide by zero");
		return result;
	}

	public double PerformMultiplication()
	{
		return Operands.Select(o => o.Value).Aggregate((acc, number) => acc * number);
	}

	public double PerformSubtraction()
	{
		return Operands.Select(o => o.Value).Aggregate((acc, number) => acc - number);
	}
}