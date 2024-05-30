using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Calculate;

namespace Calculator.Tests;

using Xunit;
public class CalculateWithRpnCalculatorTests() : CalculatorTests(new SequenceCalculatorBasic()) { }
public class CalculateBasicTests() : CalculatorTests(new SequenceCalculatorBasic()) { }

public abstract class CalculatorTests(ISequenceCalculator calculator)
{
	[Fact]
	public void Add_AddsNumbersCorrectly()
	{
		// Arrange
		double[] numbers = [1.0, 2.0, 3.0];

		// Act
		var result = calculator.Add(numbers);

		// Assert
		Assert.Equal(6.0, result, 5); // Comparing with a precision of 5 decimal places
	}

	[Fact]
	public void Add_HandlesSingleNumber()
	{
		// Arrange
		double[] numbers = [42.0];

		// Act
		var result = calculator.Add(numbers);

		// Assert
		Assert.Equal(42.0, result, 5);
	}

	[Fact]
	public void Add_HandlesNoNumbers()
	{
		// Arrange
		double[] numbers = [];

		// Act
		var result = calculator.Add(numbers);

		// Assert
		Assert.Equal(0.0, result, 5);
	}

	[Fact]
	public void Subtract_SubtractsTwoNumbersCorrectly()
	{
		// Arrange
		double[] numbers = [10.0, 3.0];

		// Act
		var result = calculator.Subtract(numbers);

		// Assert
		Assert.Equal(7.0, result, 5); // 10 - 3 = 7
	}

	[Fact]
	public void Subtract_SubtractsThreeNumbersCorrectly()
	{
		// Arrange
		double[] numbers = [10.0, 5.0, 3.0];

		// Act
		var result = calculator.Subtract(numbers);

		// Assert
		Assert.Equal(2.0, result, 5); // 10 - 5 - 3 = 2
	}

	[Fact]
	public void Subtract_HandlesSingleNumber()
	{
		// Arrange
		double[] numbers = [42.0];

		// Act
		var result = calculator.Subtract(numbers);

		// Assert
		Assert.Equal(42.0, result, 5); // Subtracting a single number should return the number itself
	}

	[Fact]
	public void Subtract_HandlesNoNumbers()
	{
		// Arrange
		double[] numbers = [];

		// Act
		var result = calculator.Subtract(numbers);

		// Assert
		Assert.Equal(0.0, result, 5); // No numbers should return 0
	}

	[Fact]
	public void Multiply_MultipliesNumbersCorrectly()
	{
		// Arrange
		double[] numbers = [2.0, 3.0, 4.0];

		// Act
		var result = calculator.Multiply(numbers);

		// Assert
		Assert.Equal(24.0, result, 5); // 2 * 3 * 4 = 24
	}

	[Fact]
	public void Multiply_HandlesSingleNumber()
	{
		// Arrange
		double[] numbers = [7.0];

		// Act
		var result = calculator.Multiply(numbers);

		// Assert
		Assert.Equal(7.0, result, 5); // Multiplying a single number should return the number itself
	}

	[Fact]
	public void Multiply_HandlesNoNumbers()
	{
		// Arrange
		double[] numbers = [];

		// Act
		var result = calculator.Multiply(numbers);

		// Assert
		Assert.Equal(1.0, result, 5); // No numbers should return 1 (multiplicative identity)
	}

	[Fact]
	public void Divide_DividesTwoNumbersCorrectly()
	{
		// Arrange
		double[] numbers = [36.0, 4.0];

		// Act
		var result = calculator.Divide(numbers);

		// Assert
		Assert.Equal(9, result, 5); // 36 / 4 / = 9
	}

	[Fact]
	public void Divide_DividesThreeNumbersCorrectly()
	{
		// Arrange
		double[] numbers = [20.0, 2.0, 2.0];

		// Act
		var result = calculator.Divide(numbers);

		// Assert
		Assert.Equal(5.0, result, 5); // 20 / 2 / 2 = 5
	}

	[Fact]
	public void Divide_HandlesSingleNumber()
	{
		// Arrange
		double[] numbers = [42.0];

		// Act
		var result = calculator.Divide(numbers);

		// Assert
		Assert.Equal(42.0, result, 5); // Dividing a single number should return the number itself
	}

	[Fact]
	public void Divide_HandlesNoNumbers()
	{
		// Arrange
		double[] numbers = [];

		// Act
		var result = calculator.Divide(numbers);

		// Assert
		Assert.Equal(1.0, result, 5); // No numbers should return 1 (divisive identity)
	}

	[Fact]
	public void Divide_HandlesDivisionByZero()
	{
		// Arrange
		double[] numbers = [42.0, 0.0];

		// Act & Assert
		var exception = Assert.Throws<DivideByZeroException>(() => calculator.Divide(numbers));
		Assert.Equal("Cannot divide by zero", exception.Message);
	}
	[Fact]
	public void Divide_HandlesDivisionByZero_WithThreeNumbers()
	{
		// Arrange
		double[] numbers = [42.0, 0.0, 18.0];

		// Act & Assert
		var exception = Assert.Throws<DivideByZeroException>(() => calculator.Divide(numbers));
		Assert.Equal("Cannot divide by zero", exception.Message);
	}
}
