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

	public Vector2 getLowestAngleLink( bool excludeVector, Vector2 exVec = new Vector2())
	{
		Dictionary<float, Vector2> angleLinkD = new Dictionary<float, Vector2>();
		float angle = 10f;
		Vector2 retval = new Vector2();
		foreach(var l in links)
		{
			if(excludeVector && VectorHelper.compareVectors(l,exVec))continue;
			var AB = (this.position - l);
			float tempAngle = VectorHelper.angleBetween(AB, exVec);
			if(tempAngle < angle)
			{
				angle = tempAngle;
				retval = l;
			}
		}

		return retval;
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

    public override int GetHashCode()
    {
        return base.GetHashCode();
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
	

	public void detectIntersectionPoints(bool useHankin = true)
	{
		intersections.Clear();
		List<List<Vector2>> localLines = new List<List<Vector2>>(); 
		if(useHankin)
		{
			foreach(var l in hankinLines)
			{
				localLines.Add(l.toList());
				var intersection = new Intersection(l.Point);

                // add enclosing poly to links of hankin line origins
                foreach (var el in enclosingPolygon)
                {

                    if (LineHelper.isPointOnLine(intersection.Position, el))
                    {
                        intersection.addLinks(el);
                    }

                }

                if (!intersections.Contains(intersection))
                {
                    intersections.Add(intersection);
                }
                else
                {
                    intersections[intersections.IndexOf(intersection)].addLinks(intersection.Links);
                }
            }

			List<Intersection> tempIntersections = new List<Intersection>();
            foreach (var el in enclosingPolygon)
            {
                foreach (var i in intersections)
                {
					if (LineHelper.isPointOnLine(i.Position, el))
                    {
						var i1 = new Intersection(el[0]);
						var i2 = new Intersection(el[1]);
						i1.addLink(i.Position);
						i2.addLink(i.Position);
						tempIntersections.Add(i1);
                        tempIntersections.Add(i2);

                    }
                }
            }


			foreach(var i in tempIntersections)
			{
				if (!intersections.Contains(i))
				{
					intersections.Add(i);
				}
				else
				{
					intersections[intersections.IndexOf(i)].addLinks(i.Links);
				}
			}
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

		GD.Print("Ended creating graph - Number of intersections: " + intersections.Count);
      
        findCycles();
    }

	public Intersection getIntersectionWithPos(Vector2 pos)
	{
		foreach(var ints in intersections)
		{
			if((ints.Position - pos).Length() < 0.0001 )return ints;
		}
		return null;
	}


	void findCycles()
	{
		GD.Print("Start finding cycles -> Recurse through all intersections");
		cycles.Clear();
		foreach(var intersection in intersections)
        {
            currentCycle = new List<Intersection>();
			foreach(var link in intersection.Links)
			{
				var i = getIntersectionWithPos(link);
				//currentCycle.Add(intersection);
				if(i != null)
				{
					recurseLinks(i, intersection);
				}
			}
        }

		GD.Print( cycles.Count + " Cycles found"); 
		GD.Print( "Clearing duplicates" );

		List<List<Intersection>> temp = new List<List<Intersection>>();
        for (int i = 0; i < cycles.Count; i++)
        {
			List<Intersection> tempCycle = new List<Intersection>();
            for (int j = 0; j < cycles[i].Count; j++)
            {
                if (i == j) continue;
                if ( !tempCycle.Contains(cycles[i][j]))
				{
                	tempCycle.Add(cycles[i][j]);
				}
            }
			//int c = cycles.Count-1;
			//var range = cycles[i].GetRange(1,c);
			temp.Add(tempCycle);
        }
		cycles = temp;



		findUniqueCycles();
		GD.Print( cycles.Count + " unique Cycles found"); 
		sortOutOverlappingCycles();
		GD.Print( cycles.Count + " Cycles found which doe not overlap"); 

		GD.Print( cycles.Count + " Cycles found which doe not overlap"); 
	}

	Intersection currentIntersection;
	List<List<Intersection>> cycles = new List<List<Intersection>>();
	List<Intersection> currentCycle;

    void recurseLinks(Intersection intersection, Intersection origin)
    {
		bool cycleFound = false;
		if( currentCycle.Count > 1 )
		{
            var c = currentCycle.GetRange(1, currentCycle.Count - 1);
            if (c.Contains(intersection))
            {
                if ( intersection == currentCycle[0])
                {
                    // cycle found
                    cycles.Add(new List<Intersection>(currentCycle));
                    return;
                }
				else
				{
					return;
				}
            }
		}

        currentCycle.Add(intersection);

        // recurse again
        foreach (var link in intersection.Links)
        {
            var intersectionLink = getIntersectionWithPos(link);
            if (!intersectionLink.Equals(origin))
            {
                // only recurse if link is not where we came from
                recurseLinks(intersectionLink, intersection);
            }
        }
		
        currentCycle.Remove(intersection);

    }


	private void findUniqueCycles()
	{
		List<List<Intersection>> temp = new List<List<Intersection>>();
		for(int i=0;i < cycles.Count; i++)
		{
			bool alreadyContained = false;
			for(int j=0;j < temp.Count;j++)
			{
				//if(i==j)continue;
				if(compareCycles(cycles[i], temp[j]))
				{
					alreadyContained = true;
				}
			}
			if(alreadyContained == false)temp.Add(cycles[i]);
		}
		cycles = temp;

	}

	private void sortOutOverlappingCycles()
	{
		List<List<Intersection>> temp = new List<List<Intersection>>();
		for(int i = 0; i<cycles.Count;i++)
        {
            bool isOverlapping = false;
            for (int j = 0; j < cycles.Count; j++)
            {
                if (i == j) continue;
                foreach (var p in cycles[j])
                {	
					var cycle = cycles[i];
					if(IsPointInPolygon(cycle, p))
					{
                    	isOverlapping = true;
					}
                }
            }
            if (isOverlapping == false)temp.Add(cycles[i]);
		}
		cycles = temp;

		//var x = Geometry.SegmentIntersectsSegment2d(new Vector2(0,0), new Vector2(2,0),new Vector2(1,0), new Vector2(3,0));
		//	var x = Geometry.SegmentIntersectsSegment2d(new Vector2(-1,0), new Vector2(1,0),new Vector2(0,-1), new Vector2(0,1));
	}

    private static bool IsPointInPolygon(List<Intersection> polygon, Intersection testPoint)
    {
		List<Vector2> temp = new List<Vector2>();
		foreach(var p in polygon)
		{
			temp.Add(p.Position);
		}
		var isPointInOrOnPolygon = Geometry.IsPointInPolygon( testPoint.Position, temp.ToArray());
		var pointIsOnLine = isPointInOrOnPolygon;
		if(isPointInOrOnPolygon)
		{
			for(int i=0;i<temp.Count; i++)
            {
                var p11 = temp[i];
                var p12 = i + 1 < temp.Count ? temp[i + 1] : temp[0];
				List<Vector2> line = new List<Vector2>(){p11,p12};
				var isOnLine = LineHelper.isPointOnLine(testPoint.Position, line, true);
				if(isOnLine)
				{
					pointIsOnLine = false;
				}
            }
		}
		return pointIsOnLine;

    }

    private bool doPolygonsOverlap(List<Intersection> polygon1, List<Intersection> polygon2)
    {
        var l1 = new List<Vector2>();
        var l2 = new List<Vector2>();
        bool doOverlap = false;
        for (int i = 0; i < polygon1.Count; i++)
        {
			var p11 = polygon1[i].Position;
			var p12 = (i + 1 < polygon1.Count) ? polygon1[i+1].Position : polygon1[0].Position;
            for (int j = 0; j < polygon2.Count; j++)
            {
				var p21 = polygon2[j].Position;
				var p22 = (j + 1 < polygon2.Count) ? polygon2[j+1].Position : polygon2[0].Position;
                if (Geometry.SegmentIntersectsSegment2d(p11,p12,p21,p22) != null)
                {
                    doOverlap = true;
                }
				if(p11 == p21 || p11 == p22 || p21 == p21 || p21 == p22)
				{
					doOverlap = false;
				}
            }


        }


        return doOverlap;
    }




	private bool compareCycles(List<Intersection> c1, List<Intersection> c2)
	{

		if(c1.Count != c2.Count)return false;
		foreach(var i in c1)
		{
			if(!c2.Contains(i))return false;
		}

		return true;
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

	int i = 0;
	int counter = 0 ; 
	public override void _Draw()
	{
		foreach(var p in intersections)
		{
			DrawCircle( p.Position ,5, Colors.GreenYellow);
		}

        if (false)// print found polygons
        {

            if (counter < 50)
            {
                List<Vector2> cycle = new List<Vector2>();
				if( i < cycles.Count)
				{
					foreach (var i in cycles[i])
					{
						cycle.Add(i.Position);
						DrawCircle(i.Position,7, Colors.Green);
					}

				}
				else
				{
					i = 0;
				}

                DrawPolygon(cycle.ToArray(), new List<Color> { Colors.DarkBlue }.ToArray());
                counter++;

            }
            else
            {
                counter = 0;
                i++;
            }
        }
    }

}
