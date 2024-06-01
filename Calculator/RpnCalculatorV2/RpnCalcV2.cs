using Calculator.UI;
using Calculator.Utilities;

namespace Calculator.RpnCalculatorV2;

public class RpnCalcV2(IBasicIO? io = null) : IRpnCalculator
{
	protected readonly IBasicIO? _io = io;
	private readonly List<CalculatorItem> _items = [];
	public int ItemCount => _items.Count;

	public static class Message
	{
		public static string AddedOperand(double value) => $"Added number: {value}";
		public static string AddedOperator(Operator @operator) => $"Added operator: {@operator}";
		public static readonly string RemoveItemFail = $"Cannot remove item, sequence is empty";
		public static string RemovedItem(CalculatorItem item) => $"Removed: {item}";
		public static readonly string Cleared = $"Cleared all items";
		public static string PrintItems(List<CalculatorItem> items) =>
			$"[ {items.PrintCollection(" ")} ]";

		public static class TryPerformOperation
		{
			public static readonly string StartMessage = "Performing calculation!";
			public static string InitialItems(List<CalculatorItem> items) =>
				$"Items:  {PrintItems(items)}";
			public static string Error(string message) => $"Error: {message}";
			public static string NotEnoughOperands => Error("Not enough operands");
			public static string NotEnoughOperators => Error("Not enough operators");
			public static string Step(List<CalculatorItem> items, int stepCount) =>
				$"Step {stepCount}: {PrintItems(items)}";
			public static string FinalStep(List<CalculatorItem> items, int stepCount) =>
				$"Step {stepCount}: {PrintItems(items)} (final result)";
		}
	}

	public void AddOperand(double value)
	{
		AddOperand(new Operand(value));
	}

	public void AddOperand(Operand operand)
	{
		_items.Add(operand);
		_io?.PushOutput(Message.AddedOperand(operand.Value));
	}

	public void AddOperator(Operator @operator)
	{
		_items.Add(@operator);
		_io?.PushOutput(Message.AddedOperator(@operator));
	}

	public void RemoveLastItem()
	{
		if (_items.Count < 1)
		{
			_io?.PushOutput(Message.RemoveItemFail);
			return;
		}

		var lastItem = _items[^1];
		_items.RemoveAt(_items.Count - 1);
		_io?.PushOutput(Message.RemovedItem(lastItem));
	}

	public void Clear()
	{
		_items.Clear();
		_io?.PushOutput(Message.Cleared);
	}

	public double? TryPerformOperation()
	{
		_io?.PushOutput(Message.TryPerformOperation.StartMessage);
		_io?.PushOutput(Message.TryPerformOperation.InitialItems(_items));

		var workItems = _items.ToList();
		var stepCount = 1;

		while (workItems.Any(i => i is Operator))
		{
			var @operator = GetFirstOperator(workItems);

			var operatorIndex = workItems.IndexOf(@operator);

			if (operatorIndex < @operator.RequiredOperands)
			{
				_io?.PushOutput(Message.TryPerformOperation.NotEnoughOperands);
				return null;
			}

			var (leftItems, operationItems, rightItems) = SplitList(workItems, operatorIndex, @operator.RequiredOperands);

			var	operationResult = OperationUnit.CreateAndGetResultingOperand(operationItems);

			workItems = [.. leftItems, operationResult, .. rightItems];

			if (workItems.Count > 1)
			{
				_io?.PushOutput(Message.TryPerformOperation.Step(workItems, stepCount));
			}
			else if (workItems.First() is Operand operand)
			{
				_io?.PushOutput(Message.TryPerformOperation.FinalStep(workItems, stepCount));
				return operand.Value;
			}

			stepCount++;
		}

		_io?.PushOutput(Message.TryPerformOperation.NotEnoughOperators);
		return null;
	}

	public string PrintStackContents() => ToString();

	public override string ToString()
	{
		return Message.PrintItems(_items);
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
