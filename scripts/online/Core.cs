using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Online
{
	public enum Status
	{
		Ok = 0,
		Offline = 1
	}
	public enum Result
	{
		Success,
		LoginFailed,
		NotAuthorised
	}
	public static class Core
	{
		public static Result Result { get; private set; }
		public static string ErrorMessage { get; private set; }
		public static VulnusHttpClient HttpClient { get; private set; } = new VulnusHttpClient();
		public static Status ServerStatus()
		{
			var result = HttpClient.GetSync("status");
			if (result.IsSuccessStatusCode)
			{
				return (Status)int.Parse(result.Content.ReadAsStringSync());
			}
			return Status.Offline;
		}
	}
	public class VulnusHttpClient : HttpClient
	{
		private static HttpClientHandler Handler = new HttpClientHandler();
		public VulnusHttpClient() : base(Handler)
		{
			Handler.AllowAutoRedirect = false;
			Handler.CookieContainer = new CookieContainer();
			Handler.UseCookies = true;
			this.DefaultRequestHeaders.UserAgent.ParseAdd("Vulnus/1.0");
			this.DefaultRequestHeaders.Accept.ParseAdd("application/json");
			this.BaseAddress = new Uri("https://vulnus.net/api/");
		}
	}
	public static class JObjectExtensions
	{
		public static HttpContent ToHttpContent(this JObject self)
		{
			return new StringContent(self.ToString(Formatting.None));
		}
	}
	public static class HttpExtensions
	{
		public static string ReadAsStringSync(this HttpContent self)
		{
			var task = self.ReadAsStringAsync();
			Task.WaitAll(task);
			return task.Result as string;
		}
		public static JObject ReadAsJsonSync(this HttpContent self)
		{
			var text = self.ReadAsStringSync();
			return JObject.Parse(text);
		}
		public static HttpResponseMessage GetSync(this HttpClient self, string url)
		{
			var task = self.GetAsync(url);
			Task.WaitAll(task);
			return task.Result as HttpResponseMessage;
		}
		public static HttpResponseMessage PostSync(this HttpClient self, string url, HttpContent content)
		{
			var task = self.PostAsync(url, content);
			Task.WaitAll(task);
			return task.Result as HttpResponseMessage;
		}
		public static HttpResponseMessage PutSync(this HttpClient self, string url, HttpContent content)
		{
			var task = self.PutAsync(url, content);
			Task.WaitAll(task);
			return task.Result as HttpResponseMessage;
		}
		public static HttpResponseMessage DeleteSync(this HttpClient self, string url)
		{
			var task = self.DeleteAsync(url);
			Task.WaitAll(task);
			return task.Result as HttpResponseMessage;
		}
	}
}