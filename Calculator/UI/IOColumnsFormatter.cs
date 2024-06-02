using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.UI;

public class IOColumnsFormatter(IBasicIO? io = null) : IBasicIO
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

	public void PushOutput(params string?[] text)
	{
		var shortest = text.Length <= StringFormat.Length ? text.Length : StringFormat.Length;
		var format = string.Join(Separator, StringFormat[..shortest]);
		var formattedText = string.Format(format, text);
		_io?.PushOutput(formattedText);
	}
}