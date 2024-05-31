using Calculator.UI;
using Calculator.Utilities;

namespace Calculator.RpnCalculatorV2;

public class RpnCalcV2(IBasicIO? io = null) : IRpnCalculator
{
	protected readonly IBasicIO? _io = io;
	private readonly List<CalculatorItem> _items = [];
	public int ItemCount => _items.Count;

	public void AddOperand(double value)
	{
		_items.Add(new Operand(value));
		_io?.PushOutput($"Added number: {value}");
	}

	public void AddOperator(Operator @operator)
	{
		_items.Add(@operator);
		_io?.PushOutput($"Added operator: {@operator}");
	}

	public void RemoveLastItem()
	{
		if (_items.Count > 0) // TODO: Make more performant
		{
			var lastItem = _items.Last();
			_items.RemoveAt(_items.Count - 1);
			_io?.PushOutput("Removed: " + lastItem);
		}
		else
		{
			_io?.PushOutput("Cannot remove item, sequence is already empty");
		}
	}

	public void Clear()
	{
		_items.Clear();
		_io?.PushOutput("Cleared all items");
	}

	public double? TryPerformOperation()
	{
		_io?.PushOutput("Performing calculation!");
		_io?.PushOutput($"Items:  [ {_items.PrintCollection(" ")} ]");

		var workItems = _items.ToList();
		var stepCount = 1;

		while (workItems.Any(i => i is Operator))
		{
			var @operator = GetFirstOperator(workItems);

			var operatorIndex = workItems.IndexOf(@operator);

			if (operatorIndex < @operator.RequiredOperands)
			{
				_io?.PushOutput("Error: Not enough operands");
				return null;
			}

			var (leftItems, operationItems, rightItems) = SplitList(workItems, operatorIndex, @operator.RequiredOperands);

			double operationResult;
			try
			{
				var (maybeOperationUnit, errorMessage) = OperationUnit.TryCreate(operationItems);

				if (maybeOperationUnit is not OperationUnit validOperationUnit)
				{
					_io?.PushOutput("Error: " + errorMessage);
					return null;
				}
				operationResult = validOperationUnit.GetResult();
			}
			catch (Exception ex)
			{
				_io?.PushOutput("Error: " + ex.Message);
				return null;
			}

			var newOperand = new Operand(operationResult);
			workItems = [.. leftItems, newOperand, .. rightItems];

			if (workItems.Count > 1)
			{
				_io?.PushOutput($"Step {stepCount}: [ {workItems.PrintCollection(" ")} ]");
			}
			else if (workItems.First() is Operand operand)
			{
				_io?.PushOutput($"Result: [ {operand.Value} ]");
				return operand.Value;
			}

			stepCount++;
		}

		_io?.PushOutput("Error: Not enough operators");
		return null;
	}

	public string PrintStackContents() => ToString();

	public override string ToString()
	{
		return $"[ {_items.PrintCollection(" ")} ]";
	}

	private static Operator GetFirstOperator(List<CalculatorItem> items)
	{
		return items.OfType<Operator>().First();
	}

	public static (List<T> leftItems, List<T> middleItems, List<T> rightItems) SplitList<T>(List<T> items, int index, int leftOffset)
	{
		if (index < leftOffset)
		{
			throw new ArgumentOutOfRangeException(nameof(leftOffset), "leftOffset larger than index");
		}
		if (leftOffset < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(leftOffset), "leftOffset cannot be negative");
		}

		var leftItems = items[..(index - leftOffset)];
		var middleItems = items[(index - leftOffset)..(index + 1)];
		var rightItems = items[(index + 1)..];

		return (leftItems, middleItems, rightItems);
	}
}
