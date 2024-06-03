using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.UI;

public class CenterFormatterIO(IBasicIO? io = null) : IStringFormatter
{
	private readonly IBasicIO? _io = io;
	public int CenterPosition { get; set; }
	public void Clear()
	{
		_io?.Clear();
	}

	public string Format(params string?[] text)
	{
		var combinedText = string.Join(string.Empty, text);
		var textMiddle = combinedText.Length / 2;
		var offset = CenterPosition - textMiddle;
		var leadingSpace = new string(' ', (offset < 0) ? 0 : offset);
		return leadingSpace + combinedText;
	}

	public FormattedString GetFormattedString(params string?[] text)
	{
		var formattedString = Format(text);
		return new FormattedString(formattedString);
	}

	public string GetInput()
	{
		return _io?.GetInput() ?? string.Empty;
	}

	public void PushOutput(params string?[] text)
	{
		_io?.PushOutput(Format(text));
	}
}