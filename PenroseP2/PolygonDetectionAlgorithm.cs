using Godot;
using System;
using System.Collections.Generic;

public class PolygonDetectionAlgorithm : Node2D
{
    List<List<Vector2>> lines = new List<List<Vector2>>();
    List<HankinLine> hankinLines = new List<HankinLine>();

    public void addLine(List<Vector2> line)
    {
        lines.Add(line);
    }

    public void addLines(List<List<Vector2>> _lines)
    {
        lines.AddRange(_lines);
    }

    public void addHankinLines(List<HankinLine> _lines)
    {
        hankinLines.AddRange(_lines);
    }

    //*******************************************************************************
    //*******************************************************************************
    // ALGO
    //*******************************************************************************
    //*******************************************************************************
    
    // 1. First we detect line segment intersections using the Bentley-Ottmann algorithm
    // 2. Removing line segment intersections
    public List<Vector2> detectIntersectionPoints(bool useHankin = true)
    {
        if(useHankin)
        {
            lines.Clear();
            foreach(var l in hankinLines)
            {
                lines.Add(l.toList());
            }
        }
        List<Vector2> intersections = new List<Vector2>();
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines.Count; j++)
            {
                if(i==j)continue;
                bool outlier = false;
                var intersection = PolygonDetectionAlgorithm.calcIntersection(lines[i], lines[j], ref outlier);
                if(!outlier)
                {
                    intersections.Add(intersection);
                }
            }
        }
        return intersections;
    }


    //*******************************************************************************
    //*******************************************************************************
    //*******************************************************************************
    //*******************************************************************************

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

		var x1 = l1[0].x;
		var y1 = l1[0].y;

		var x2 = l1[1].x;
		var y2 = l1[1].y;

		var x3 = l2[0].x;
		var y3 = l2[0].y;

		var x4 = l2[1].x;
		var y4 = l2[1].y;

		var nominatorUA = (x4 - x3)*(y1-y3) - (y4-y3)*(x1-x3);
		var denominatorUA = (y4 - y3)*(x2-x1) - (x4-x3)*(y2-y1);
		var ua = nominatorUA/denominatorUA;

        var nominatorUB = (x2 - x1)*(y1-y3) - (y2-y1)*(x1-x3);
		var denominatorUB = (y4 - y3)*(x2-x1) - (x4-x3)*(y2-y1);
		var ub = nominatorUB/denominatorUB;

        var xIntersect = x1 + ua * (x2 - x1);
        var yIntersect = y1 + ua * (y2 - y1);

        // check if intersection point lies outside original line segments
        if (ua <= 1.01 && ua >= -0.01 && ub <= 1.01 && ub >= -0.01)
        {
            isFilterSegmentOutlier = false;
        }
        else
        {
            isFilterSegmentOutlier = true;
        }
        return new Vector2(xIntersect, yIntersect);




    }

    public override void _Process(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
        var intersections = detectIntersectionPoints();
        foreach(var p in intersections)
        {
            DrawCircle(p,5, Colors.Red);
        }
    }

}
