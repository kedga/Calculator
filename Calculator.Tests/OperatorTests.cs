using Calculator.RpnCalculatorV2;

namespace Calculator.Tests;

public class OperatorTests
{
	[Fact]
	public void AddOperator_ShouldAddOperands()
	{
		var operands = new List<Operand> { new (2), new (3) };
		double result = Operator.Add.Operation(operands);
		Assert.Equal(5, result);
	}

	[Fact]
	public void SubtractOperator_ShouldSubtractOperands()
	{
		var operands = new List<Operand> { new (5), new (3) };
		double result = Operator.Subtract.Operation(operands);
		Assert.Equal(2, result);
	}

	[Fact]
	public void MultiplyOperator_ShouldMultiplyOperands()
	{
		var operands = new List<Operand> { new (2), new (3) };
		double result = Operator.Multiply.Operation(operands);
		Assert.Equal(6, result);
	}

	[Fact]
	public void DivideOperator_ShouldDivideOperands()
	{
		var operands = new List<Operand> { new (6), new (2) };
		double result = Operator.Divide.Operation(operands);
		Assert.Equal(3, result);
	}

	[Fact]
	public void DivideOperator_ShouldThrowDivideByZeroException()
	{
		var operands = new List<Operand> { new (6), new (0) };
		Assert.Throws<DivideByZeroException>(() => Operator.Divide.Operation(operands));
	}

	[Fact]
	public void ExponentialOperator_ShouldCalculatePower()
	{
		var operands = new List<Operand> { new (2), new (3) };
		double result = Operator.Exponential.Operation(operands);
		Assert.Equal(8, result); // 2^3 = 8
	}

	[Fact]
	public void SineOperator_ShouldCalculateSine()
	{
		var operands = new List<Operand> { new (Math.PI / 2) };
		double result = Operator.Sine.Operation(operands);
		Assert.Equal(1, result, 5); // sin(π/2) = 1
	}

	[Fact]
	public void GetAllOperators_ShouldReturnAllOperators()
	{
		var operators = Operator.GetAllOperators();
		Assert.Contains(Operator.Add, operators);
		Assert.Contains(Operator.Subtract, operators);
		Assert.Contains(Operator.Multiply, operators);
		Assert.Contains(Operator.Divide, operators);
		Assert.Contains(Operator.Exponential, operators);
		Assert.Contains(Operator.Sine, operators);
	}
}
