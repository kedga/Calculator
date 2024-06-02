namespace Calculator.RpnCalculatorV2;

public record Operand(double Value) : CalculatorItem
{
	public override string ToString()
	{
		return $"{Value:G3}";
	}
}
