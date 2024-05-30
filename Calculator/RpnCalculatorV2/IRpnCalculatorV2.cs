namespace Calculator.RpnCalculatorV2
{
	public interface IRpnCalculatorV2
	{
		int ItemCount { get; }
		void Clear();
		string PrintStackContents();
		bool TryAddItem(string input);
		void TryPerformOperation();
	}
}