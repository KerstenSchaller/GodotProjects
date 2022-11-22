using Godot;
using System;
using System.Collections.Generic;

public static class VectorHelper
{
	public static bool compareVectors(Vector2 v1, Vector2 v2)
	{
		return (v1 - v2).Length() > 0.001;
	}
}

public static class LineHelper 
{
	

	public static Vector2 calcIntersection(Vector2 p1, float angle1, Vector2 p2, float angle2)
	{
		bool dummy = false;
		return calcIntersection(p1,angle1,p2,angle2, ref dummy);
	}
	public static Vector2 calcIntersection(Vector2 p1, float angle1, Vector2 p2, float angle2, ref bool isFilterSegmentOutlier)
	{
		List<Vector2> l1 = new List<Vector2>(){p1,new Vector2( p1.x + (float)Math.Cos(angle1), p1.y +  (float)Math.Sin(angle1))};
		List<Vector2> l2 = new List<Vector2>(){p2,new Vector2( p2.x + (float)Math.Cos(angle2), p2.y +  (float)Math.Sin(angle2))};
		return calcIntersection(l1,l2, ref isFilterSegmentOutlier);
	}

	public static Vector2 calcIntersection(List<Vector2> l1, List<Vector2> l2)
	{
		bool dummy = false;
		return calcIntersection(l1,l2, ref dummy);
	}

	public static Vector2 calcIntersection(List<Vector2> l1, List<Vector2> l2, ref bool isFilterSegmentOutlier)
	{
		//http://paulbourke.net/geometry/pointlineplane/

		double x1 = l1[0].x;
		double y1 = l1[0].y;

		double x2 = l1[1].x;
		double y2 = l1[1].y;

		double x3 = l2[0].x;
		double y3 = l2[0].y;

		double x4 = l2[1].x;
		double y4 = l2[1].y;

		double nominatorUA = (x4 - x3)*(y1-y3) - (y4-y3)*(x1-x3);
		double denominatorUA = (y4 - y3)*(x2-x1) - (x4-x3)*(y2-y1);
		double ua = nominatorUA/denominatorUA;

		double nominatorUB = (x2 - x1)*(y1-y3) - (y2-y1)*(x1-x3);
		double denominatorUB = (y4 - y3)*(x2-x1) - (x4-x3)*(y2-y1);
		double ub = nominatorUB/denominatorUB;

		double xIntersect = x1 + ua * (x2 - x1);
		double yIntersect = y1 + ua * (y2 - y1);

		// check if intersection point lies outside original line segments
		if (ua <= 1.01 && ua >= -0.01 && ub <= 1.01 && ub >= -0.01)
		//if (ua <= 1 && ua >= 0 && ub <= 1 && ub >= 0)
		{
			isFilterSegmentOutlier = false;
		}
		else
		{
			isFilterSegmentOutlier = true;
		}
		return new Vector2((float)xIntersect, (float)yIntersect);
	}


}
