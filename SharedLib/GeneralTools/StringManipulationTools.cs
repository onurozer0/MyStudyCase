namespace SharedLib.GeneralTools
{
	public static class StringManipulationTools
	{
		public static string GetUsernameByEmail(string email)
		{
			email = email.Trim().ToLower();
			int index = email.IndexOf("@");
			string username = email.Substring(0, index);
			string rnd = new Random().Next(100, 10000).ToString();
			username = username + rnd;
			return username;
		}
	}
}
