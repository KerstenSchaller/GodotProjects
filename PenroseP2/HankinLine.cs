using Godot;
using System;
using System.Collections.Generic;

public class HankinLine : Node2D
{
	Vector2 point;
	Vector2 shiftedPoint;

    float angleDeg;
	float angleRad;
	float baseAngleRad;

	float offset;



	Vector2 intersectionPoint = new Vector2(0,0);

	HankinLine neighbour;

	public void addNeighbour(HankinLine _neighbour)
	{
		neighbour = _neighbour;
		calcIntersection();
		Update();
	}

	public Vector2 Point
	{
		get{return shiftedPoint;}
	}

	public float Angle
	{
		get{return angleRad;}
        set
        {
			//GD.Print("initinit: " + angleRad);
            angleDeg = value;
            init(point, angleDeg, baseAngleRad);
        } 
	}

    public float Offset
    {
        get { return offset; }
        set
        {
            offset = value;
            //init(point, angleDeg, baseAngleRad);
			shiftPoint(offset);
        }
    }

    public void calcIntersection()
	{
		if(neighbour == null)return;

		//http://paulbourke.net/geometry/pointlineplane/

		var x1 = shiftedPoint.x;
		var y1 = shiftedPoint.y;
		var x2 = x1 + (float)Math.Cos(angleRad);
		var y2 = y1 + (float)Math.Sin(angleRad);

		var x3 = neighbour.Point.x;
		var y3 = neighbour.Point.y;
		var x4 = x3 + (float)Math.Cos(neighbour.Angle);
		var y4 = y3 + (float)Math.Sin(neighbour.Angle);

		var nominator = (x4 - x3)*(y1-y3) - (y4-y3)*(x1-x3);
		var denominator = (y4 - y3)*(x2-x1) - (x4-x3)*(y2-y1);

		var ua = nominator/denominator;
		var xIntersect = x1 + ua*(x2-x1);
		var yIntersect = y1 + ua*(y2-y1);

		intersectionPoint = new Vector2(xIntersect,yIntersect);

 
	}

    public override void _Process(float delta)
    {
        Update();
    }

	void shiftPoint(float _offset)
	{
		//shift point by offset
		var turnedAngle = baseAngleRad + (float)Math.PI/2;
		Vector2 vShift = new Vector2((float)Math.Cos(turnedAngle),(float)Math.Sin(turnedAngle))*_offset;
		shiftedPoint = point + vShift;
	}

	public void init(Vector2 _point, float _angleDeg, float _baseAngleRad)
	{

		baseAngleRad = _baseAngleRad;

		point = _point;
		angleRad = baseAngleRad + _angleDeg * (float)Math.PI/180;
	}

	public override void _Draw()
	{
		if(neighbour == null)return;
        calcIntersection();

		//GD.Print("angle:" + angleRad * 180/Math.PI);

		var x = (float)Math.Cos(angleRad);
		var y = (float)Math.Sin(angleRad);
		DrawLine(shiftedPoint, intersectionPoint, Colors.White,1);
		//DrawCircle(intersectionPoint,3f ,Colors.IndianRed);
		//DrawLine(point, point +  new Vector2(x,y)*50, Colors.Violet);
	}	
} 