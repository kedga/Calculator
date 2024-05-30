using Calculator.UI;
using Calculator.Utilities;

namespace Calculator.RpnCalculatorV2;

public partial class RpnCalcV2(IBasicIO io) : IRpnCalculatorV2
{
	private readonly List<CalculatorItem> _items = [];
	public int ItemCount => _items.Count;
	
	public bool TryAddItem(string input)
	{
		if (double.TryParse(input, out var doubleValue))
		{
			_items.Add(new Operand(doubleValue));
			io.PushOutput("Added number: " + input);
			return true;
		}
		else if (Operator.TryGetOperator(input) is Operator @operator)
		{
			_items.Add(@operator);
			io.PushOutput("Added operator: " + input);
			return true;
		}
		io.PushOutput("Unknown input: " + input);
		return false;
	}

	public void Clear()
	{
		_items.Clear();
	}

	private static Operator GetFirstOperator(List<CalculatorItem> items)
	{
		return (Operator)items.First(i => i is Operator);
	}

	public void TryPerformOperation()
	{
		io.PushOutput("Performing calculation!");

		var workItems = _items;
		int i = 0;

		while (workItems.Any(i => i is Operator))
		{
			io.PushOutput($"step {i}: [ {workItems.PrintCollection(" ")} ]");

			var @operator = GetFirstOperator(workItems);

			var operatorIndex = workItems.IndexOf(@operator);

			if (operatorIndex < @operator.RequiredOperands)
			{
				io.PushOutput("Error: Not enough operands");
				return;
			}

			var (leftItems, operationItems, rightItems) = ListUtilities.SplitList(workItems, operatorIndex, @operator.RequiredOperands);

			OperationUnit operationUnit;
			double operationResult;
			try
			{
				var (maybeOperationUnit, errorMessage) = OperationUnit.TryCreate(operationItems);
				if (maybeOperationUnit is not OperationUnit validOperationUnit)
				{
					io.PushOutput(errorMessage);
					return;
				}

				operationUnit = validOperationUnit;
				operationResult = operationUnit.GetResult();
			}
			catch (Exception ex)
			{
				io.PushOutput("Error: " + ex.Message);
				return;
			}

			var newOperand = new Operand(operationResult);
			workItems = [.. leftItems, newOperand, .. rightItems];

			i++;
		}

		if (workItems.Count == 1 && workItems.First() is Operand operand)
		{
			io.PushOutput($"step {i}: [ {operand.Value} ]");
			io.PushOutput("Result: " + operand.Value);
		}
		else
		{
			io.PushOutput("Error: Not enough operators");
		}
	}

	public string PrintStackContents() => ToString();
	public override string ToString()
	{
		return $"[ {_items.PrintCollection(" ")} ]";
	}
}
