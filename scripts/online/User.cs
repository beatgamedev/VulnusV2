using Godot;
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
			this.Id = (string)json["id"];
			this.Username = (string)json["username"];
		}
		public void LoadAvatar()
		{
			if (AvatarUrl == "null")
				return;

		}
	}
}