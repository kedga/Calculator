using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.UI;

public class FormattedStringOutput(IBasicIO io)
{
	private readonly IBasicIO _io = io;
	public void PushOutput(FormattedString formattedString)
	{
		_io.PushOutput(formattedString.String);
	}
}
