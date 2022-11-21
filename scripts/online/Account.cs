using Godot;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Online
{
	public class Account
	{
		public bool Authenticated { get; private set; } = false;
		public string ErrorMessage { get; private set; }

		public User User { get; private set; }
		private Account(string username)
		{
			this.User = new User(username);
		}
		public static Account AttemptLogin(string username, string password)
		{
			var account = new Account(username);
			var formData = new JObject();
			formData["username"] = username;
			formData["password"] = password;
			var result = Core.HttpClient.PostSync("auth/login", formData.ToHttpContent());
			var content = result.Content.ReadAsJsonSync();
			if ((bool)content.GetValue("has_error"))
				account.ErrorMessage = (string)content.GetValue("msg");
			return account;
		}
	}
}