namespace Calculator.RpnCalculatorV2;

public interface IRpnCalculatorV2
{
	int ItemCount { get; }
	void Clear();
	string PrintStackContents();
	void AddOperator(Operator @operator);
	void AddOperand(double value);
	void RemoveLastItem();
	double? TryPerformOperation();
}