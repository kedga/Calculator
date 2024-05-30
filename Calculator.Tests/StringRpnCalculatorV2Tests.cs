using Calculator.RpnCalculatorV2;
using Calculator.UI;
using System;
using Xunit;

namespace Calculator.Tests;

public class StringRpnCalculatorV2Tests
{
	private static StringRpnCalcV2 CreateCalculator()
	{
		return new StringRpnCalcV2(new ConsoleUI());
	}

	[Fact]
	public void TryPerformOperation_WithValidInput_ReturnsExpectedResult()
	{
		// Arrange
		var calculator = CreateCalculator();
		calculator.TryAddItem("3");
		calculator.TryAddItem("5");
		calculator.TryAddItem("+");
		calculator.TryAddItem("8");
		calculator.TryAddItem("/");
		calculator.TryAddItem("3");
		calculator.TryAddItem("6");
		calculator.TryAddItem("+");
		calculator.TryAddItem("*");

		// Act
		var result = calculator.TryPerformOperation();

		// Assert
		Assert.NotNull(result);
		Assert.Equal(9, result.Value);
	}

	[Fact]
	public void TryAddItem_AddsOperandsAndOperators_CorrectlyUpdatesItemCount()
	{
		// Arrange
		var calculator = CreateCalculator();

		// Act
		calculator.TryAddItem("3");
		calculator.TryAddItem("5");
		calculator.TryAddItem("+");
		calculator.TryAddItem("8");
		calculator.TryAddItem("/");

		// Assert
		Assert.Equal(5, calculator.ItemCount);
	}

	[Fact]
	public void Clear_ClearsAllItems_ItemCountIsZero()
	{
		// Arrange
		var calculator = CreateCalculator();
		calculator.TryAddItem("3");
		calculator.TryAddItem("5");
		calculator.TryAddItem("+");

		// Act
		calculator.Clear();

		// Assert
		Assert.Equal(0, calculator.ItemCount);
	}

	[Fact]
	public void PrintStackContents_ReturnsCorrectContents()
	{
		// Arrange
		var calculator = CreateCalculator();
		calculator.TryAddItem("3");
		calculator.TryAddItem("5");

		// Act
		var contents = calculator.PrintStackContents();

		// Assert
		Assert.Equal("[ 3 5 ]", contents);
	}

	[Fact]
	public void TryPerformOperation_WithInvalidOperation_ReturnsNull()
	{
		// Arrange
		var calculator = CreateCalculator();
		calculator.TryAddItem("3");
		calculator.TryAddItem("/");

		// Act
		var result = calculator.TryPerformOperation();

		// Assert
		Assert.Null(result);
	}
}