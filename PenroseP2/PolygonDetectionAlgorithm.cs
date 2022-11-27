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
	
	// 1. First we detect line segment intersections using the Bentley-Ottmann algorithm
	// 2. Removing line segment intersections
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

      
        //	findCycles();
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
		cycles.Clear();
		foreach(var intersection in intersections)
		{
			currentCycle = new List<Intersection>();
			foreach(var link in intersection.Links)
			{
				var i = getIntersectionWithPos(link);
				if(i != null)
				{
					recurseLinks(i, intersection);
				}
			}
		}
	}

	Intersection currentIntersection;
	List<List<Intersection>> cycles = new List<List<Intersection>>();
	List<Intersection> currentCycle;
	void recurseLinks(Intersection intersection, Intersection origin)
	{
		if(intersection == null){GD.Print("intersection == null");}

		if(currentCycle.Contains(intersection))
		{
			// cycle found
			cycles.Add(new List<Intersection>(currentCycle));
			return;
		}
		else
		{
			currentCycle.Add(intersection);
		}


		// recurse again
		foreach(var link in intersection.Links)
		{
			var intersectionLink = getIntersectionWithPos(link);
			if(!intersectionLink.Equals(origin))
			{
				// only recurse if link is not where we came from
				recurseLinks(intersectionLink, intersection);
			} 
		}

		currentCycle.Remove(intersection);

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
