using Godot;
using System;

public class SmallRomb : KinematicBody2D
{

	CollisionPolygon2D collisionPoly;
	float speed = 0;

	public override void _Ready()
	{
		GD.Print(this.Name);
		GD.Print(this.GetPath());
		base._Ready();

		var dragable = new Dragable();
		dragable.overrideChild(this);
		AddChild(dragable);

		collisionPoly = new CollisionPolygon2D();
		collisionPoly.Polygon = KiteAndDart.getDartVertices().ToArray();
		this.AddChild(collisionPoly);
	}


	public override void _Process(float delta)
	{
		Vector2 velocity = new Vector2(speed,0);
		var collisionInfo = MoveAndCollide(velocity * delta);
		if (collisionInfo != null)
		{
			GD.Print("Collided");
		   // var collisionPoint = collisionInfo.GetPosition();
		}
		this.Update();
	}

	public override void _Draw()
	{
		DrawColoredPolygon(collisionPoly.Polygon, Colors.Aqua);
	}
}





