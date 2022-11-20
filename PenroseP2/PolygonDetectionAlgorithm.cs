using Godot;
using System;
using System.Collections.Generic;

class DirectedGraph
{

}

class Intersection
{
	Vector2 position;
	List<Vector2> links = new List<Vector2>();

	public List<Vector2> Links{get {return links;}}
	public Vector2 Position{get {return position;}}

	public Intersection(float x, float y)
	{
		position = new Vector2( x, y);
	}

	public Intersection(Vector2 _pos)
	{
		position = _pos;
	}

	public void addLink(Vector2 link)
	{
		if(link != position && !links.Contains(link))
		{
			links.Add(link);
		}
	}

	public void addLinks(List<Vector2> _links)
	{
		foreach(var link in _links)
		{
			if(link != position && !links.Contains(link))
			{
				links.Add(link);
			}
		}
	}

	public override bool Equals(object obj)
	{
		var item = obj as Intersection;

		if (item == null)
		{
			return false;
		}

		return item.position == this.position;
	}


}

public class PolygonDetectionAlgorithm : Node2D
{
	List<List<Vector2>> lines = new List<List<Vector2>>();
	List<HankinLine> hankinLines = new List<HankinLine>();
	List<List<Vector2>> enclosingPolygon = new List<List<Vector2>>();

	List<Intersection> intersections = new List<Intersection>();

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

	public void addEnclosingPoly(List<List<Vector2>> poly)
	{
		//return
		enclosingPolygon = poly;
		lines.AddRange(poly);
	}

	//*******************************************************************************
	//*******************************************************************************
	// ALGO
	//*******************************************************************************
	//*******************************************************************************
	
	// 1. First we detect line segment intersections using the Bentley-Ottmann algorithm
	// 2. Removing line segment intersections
	public void detectIntersectionPoints(bool useHankin = true)
	{
		List<List<Vector2>> localLines = new List<List<Vector2>>(); 
		if(useHankin)
		{
			foreach(var l in hankinLines)
			{
				localLines.Add(l.toList());
			}
		}

		if(true)// if enclosing poly is used
		{
			localLines.AddRange(enclosingPolygon);
		}

		for (int i = 0; i < localLines.Count; i++)
		{
			for (int j = 0; j < localLines.Count; j++)
			{
				if(i==j)continue;
				bool outlier = false;
				var intersectionPoint = PolygonDetectionAlgorithm.calcIntersection(localLines[i], localLines[j], ref outlier);
				if (!outlier)
				{

					var intersection = new Intersection(intersectionPoint);
					
					if (localLines[i][0] != intersectionPoint)
					{
						intersection.addLink(localLines[i][0]);
					}
					if (localLines[i][1] != intersectionPoint)
					{
						intersection.addLink(localLines[i][1]);
					}
					if (localLines[j][0] != intersectionPoint)
					{
						intersection.addLink(localLines[j][0]);
					}
					if (localLines[j][1] != intersectionPoint)
					{
						intersection.addLink(localLines[j][1]);
					}
					if(!intersections.Contains(intersection))
					{
						intersections.Add(intersection);
					}
					else
					{
						intersections[intersections.IndexOf(intersection)].addLinks(intersection.Links);
					}


				}

			}
		}
		
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

	public void update()
	{
		intersections.Clear();
		detectIntersectionPoints();
		Update();
	}


	int cnt = 0;
	public override void _Process(float delta)
	{
		// workaround because it seems that somethings odd in the order of process and draw are called
		if(cnt == 100)
		{   
			this.update();
			cnt = 0;
		}
		else
		{
			cnt++;
		}
		Update();

	}

	public override void _Draw()
	{
		foreach(var p in intersections)
		{
			DrawCircle( p.Position ,5, Colors.Red);
		}
	}

}
