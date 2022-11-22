using Godot;
using System;
using System.Collections.Generic;

public class Intersection
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
				var intersectionPoint = LineHelper.calcIntersection(localLines[i], localLines[j], ref outlier);
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

	public Intersection getIntersectionWithPos(Vector2 pos)
	{
		foreach(var ints in intersections)
		{
			if(ints.Position == pos)return ints;
			return null;
		}
		return null;
	}


	//*******************************************************************************
	//*******************************************************************************
	//*******************************************************************************
	//*******************************************************************************

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
