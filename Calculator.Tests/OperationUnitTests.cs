using Calculator.RpnCalculatorV2;

namespace Calculator.Tests;

public class OperationUnitTests
{
	[Fact]
	public void TryCreate_ShouldReturnError_WhenItemsListIsEmpty()
	{
		var items = new List<CalculatorItem>();
		var (operationUnit, errorMessage) = OperationUnit.TryCreate(items);

		Assert.Null(operationUnit);
		Assert.Equal(OperationUnit.ErrorMessage.TryCreate.NoItems, errorMessage);
	}

	[Fact]
	public void TryCreate_ShouldReturnError_WhenLastItemIsNotOperator()
	{
		var items = new List<CalculatorItem> { Operator.Add, new Operand(2) };
		var (operationUnit, errorMessage) = OperationUnit.TryCreate(items);

		Assert.Null(operationUnit);
		Assert.Equal(OperationUnit.ErrorMessage.TryCreate.LastItemNotOperator, errorMessage);
	}

	[Fact]
	public void TryCreate_ShouldReturnError_WhenOperandsCountDoesNotMatchRequiredOperands()
	{
		var items = new List<CalculatorItem> { new Operand(2), Operator.Add };
		var (operationUnit, errorMessage) = OperationUnit.TryCreate(items);

		Assert.Null(operationUnit);
		Assert.Equal(OperationUnit.ErrorMessage.TryCreate.WrongNumberOperands(2, 1), errorMessage);
	}

	[Fact]
	public void TryCreate_ShouldReturnError_WhenMoreThanOneOperatorExists()
	{
		var items = new List<CalculatorItem> { new Operand(2), Operator.Subtract, new Operand(7), Operator.Add };
		var (operationUnit, errorMessage) = OperationUnit.TryCreate(items);

		Assert.Null(operationUnit);
		Assert.Equal(OperationUnit.ErrorMessage.TryCreate.TooManyOperators, errorMessage);
	}

	[Fact]
	public void TryCreate_ShouldReturnSuccess_WhenItemsAreValid()
	{
		var items = new List<CalculatorItem> { new Operand(2), new Operand(3), Operator.Add };
		var (operationUnit, errorMessage) = OperationUnit.TryCreate(items);

		Assert.NotNull(operationUnit);
		Assert.Equal("Success", errorMessage);
		Assert.Equal(2, operationUnit.Operands[0].Value);
		Assert.Equal(3, operationUnit.Operands[1].Value);
		Assert.Equal(Operator.Add, operationUnit.Operator);
	}
}
