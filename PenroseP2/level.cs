using Godot;
using System;
using System.Collections.Generic;

public class level : Node2D
{
	List<PatternPolygon> polygons = new List<PatternPolygon>();
	private float scale = 0.3f;

	float offset = 0f;
	[Export(PropertyHint.Range, "0,150,1.1")]
	public float exOffset
	{
		get { return offset; }
		set
		{
			foreach(var p in polygons)
			{
				p.Offset = value;
			}

		}
	}


	float hankinsAngle = 75f;
	[Export(PropertyHint.Range, "1,89,1.1")]
	public float exAngle
	{
		get { return hankinsAngle; }
		set
		{
			hankinsAngle = value;
			foreach(var p in polygons)
			{
				p.Angle = value;
			}

		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();



		var patternPolygonScene = ResourceLoader.Load("res://PatternPolygon.tscn") as PackedScene;

		for (int x = 0; x < 4; x++)
		{
			for (int y = 0; y < 3; y++)
			{

				PatternPolygon newChild = patternPolygonScene.Instance() as PatternPolygon;
				newChild.Position = new Vector2(x*300f,y*300f);
				//newChild.Scale = new Vector2(0.1f, 0.1f);
				this.AddChild(newChild);
				polygons.Add(newChild);
			}
		}
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
			if (@event.IsActionPressed("mouse_button_right"))
			{
				var pos = eventMouseButton.Position;
				GD.Print("clicked at " + pos);

				var randomInt = GD.Randi() % 2;
				if (randomInt == 1)
				{
					var bigRombScene = ResourceLoader.Load("res://BigRomb.tscn") as PackedScene;
					BigRomb newChild = bigRombScene.Instance() as BigRomb;
					newChild.Position = pos;
					newChild.Scale = new Vector2(scale, scale);
					this.AddChild(newChild);

				}
				else
				{
					var smallRombScene = ResourceLoader.Load("res://SmallRomb.tscn") as PackedScene;
					SmallRomb newChild = smallRombScene.Instance() as SmallRomb;
					newChild.Position = pos;
					newChild.Scale = new Vector2(scale, scale);
					this.AddChild(newChild);

				}


			}
		}

		if (@event.IsActionReleased("mouse_button_right"))
		{
		}

	}

}
