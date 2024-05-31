using Calculator.RpnCalculatorV2;
using Calculator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculator.RpnCalculatorV2.RpnCalcV2;

namespace Calculator.Tests;

public class SplitListTests
{
	[Fact]
	public void SplitList_NormalCase_SplitsCorrectly()
	{
		// Arrange
		var items = new List<int>
		{
			0, 1, 2, 3, 4, 5, 6
		};
		int index = 4;
		int leftOffset = 2;

		// Act
		var (leftItems, middleItems, rightItems) = SplitList(items, index, leftOffset);

		// Assert
		Assert.Equal(0, leftItems.First());
		Assert.Equal(1, leftItems.Last());

		Assert.Equal(2, middleItems.First());
		Assert.Equal(4, middleItems.Last());

		Assert.Equal(5, rightItems.First());
		Assert.Equal(6, rightItems.Last());
	}

	[Fact]
	public void SplitList_NoOffset_SplitsCorrectly()
	{
		// Arrange
		var items = new List<int>
		{
			0, 1, 2, 3, 4, 5, 6
		};
		int index = 4;
		int leftOffset = 0;

		// Act
		var (leftItems, middleItems, rightItems) = SplitList(items, index, leftOffset);

		// Assert
		Assert.Equal(0, leftItems.First());
		Assert.Equal(3, leftItems.Last());

		Assert.Equal(4, middleItems.First());
		Assert.Equal(4, middleItems.Last());

		Assert.Equal(5, rightItems.First());
		Assert.Equal(6, rightItems.Last());
	}
	[Fact]
	public void SplitList_IndexAtStart_SplitsCorrectly()
	{
		// Arrange
		var items = new List<int>
		{
			0, 1, 2, 3, 4, 5, 6
		};
		int index = 0;
		int leftOffset = 0;

		// Act
		var (leftItems, middleItems, rightItems) = SplitList(items, index, leftOffset);

		// Assert
		Assert.Empty(leftItems);

		Assert.Equal(0, middleItems.First());
		Assert.Equal(0, middleItems.Last());

		Assert.Equal(1, rightItems.First());
		Assert.Equal(6, rightItems.Last());
	}
	[Fact]
	public void SplitList_LeftOffsetLargerThanIndex_Throws()
	{
		// Arrange
		var items = new List<int>
		{
			0, 1, 2, 3, 4, 5, 6
		};
		int index = 2;
		int leftOffset = 3;

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => SplitList(items, index, leftOffset));
	}
	[Fact]
	public void SplitList_IndexAtEnd_SplitsCorrectly()
	{
		// Arrange
		var items = new List<int>
		{
			0, 1, 2, 3, 4, 5, 6
		};
		int index = 6;
		int leftOffset = 1;

		// Act
		var (leftItems, middleItems, rightItems) = SplitList(items, index, leftOffset);

		// Assert
		Assert.Equal(0, leftItems.First());
		Assert.Equal(4, leftItems.Last());

		Assert.Equal(5, middleItems.First());
		Assert.Equal(6, middleItems.Last());

		Assert.Empty(rightItems);
	}
	[Fact]
	public void SplitList_NegativeLeftOffset_Throws()
	{
		// Arrange
		var items = new List<int>
		{
			0, 1, 2, 3, 4, 5, 6
		};
		int index = 4;
		int leftOffset = -2;

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => SplitList(items, index, leftOffset));
	}
}