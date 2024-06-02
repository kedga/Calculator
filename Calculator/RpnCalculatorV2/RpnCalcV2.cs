using Calculator.UI;
using Calculator.Utilities;
using System;
using System.Collections.Generic;

namespace Calculator.RpnCalculatorV2;

public class RpnCalcV2(IBasicIO? io = null) : IRpnCalculator
{
	protected readonly IBasicIO? _columnIO = new IOColumnsFormatter(io)
	{
		StringFormat = ["{0, 20}", "{1, -20}"],
		Separator = " : "
	};
	protected readonly IBasicIO? _centerIO = new IOCenterFormatter(io)
	{
		CenterPosition = 22
	};

	private readonly List<CalculatorItem> _items = [];

	public int ItemCount => _items.Count;

	public void AddOperand(double value)
	{
		AddOperand(new Operand(value));
	}

	public void AddOperand(Operand operand)
	{
		_items.Add(operand);
		_columnIO?.PushOutput(Message.AddedOperand(operand.Value), Message.PrintItems(_items));
	}

	public void AddOperator(Operator @operator)
	{
		_items.Add(@operator);
		_columnIO?.PushOutput(Message.AddedOperator(@operator), Message.PrintItems(_items));
	}

	public void RemoveLastItem()
	{
		if (_items.Count < 1)
		{
			_centerIO?.PushOutput(Message.RemoveItemFail);
			return;
		}

		var lastItem = _items[^1];
		_items.RemoveAt(_items.Count - 1);
		_columnIO?.PushOutput(Message.RemovedItem(lastItem), Message.PrintItems(_items));
	}

	public void Clear()
	{
		_items.Clear();
		_centerIO?.PushOutput(Message.Cleared);
	}

	public double? TryPerformOperation()
	{
		_centerIO?.PushOutput(Message.TryPerformOperation.StartMessage);
		_columnIO?.PushOutput(string.Empty);
		_columnIO?.PushOutput(Message.TryPerformOperation.InitialItems(_items));

		var workItems = _items.ToList();

		while (workItems.Any(i => i is Operator))
		{
			var firstOperator = workItems.OfType<Operator>().First();
			var firstOperatorIndex = workItems.IndexOf(firstOperator);
			var firstOperandIndex = firstOperatorIndex - firstOperator.RequiredOperands;

			if (firstOperandIndex < 0)
			{
				_centerIO?.PushOutput(Message.TryPerformOperation.NotEnoughOperands);
				return null;
			}

			var leftItems = workItems[..firstOperandIndex];
			var operationItems = workItems[firstOperandIndex..(firstOperatorIndex + 1)];
			var rightItems = workItems[(firstOperatorIndex + 1)..];

			var operationUnit = OperationUnit.Create(operationItems);

			workItems = [.. leftItems, operationUnit.GetResultAsOperand(), .. rightItems];

			_columnIO?.PushOutput($"({operationUnit.GetOperationAsString()})", Message.PrintItems(workItems));

			if (workItems.Count == 1 && workItems.First() is Operand operand)
			{
				_items.Clear();
				_items.Add(operand);
				return operand.Value;
			}
		}

		_centerIO?.PushOutput(Message.TryPerformOperation.NotEnoughOperators);
		return null;
	}

	public void PrintStackContents()
	{
		_columnIO?.PushOutput([null, Message.PrintItems(_items)]);
	}
	public string GetStackContentsAsString()
	{
		return Message.PrintItems(_items);
	}
	public void PrintErrorMessage(string message)
	{
		_centerIO?.PushOutput(Message.Error(message));
	}

	private record OperationIterationInformation(List<CalculatorItem> WorkItems, int StepCount, string OperationAsString);

	public static class Message
	{
		public static string AddedOperand(double value) => $"Added number: {value}";
		public static string AddedOperator(Operator @operator) => $"Added operator: {@operator}";
		public static readonly string RemoveItemFail = $"Cannot remove item, sequence is empty";
		public static string RemovedItem(CalculatorItem item) => $"Removed: {item}";
		public static readonly string Cleared = $"Cleared all items";
		public static string PrintItems(List<CalculatorItem> items) =>
			$"[ {items.PrintCollection(" ")} ]";
		public static string Error(string message) => $"Error: " + message;

		public static class TryPerformOperation
		{
			public static readonly string StartMessage = "Performing calculation!";
			public static string[] InitialItems(List<CalculatorItem> items) =>
				[$"Initial", PrintItems(items)];
			public static string[] Step(List<CalculatorItem> items, string operationString) =>
				[$"({operationString})", PrintItems(items)];
			public static string NotEnoughOperands => Error("Not enough operands");
			public static string NotEnoughOperators => Error("Not enough operators");
		}
	}
}