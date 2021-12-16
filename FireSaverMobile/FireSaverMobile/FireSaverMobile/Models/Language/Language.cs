using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models.Language
{
    public class Language
    {
		public Language(string name, string ci)
		{
			Name = name;
			CI = ci;
		}

		public string Name { get; }

		public string CI { get; }
	}
}
