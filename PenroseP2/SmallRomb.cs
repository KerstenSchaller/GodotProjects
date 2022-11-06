using Godot;
using System;

public class SmallRomb : Dragable
{
	public override void _Ready()
	{
		GD.Print(this.Name);
		GD.Print(this.GetPath());
		base._Ready();

		CollisionPolygon2D collisionPoly = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
		collisionPoly.Polygon = KiteAndDart.getDartVertices().ToArray();
	}



//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}






