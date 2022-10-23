using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public static class Online
{
	public static string ErrorMessage { get; private set; }
	private static VulnusHttpClient httpClient = new VulnusHttpClient();
	public enum ServerStatus
	{
		Ok = 0,
		Offline = 1
	}
	public static ServerStatus CurrentServerStatus()
	{
		var result = httpClient.GetSync("status");
		if (result.IsSuccessStatusCode)
		{
			return (ServerStatus)int.Parse(result.Content.ReadAsStringSync());
		}
		return ServerStatus.Offline;
	}
	public enum OnlineResult
	{
		Success,
		LoginFailed,
		NotAuthorised
	}
	public static OnlineResult AttemptLogin(string username, string password)
	{
		var formData = new StringContent($"{{\"u\":\"{username}\",\"p\":\"{password}\"}}");
		var result = httpClient.PostSync("auth/login", formData);
		var content = JObject.Parse(result.Content.ReadAsStringSync());
		if ((bool)content.GetValue("success") == true)
			return OnlineResult.Success;
		ErrorMessage = (string)content.GetValue("message");
		return OnlineResult.LoginFailed;
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
public static class HttpExtensions
{
	public static string ReadAsStringSync(this HttpContent self)
	{
		var task = self.ReadAsStringAsync();
		Task.WaitAll(task);
		return task.Result as string;
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