using Godot;
using System;

public class BigRomb : Dragable
{

/*
	public override void _Input(InputEvent inputEvent)
	{
		if(inputEvent.IsActionPressed("mouse_button_left"))
		{
			InputEventMouseButton mouseInput = inputEvent as InputEventMouseButton;
			//if(mouseInput.Doubleclick)
			{
				GD.Print(mouseInput.Position);
				
			}
		}
	}
*/



	// Called when the node enters the scene tree for the first time.
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





