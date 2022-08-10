using Godot;
using System;

public class Play : View
{
	private Control mapList;
	private Control mapDetail;
	public override void _Ready()
	{
		mapList = GetNode<Control>("MapList");
		mapDetail = GetNode<Control>("MapDetail");
	}
}
