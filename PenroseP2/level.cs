using Godot;
using System;

public class level : Node2D
{
	private float scale;

	[Export(PropertyHint.Range, "0,10,0.1")]
	public float _Scale
	{
		get{return scale;}
		set{
			scale = value;
			setScaleOfChilds(scale);
			}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
	}

	void setScaleOfChilds(float scale)
	{
		var bigRomb = GetNode<Node2D>("BigRomb");
		var smallRomb = GetNode<Node2D>("SmallRomb");
		if(bigRomb == null || smallRomb == null)return;
		bigRomb.Scale = new Vector2(scale,scale);
		smallRomb.Scale = new Vector2(scale,scale);
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	//if(Input.IsMouseButtonPressed(ButtonList.)))
	  
  }

  	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if(@event.IsActionPressed("mouse_button_right"))
			{
				var pos = eventMouseButton.Position;
				GD.Print("clicked at " + pos);

				var randomInt =  GD.Randi() % 2;
				if(randomInt == 1)
				{
					var bigRombScene = ResourceLoader.Load("res://BigRomb.tscn") as PackedScene;
					BigRomb newChild = bigRombScene.Instance() as BigRomb;
					newChild.Position = pos;
					newChild.Scale = new Vector2(scale,scale);
					this.AddChild(newChild);

				}
				else
				{
					var smallRombScene = ResourceLoader.Load("res://SmallRomb.tscn") as PackedScene;
					SmallRomb newChild = smallRombScene.Instance() as SmallRomb;
					newChild.Position = pos;
					newChild.Scale = new Vector2(scale,scale);
					this.AddChild(newChild);

				}


			}
		}

		if(@event.IsActionReleased("mouse_button_right"))
		{
		}

	}
}
