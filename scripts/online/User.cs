using Godot;
using System;
using Newtonsoft.Json.Linq;

namespace Online
{
	public class User
	{
		public string Username { get; private set; }
		public string Id { get; private set; } = "null";
		public string AvatarUrl { get; private set; } = "null";
		public Texture AvatarTexture { get; private set; } = Global.Matt;
		public User(string username)
		{
			this.Username = username;
		}
		public User(JObject json)
		{
			this.Id = (string)json.GetValue("id");
			this.Username = (string)json.GetValue("username");
		}
		public static User FromId(string id)
		{
			var req = Core.HttpClient.GetSync($"user/{id}");
			var content = req.Content.ReadAsJsonSync();
			if ((string)content.GetValue("status") == "Ok")
				return new User(content);
			else
				throw new Exception("User doesn't exist");
		}
		public void LoadAvatar()
		{
			if (AvatarUrl == "null")
				return;

		}
	}
}