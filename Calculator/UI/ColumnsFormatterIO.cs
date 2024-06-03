using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.UI;

public class ColumnsFormatterIO(IBasicIO? io = null) : IStringFormatter
{
	private readonly IBasicIO? _io = io;
	public string[] StringFormat { get; set; } = ["{0, 20}", "{1, -20}"];
	public string Separator { get; set; } = " ";
	public void Clear()
	{
		_io?.Clear();
	}

	public string GetInput()
	{
		return _io?.GetInput() ?? string.Empty;
	}

	public FormattedString GetFormattedString(params string?[] text)
	{
		var formattedString = Format(text);
		return new FormattedString(formattedString);
	}

	public void PushOutput(params string?[] text)
	{
		var formattedText = Format(text);
		_io?.PushOutput(formattedText);
	}
	public string Format(params string?[] text)
	{
		var shortest = text.Length <= StringFormat.Length ? text.Length : StringFormat.Length;
		var format = string.Join(Separator, StringFormat[..shortest]);
		var formattedText = string.Format(format, text);
		return formattedText;
	}
}