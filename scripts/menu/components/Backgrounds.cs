using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Backgrounds : Control
{
	private Random random = new Random();
	public TextureRect Texture;
	public List<Texture>
		Textures = new List<Texture>{
			(StreamTexture)GD.Load("res://assets/backgrounds/coffee.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/gok.png"),
			(StreamTexture)GD.Load("res://assets/backgrounds/sunset.png"),
			(StreamTexture)GD.Load("res://assets/backgrounds/animegril.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/awesomeassstreet.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/coolcity.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/flyingship.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/funnyasstree.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/greybganimegirl.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/skyclouds.jpg"),
			(StreamTexture)GD.Load("res://assets/backgrounds/treewithswing.jpg")
		};
	private int background = 0;
	private Tween tween;
	private DateTimeOffset lastChange = DateTimeOffset.Now;
	public override void _Ready()
	{
		Textures = Textures.OrderBy(_ => random.Next()).ToList();
		tween = new Tween();
		AddChild(tween);
		Texture = GetNode<TextureRect>("Texture");
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
			if (random.Next(0, 50) == 1)
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
