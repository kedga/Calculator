using Calculator.Calculator;
using Calculator.UI;
using Calculator.Utilities;
using System;
using System.Collections.Generic;

namespace Calculator.RpnCalculatorV2;

public class RpnCalcV2 : IRpnCalculator
{
	private static readonly ColumnsFormatterIO _columns = new()
	{
		StringFormat = ["{0, 20}", "{1, -20}"],
		Separator = " : "
	};

	private static readonly CenterFormatterIO _center = new()
	{
		CenterPosition = 22
	};

    private readonly FormattedStringOutput? _output = null;
	public RpnCalcV2(IBasicIO? io = null)
	{
		if (io is not null)
		{
			_output = new FormattedStringOutput(io);
		}
	}

	private readonly List<CalculatorItem> _items = [];

	public int ItemCount => _items.Count;

	public void AddOperand(double value)
	{
		AddOperand(new Operand(value));
	}

	public void AddOperand(Operand operand)
	{
		_items.Add(operand);
		_output?.PushOutput(DisplayWithItems(Message.AddedOperand(operand.Value)));
	}

	public void AddOperator(Operator @operator)
	{
		_items.Add(@operator);
		_output?.PushOutput(DisplayWithItems(Message.AddedOperator(@operator)));
	}

	public void RemoveLastItem()
	{
		if (_items.Count < 1)
		{
			_output?.PushOutput(_center.GetFormattedString(Message.RemoveItemFail));
			return;
		}

		var lastItem = _items[^1];
		_items.RemoveAt(_items.Count - 1);
		_output?.PushOutput(DisplayWithItems(Message.RemovedItem(lastItem)));
	}

	public void Clear()
	{
		_items.Clear();
		_output?.PushOutput(_center.GetFormattedString(Message.Cleared));
	}

	public double? TryPerformOperation()
	{
		_output?.PushOutput(_center.GetFormattedString(Message.TryPerformOperation.StartMessage));
		_output?.PushOutput(FormattedString.Empty);
		_output?.PushOutput(DisplayWithItems(Message.TryPerformOperation.InitialItemsHeader));

		var workItems = _items.ToList();

		while (workItems.Any(i => i is Operator))
		{
			var firstOperator = workItems.OfType<Operator>().First();
			var firstOperatorIndex = workItems.IndexOf(firstOperator);
			var firstOperandIndex = firstOperatorIndex - firstOperator.RequiredOperands;

			if (firstOperandIndex < 0)
			{
				_output?.PushOutput(_center.GetFormattedString(Message.TryPerformOperation.NotEnoughOperands));
				return null;
			}

			var leftItems = workItems[..firstOperandIndex];
			var operationItems = workItems[firstOperandIndex..(firstOperatorIndex + 1)];
			var rightItems = workItems[(firstOperatorIndex + 1)..];

			var operationUnit = OperationUnit.Create(operationItems);

			workItems = [.. leftItems, operationUnit.GetResultAsOperand(), .. rightItems];

			_output?.PushOutput(DisplayWithItems($"({operationUnit.GetOperationAsString()})", workItems));

			if (workItems.Count == 1 && workItems.First() is Operand operand)
			{
				_items.Clear();
				_items.Add(operand);
				return operand.Value;
			}
		}

		_output?.PushOutput(_center.GetFormattedString(Message.TryPerformOperation.NotEnoughOperators));
		return null;
	}

	private FormattedString DisplayWithItems(string message, List<CalculatorItem>? items = null)
	{
		var displayItems = items is null ? _items : items;
		return _columns.GetFormattedString(message, Message.PrintItems(displayItems));
	}

	public void PrintStackContents()
	{
		_output?.PushOutput(_columns.GetFormattedString(string.Empty, Message.PrintItems(_items)));
	}
	public string GetStackContentsAsString()
	{
		return Message.PrintItems(_items);
	}
	public void PrintErrorMessage(string message)
	{
		_output?.PushOutput(_center.GetFormattedString($"{Message.Error} {message}"));
	}

	private record OperationIterationInformation(List<CalculatorItem> WorkItems, int StepCount, string OperationAsString);

	public static class Message
	{
		public static string AddedOperand(double value) => $"Added number: {value}";
		public static string AddedOperator(Operator @operator) => $"Added operator: {@operator}";
		public const string RemoveItemFail = $"Cannot remove item, sequence is empty";
		public static string RemovedItem(CalculatorItem item) => $"Removed: {item}";
		public const string Cleared = $"Cleared all items";
		public static string PrintItems(List<CalculatorItem> items) =>
			$"[ {items.PrintCollection(" ")} ]";
		public const string Error = $"Error: ";

		public static class TryPerformOperation
		{
			public const string StartMessage = "Performing calculation!";
			public const string InitialItemsHeader = $"Initial";
			public const string NotEnoughOperands = "Not enough operands";
			public const string NotEnoughOperators = "Not enough operators";
		}
	}
}