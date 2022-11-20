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

	public List<Vector2> toList()
	{
		List<Vector2> retval = new List<Vector2>();
		retval.Add(shiftedPoint);
		//retval.Add( new Vector2(shiftedPoint.x +(float)Math.Cos(angleRad),shiftedPoint.y + (float)Math.Sin(angleRad)));
		retval.Add(intersectionPoint);
		return retval;
	}

	Vector2 intersectionPoint = new Vector2(0,0);

	HankinLine neighbour;

	public void addNeighbour(HankinLine _neighbour)
	{
		neighbour = _neighbour;
		intersectionPoint = PolygonDetectionAlgorithm.calcIntersection(this.shiftedPoint, this.angleRad, neighbour.Point, neighbour.AngleRad);
		Update();
	}

	public Vector2 Point
	{
		get{return shiftedPoint;}
	}

	public float AngleRad{get{return angleRad;}}

	public float Angle
	{
		get{return angleDeg;}//Bug, this should return
        set
        {
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
		shiftPoint(offset);
	}

	public override void _Draw()
	{
		if(neighbour == null)return;

		intersectionPoint = PolygonDetectionAlgorithm.calcIntersection(this.shiftedPoint, this.angleRad, neighbour.Point, neighbour.AngleRad);

		//GD.Print("angle:" + angleRad * 180/Math.PI);

		var x = (float)Math.Cos(angleRad);
		var y = (float)Math.Sin(angleRad);
		DrawLine(shiftedPoint, intersectionPoint, Colors.White,1);
		//DrawCircle(intersectionPoint,3f ,Colors.IndianRed);
		//DrawLine(point, point +  new Vector2(x,y)*50, Colors.Violet);
	}	
} 