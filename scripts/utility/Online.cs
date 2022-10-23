using Godot;
using System;
using System.Net.Http;

public static class Online
{
	
}
public class VulnusWebRequest : HttpWebRequest
{
	public VulnusWebRequest(string url) : base.Create(url) {

	}
}
