using Godot;
using System;

public class BigRomb : KinematicBody2D
{

	float speed = 0;
	CollisionPolygon2D collisionPoly;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(this.Name);
		GD.Print(this.GetPath());

		base._Ready();

		var dragable = new Dragable();
		dragable.overrideChild(this);
		AddChild(dragable);

		collisionPoly = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
		collisionPoly.Polygon = KiteAndDart.getKiteVertices().ToArray();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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





