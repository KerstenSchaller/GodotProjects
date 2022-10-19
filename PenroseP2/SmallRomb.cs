using Godot;
using System;

public class SmallRomb : Dragable
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public override void _Ready()
	{
		GD.Print(this.Name);
		GD.Print(this.GetPath());
		base._Ready();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}






