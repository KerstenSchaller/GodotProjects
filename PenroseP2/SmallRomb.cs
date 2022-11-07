using Godot;
using System;

public class SmallRomb : Dragable
{

	CollisionPolygon2D collisionPoly;

	public override void _Ready()
	{
		GD.Print(this.Name);
		GD.Print(this.GetPath());
		base._Ready();

		collisionPoly = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
		collisionPoly.Polygon = KiteAndDart.getDartVertices().ToArray();
		GD.Print("1" + KiteAndDart.getDartVertices().ToArray().ToString());
	}


	public override void _Draw()
	{
		DrawColoredPolygon(collisionPoly.Polygon, Colors.Aqua);
		GD.Print("2" + collisionPoly.Polygon.ToString());
	}
}





