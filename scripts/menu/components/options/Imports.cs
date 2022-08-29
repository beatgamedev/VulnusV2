using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Imports : Control
{
	private int currentAction = 0;
	public Node FolderDialog;
	public Node SelectFileDialog;
	public override void _Ready()
	{
		FolderDialog = GetNode<Node>("FolderDialog");
		FolderDialog.Connect("folder_selected", this, nameof(FolderSelected));
		SelectFileDialog = GetNode<Node>("FileDialog");
		SelectFileDialog.Connect("files_selected", this, nameof(FileSelected));
		GetNode<Button>("OldMap").Connect("pressed", this, nameof(SetCurrentAction), new Godot.Collections.Array(1));
		GetNode<Button>("SSPMap").Connect("pressed", this, nameof(SetCurrentAction), new Godot.Collections.Array(2));
	}
	public void SetCurrentAction(int action)
	{
		if (currentAction != 0)
			return;
		currentAction = action;
		switch (action)
		{
			case 1:
				FolderDialog.Set("title", "Select an old map (folder)");
				FolderDialog.Call("show");
				break;
			case 2:
				SelectFileDialog.Set("title", "Select a Sound Space Plus map (.sspm)");
				SelectFileDialog.Call("clear_filters");
				SelectFileDialog.Call("add_filter", "*.sspm ; Sound Space Plus map");
				SelectFileDialog.Call("show");
				break;
			default:
				currentAction = 0;
				break;
		}
	}
	public void FolderSelected(string folder)
	{
		FolderDialog.Call("hide");
		if (folder == "")
		{
			currentAction = 0;
			return;
		}
		switch (currentAction)
		{
			case 1:
				Compatibility.Old.Map.ImportFolder(folder);
				BeatmapLoader.LoadMapsFromDirectory(Global.MapPath);
				break;
			default:
				break;
		}
		currentAction = 0;
	}
	public void FileSelected(string[] files)
	{
		SelectFileDialog.Call("hide");
		if (files.Length < 1)
		{
			currentAction = 0;
			return;
		}
		var file = files[0];
		switch (currentAction)
		{
			case 2:
				Compatibility.SSP.Map.Import(file);
				BeatmapLoader.LoadMapsFromDirectory(Global.MapPath);
				break;
			default:
				break;
		}
		currentAction = 0;
	}
}
