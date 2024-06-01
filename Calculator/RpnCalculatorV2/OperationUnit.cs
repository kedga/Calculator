﻿using Calculator.UI;

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
			return (null, ErrorMessage.TryCreate.NoItems);
		}
		if (items.OfType<Operator>().Count() > 1)
		{
			return (null, ErrorMessage.TryCreate.TooManyOperators);
		}
		if (items.Last() is not Operator @operator)
		{
			return (null, ErrorMessage.TryCreate.LastItemNotOperator);
		}

		var operands = items.OfType<Operand>().ToList();
		if (operands.Count != @operator.RequiredOperands)
		{
			return (null, ErrorMessage.TryCreate.WrongNumberOperands(@operator.RequiredOperands, operands.Count));
		}

		return (new OperationUnit(operands, @operator), ErrorMessage.TryCreate.Success);
	}

	public static Operand CreateAndGetResultingOperand(List<CalculatorItem> items)
	{
		if (items == null)
		{
			throw new ArgumentNullException(nameof(items), ErrorMessage.TryCreate.NoItems);
		}

		if (items.Count < 1)
		{
			throw new ArgumentException(ErrorMessage.TryCreate.NoItems, nameof(items));
		}

		if (items.OfType<Operator>().Count() > 1)
		{
			throw new ArgumentException(ErrorMessage.TryCreate.TooManyOperators, nameof(items));
		}

		if (items.Last() is not Operator @operator)
		{
			throw new ArgumentException(ErrorMessage.TryCreate.LastItemNotOperator, nameof(items));
		}

		var operands = items.OfType<Operand>().ToList();
		if (operands.Count != @operator.RequiredOperands)
		{
			throw new ArgumentException(ErrorMessage.TryCreate.WrongNumberOperands(@operator.RequiredOperands, operands.Count), nameof(items));
		}

		var operationUnit = new OperationUnit(operands, @operator);
		var result = operationUnit.GetResult();
		return new Operand(result);
	}

	public double GetResult()
	{
		var operation = Operator.Operation;
		return operation(Operands);
	}

	public static class ErrorMessage
	{
		public static class TryCreate
		{
			private static string TryCreateError(string message) => "Unable to create OperationUnit: " + message;

			public static readonly string NoItems = TryCreateError("The items list was empty.");

			public static readonly string TooManyOperators = TryCreateError("There must not be more than one operator.");

			public static readonly string LastItemNotOperator = TryCreateError("The last item in the list must be an operator.");

			public static string WrongNumberOperands(int required, int actual) =>
				TryCreateError($"There are not the right number of operands for the operation, required: {required}, actual: {actual}.");

			public static readonly string Success = "Success";
		}
	}
}