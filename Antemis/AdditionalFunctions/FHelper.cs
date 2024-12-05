namespace Antemis.AdditionalFunctions
{
	public static class FHelper
	{
		public static bool OfDigits(string str)
		{
			for (int i = 0; i < str.Length; i++)
				if (!Char.IsDigit(str[i]))
					return false;
			return true;
		}
	}
}
