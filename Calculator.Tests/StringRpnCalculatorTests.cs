using Calculator;
using Calculator.Calculator;
using Calculator.UI;

namespace Calculator.Tests;
public class StringRpnCalculatorTests
{
	[Fact]
	public void AddValue_ShouldAddValueToStack()
	{
		// Arrange
		var calculator = new StringRpnCalculator();

		// Act
		calculator.AddValue(5.0);
		
		// Assert
		Assert.Equal(1, calculator.StackCount);
		Assert.Equal(5.0, calculator.LastNumber);
	}

	[Fact]
	public void TryAddValue_ShouldReturnTrueAndAddValue_WhenInputIsValid()
	{
		// Arrange
		var calculator = new StringRpnCalculator();

		// Act
		var result = calculator.TryAddValue("10");

		// Assert
		Assert.True(result);
		Assert.Equal(1, calculator.StackCount);
		Assert.Equal(10.0, calculator.LastNumber);
	}

	[Fact]
	public void TryAddValue_ShouldReturnFalse_WhenInputIsInvalid()
	{
		// Arrange
		var calculator = new StringRpnCalculator();

		// Act
		var result = calculator.TryAddValue("invalid");

		// Assert
		Assert.False(result);
		Assert.Equal(0, calculator.StackCount);
	}

	[Fact]
	public void Clear_ShouldEmptyTheStack()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(1.0);
		calculator.AddValue(2.0);

		// Act
		calculator.Clear();

		// Assert
		Assert.Equal(0, calculator.StackCount);
	}

	[Fact]
	public void TryPerformOperation_ShouldReturnFalse_WhenStackHasLessThanTwoValues()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(1.0);

		// Act
		var result = calculator.TryPerformOperation("+");

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void TryPerformOperation_ShouldReturnFalse_WhenOperatorIsInvalid()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(1.0);
		calculator.AddValue(2.0);

		// Act
		var result = calculator.TryPerformOperation("invalid");

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void TryPerformOperation_ShouldReturnTrueAndPerformAddition()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(1.0);
		calculator.AddValue(2.0);

		// Act
		var result = calculator.TryPerformOperation("+");

		// Assert
		Assert.True(result);
		Assert.Equal(1, calculator.StackCount);
		Assert.Equal(3.0, calculator.LastNumber);
	}

	[Fact]
	public void TryPerformOperation_ShouldReturnTrueAndPerformSubtraction()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(5.0);
		calculator.AddValue(3.0);

		// Act
		var result = calculator.TryPerformOperation("-");

		// Assert
		Assert.True(result);
		Assert.Equal(1, calculator.StackCount);
		Assert.Equal(2.0, calculator.LastNumber);
	}

	[Fact]
	public void TryPerformOperation_ShouldReturnTrueAndPerformMultiplication()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(2.0);
		calculator.AddValue(4.0);

		// Act
		var result = calculator.TryPerformOperation("*");

		// Assert
		Assert.True(result);
		Assert.Equal(1, calculator.StackCount);
		Assert.Equal(8.0, calculator.LastNumber);
	}

	[Fact]
	public void TryPerformOperation_ShouldReturnTrueAndPerformDivision()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(8.0);
		calculator.AddValue(2.0);

		// Act
		var result = calculator.TryPerformOperation("/");

		// Assert
		Assert.True(result);
		Assert.Equal(1, calculator.StackCount);
		Assert.Equal(4.0, calculator.LastNumber);
	}

	[Fact]
	public void ToString_ShouldReturnStackContents()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(1.0);
		calculator.AddValue(2.0);

		// Act
		var result = calculator.ToString();

		// Assert
		Assert.Equal("[1, 2]", result);
	}

	[Fact]
	public void TryPerformOperation_ShouldReturnFalse_WhenDividingByZero()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(10);
		calculator.AddValue(0);

		// Act
		var result = calculator.TryPerformOperation("/");

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void TryPerformOperation_ShouldNotChangeStack_WhenDividingByZero()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(10);
		calculator.AddValue(0);

		// Act
		calculator.TryPerformOperation("/");

		// Assert
		Assert.Equal("[10, 0]", calculator.ToString());
	}

	[Fact]
	public void TryPerformOperation_Overflow_ReturnsError()
	{
		// Arrange
		var calculator = new StringRpnCalculator();
		calculator.AddValue(double.MaxValue);
		calculator.AddValue(double.MaxValue);

		// Act
		var result = calculator.TryPerformOperation("+");

		// Assert
		Assert.False(result);
		Assert.Contains("Result is out of range for double type", calculator.OutputMessage);
	}
}
