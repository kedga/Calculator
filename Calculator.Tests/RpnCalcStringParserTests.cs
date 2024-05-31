using Calculator.RpnCalculatorV2;
using Calculator.UI;
using Moq;
using System;
using Xunit;

namespace Calculator.Tests;

public class RpnCalcStringParserTests
{
	private readonly RpnCalcV2 _calc = new();
	private readonly RpnCalcStringParser _parser = new();

	[Fact]
	public void TryPerformOperation_WithValidInput_ReturnsExpectedResult()
	{
		// Arrange
		_parser.TryAddItem("3", _calc);
		_parser.TryAddItem("5", _calc);
		_parser.TryAddItem("+", _calc);
		_parser.TryAddItem("8", _calc);
		_parser.TryAddItem("/", _calc);
		_parser.TryAddItem("3", _calc);
		_parser.TryAddItem("6", _calc);
		_parser.TryAddItem("+", _calc);
		_parser.TryAddItem("*", _calc);

		// Act
		var result = _calc.TryPerformOperation();

		// Assert
		Assert.NotNull(result);
		Assert.Equal(9, result.Value);
	}

	[Fact]
	public void TryAddItem_AddsOperandsAndOperators_CorrectlyUpdatesItemCount()
	{
		// Arrange

		// Act
		_parser.TryAddItem("3", _calc);
		_parser.TryAddItem("5", _calc);
		_parser.TryAddItem("+", _calc);
		_parser.TryAddItem("8", _calc);
		_parser.TryAddItem("/", _calc);

		// Assert
		Assert.Equal(5, _calc.ItemCount);
	}

	[Fact]
	public void Clear_ClearsAllItems_ItemCountIsZero()
	{
		// Arrange
		_parser.TryAddItem("3", _calc);
		_parser.TryAddItem("5", _calc);
		_parser.TryAddItem("+", _calc);

		// Act
		_calc.Clear();

		// Assert
		Assert.Equal(0, _calc.ItemCount);
	}

	[Fact]
	public void PrintStackContents_ReturnsCorrectContents()
	{
		// Arrange
		_parser.TryAddItem("3", _calc);
		_parser.TryAddItem("5", _calc);

		// Act
		var contents = _calc.PrintStackContents();

		// Assert
		Assert.Equal("[ 3 5 ]", contents);
	}

	[Fact]
	public void TryPerformOperation_WithInvalidOperation_ReturnsNull()
	{
		// Arrange
		_parser.TryAddItem("3", _calc);
		_parser.TryAddItem("/", _calc);

		// Act
		var result = _calc.TryPerformOperation();

		// Assert
		Assert.Null(result);
	}
}