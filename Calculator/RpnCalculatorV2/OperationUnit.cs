using Calculator.UI;

namespace Calculator.RpnCalculatorV2;

public class OperationUnit
{
	public List<Operand> Operands { get; set; }
	public Operator Operator { get; set; }
    private OperationUnit(List<Operand> operands, Operator @operator)
    {
		Operands = operands;
		Operator = @operator;
	}
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
		//var operation = Operator.GetOperation(Operator);
		var operation = Operator.Operation;
		return operation(Operands);
	}
}