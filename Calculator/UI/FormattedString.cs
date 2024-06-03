namespace Calculator.UI;

public record FormattedString(string String)
{
	public static FormattedString Empty { get; } = new FormattedString(string.Empty);
}