using Godot;
using System;

public class Dragable : Node2D
{


	bool isPickedUp = false;
	KinematicBody2D parent;
	KinematicBody2D childKinematicBody;

	public override void _Ready()
	{
	}

	public void overrideChild(KinematicBody2D node)
	{
		parent = node;
		parent.Connect("input_event", this, nameof(_on_KinematicBody2D_input_event));
	}

	KinematicBody2D recurseGetNode(Node2D node)
	{
		var children = node.GetChildren();
		foreach(var child in children )
		{
			if(child is KinematicBody2D)
			{
				return (KinematicBody2D)child;
			}
			else
			{
				return (KinematicBody2D)recurseGetNode((Node2D)child);
			}
		}
		return null;

	}

	public void _on_KinematicBody2D_input_event(object viewport, object @event, int shape_idx)
	{
		var inputEvent = @event as InputEventMouse;
		if(inputEvent.IsActionPressed("mouse_button_left"))
		{
			isPickedUp = true;
		}	
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (isPickedUp)
			{
				parent.Position = eventMouseMotion.Position;
				//parent.MoveAndSlide(eventMouseMotion.Position);
			}
		}

		if(@event.IsActionReleased("mouse_button_left"))
		{
			isPickedUp = false;
			//GD.Print("released Node");
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

	}

	public override void _PhysicsProcess(float delta)
	{

		if(Input.IsActionPressed("Rotate"))
		{
			if(isPickedUp)
			{
				GD.Print("rotate");				
				parent.RotationDegrees = parent.RotationDegrees + 360f/60;
				
			}
		}
	}

}
