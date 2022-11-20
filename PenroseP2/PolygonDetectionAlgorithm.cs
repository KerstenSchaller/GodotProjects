using Godot;
using System;
using System.Collections.Generic;

public class PolygonDetectionAlgorithm : Node2D
{
    List<List<Vector2>> lines;

    public void addLine(List<Vector2> line)
    {
        lines.Add(line);
    } 

    public static Vector2 calcIntersection(Vector2 p1, float angle1, Vector2 p2, float angle2)
	{
		List<Vector2> l1 = new List<Vector2>(){p1,new Vector2( p1.x + (float)Math.Cos(angle1), p1.y +  (float)Math.Sin(angle1))};
		List<Vector2> l2 = new List<Vector2>(){p2,new Vector2( p2.x + (float)Math.Cos(angle2), p2.y +  (float)Math.Sin(angle2))};
		return calcIntersection(l1,l2);
	}

    public static Vector2 calcIntersection(List<Vector2> l1, List<Vector2> l2)
	{
		//http://paulbourke.net/geometry/pointlineplane/

		var x1 = l1[0].x;
		var y1 = l1[0].y;

		var x2 = l1[1].x;
		var y2 = l1[1].y;

		var x3 = l2[0].x;
		var y3 = l2[0].y;

		var x4 = l2[1].x;
		var y4 = l2[1].y;

		var nominator = (x4 - x3)*(y1-y3) - (y4-y3)*(x1-x3);
		var denominator = (y4 - y3)*(x2-x1) - (x4-x3)*(y2-y1);

		var ua = nominator/denominator;
		var xIntersect = x1 + ua*(x2-x1);
		var yIntersect = y1 + ua*(y2-y1);

		return new Vector2(xIntersect,yIntersect); 
	}


    // First we detect line segment intersections using the Bentley-Ottmann algorithm
    public void detectIntersectionPoints()
    {

    }

    // Removing line segment intersections

}
