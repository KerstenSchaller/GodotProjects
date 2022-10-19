using Godot;
using System;

public class Dragable : Node2D
{


	bool isPickedUp = false;
	Node2D parent;
	KinematicBody2D childKinematicBody;

	bool isAncestor  = true;

	public override void _Ready()
	{
		KinematicBody2D child = this.recurseGetNode(this);
		if (child is KinematicBody2D)
		{
			childKinematicBody = child;
			childKinematicBody.Connect("input_event", this, nameof(_on_KinematicBody2D_input_event));
		}
	}



	public void overrideChild(Node2D node)
	{
		parent = node;
		KinematicBody2D child = this.recurseGetNode(node);
		if (child is KinematicBody2D)
		{
			childKinematicBody = child;
			childKinematicBody.Connect("input_event", this, nameof(_on_KinematicBody2D_input_event));
			isAncestor = false; // is overriden means no ancestry(not used to inherit from)
		}

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
		//GD.Print("Hovering node");
		if(inputEvent.IsActionPressed("mouse_button_left"))
		{
			isPickedUp = true;
			GD.Print("clicked Node");
		}

		
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (isPickedUp)
			{
				GD.Print("moving Node");
				
				// the target object is different if this class was inherited rather than instanciated
				if(isAncestor)
				{
					this.Position = eventMouseMotion.Position;
				}
				else
				{
					((Node2D)GetParent()).Position = eventMouseMotion.Position;
				}
			}
		}

		if(@event.IsActionReleased("mouse_button_left"))
		{
			isPickedUp = false;
			GD.Print("released Node");
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

	}

}
