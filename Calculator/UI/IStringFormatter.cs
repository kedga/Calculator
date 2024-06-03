namespace Calculator.UI;

public interface IStringFormatter
{
	string Format(params string?[] text);
	FormattedString GetFormattedString(params string?[] text);
}