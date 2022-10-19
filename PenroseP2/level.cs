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
		
	}

	void setScaleOfChilds(float scale)
	{
		var bigRomb = GetNode<Node2D>("BigRomb");
		var smallRomb = GetNode<Node2D>("SmallRomb");
		if(bigRomb == null || smallRomb == null)return;
		bigRomb.Scale = new Vector2(scale,scale);
		smallRomb.Scale = new Vector2(scale,scale);;
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	//if(Input.IsMouseButtonPressed(ButtonList.)))
	  
  }
}
