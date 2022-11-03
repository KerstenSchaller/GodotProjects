using Godot;
using System;

public class Dragable : KinematicBody2D
{


	bool isPickedUp = false;
	KinematicBody2D parent;
	KinematicBody2D childKinematicBody;

	bool isAncestor  = true;

	public override void _Ready()
	{
		this.Connect("input_event", this, nameof(_on_KinematicBody2D_input_event));
	}



	public void overrideChild(KinematicBody2D node)
	{
		parent = node;
		parent.Connect("input_event", this, nameof(_on_KinematicBody2D_input_event));
		isAncestor = false; // is overriden means no ancestry(not used to inherit from)
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
		//GD.Print("Collision Occured");
		if(inputEvent.IsActionPressed("mouse_button_left"))
		{
			isPickedUp = true;
			//GD.Print("clicked Node");
		}	
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (isPickedUp)
			{
				
				// the target object is different if this class was inherited rather than instanciated
				
				if(isAncestor)
				{
					this.Position = eventMouseMotion.Position;
					//this.MoveAndSlide(eventMouseMotion.Position);
				}
				else
				{
					parent.Position = eventMouseMotion.Position;
					//parent.MoveAndSlide(eventMouseMotion.Position);
				}
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
				if(isAncestor)
				{
					this.RotationDegrees = this.RotationDegrees + 360f/60;
				}
				else
				{
					parent.RotationDegrees = parent.RotationDegrees + 360f/60;
				}
			}
		}
	}

}
