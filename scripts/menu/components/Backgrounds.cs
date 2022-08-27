using Godot;
using System;
using System.Collections.Generic;

public class Backgrounds : Control
{
	public TextureRect Texture;
	public List<Texture> Textures;
	private int background = 0;
	private Tween tween;
	private DateTimeOffset lastChange = DateTimeOffset.Now;
	public override void _Ready()
	{
		tween = new Tween();
		AddChild(tween);
		Texture = GetNode<TextureRect>("Texture");
		Textures = new List<Texture>();
		var dir = new Directory();
		dir.Open("res://assets/backgrounds");
		dir.ListDirBegin(true, true);
		string file = "placeholder";
		while (file != "")
		{
			file = dir.GetNext();
			if (file.Extension() == "import" || file.GetFile() == "")
				continue;
			try
			{
				var image = dir.GetCurrentDir().PlusFile(file);
				var texture = (StreamTexture)GD.Load(image);
				Textures.Add(texture);
			}
			catch { }
		}
		dir.ListDirEnd();
		Texture.Texture = Textures[0];
	}
	public override void _Process(float delta)
	{
		var now = DateTimeOffset.Now;
		if ((now - lastChange).Seconds > 30)
		{
			lastChange = now;
			var transition = Texture.Duplicate();
			AddChild(transition);
			MoveChild(transition, 1);
			var next = Textures[Mathf.PosMod(background++, Textures.Count)];
			if (new Random().Next(0, 50) == 1)
				next = Global.Matt;
			Texture.Texture = next;
			tween.StopAll();
			tween.InterpolateProperty(transition, "modulate:a", 1f, 0f, 0.5f, Tween.TransitionType.Quart, Tween.EaseType.InOut);
			if (tween.IsConnected("tween_all_completed", this, nameof(TweenFinished)))
				tween.Disconnect("tween_all_completed", this, nameof(TweenFinished));
			tween.Connect("tween_all_completed", this, nameof(TweenFinished), new Godot.Collections.Array(transition));
			tween.Start();
		}
	}
	public void TweenFinished(TextureRect transition)
	{
		transition.QueueFree();
	}
}
