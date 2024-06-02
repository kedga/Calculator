namespace Calculator.RpnCalculatorV2;

public interface IRpnCalculator
{
	int ItemCount { get; }
	void Clear();
	void PrintStackContents();
	void AddOperator(Operator @operator);
	void AddOperand(double value);
	void RemoveLastItem();
	double? TryPerformOperation();
	void PrintErrorMessage(string message);
}