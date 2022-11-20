using Godot;
using System;
using System.Collections.Generic;

public class SquarePatternTile : Node2D
{
	
	PatternPolygon patternPoly = new PatternPolygon();
	
	public float Offset{set{ patternPoly.Offset =  value;}}
	public float Angle{set{ patternPoly.Angle =  value;}}

	public void init(float size)
	{
		List<Vector2> vertices = new List<Vector2>();
		vertices.Add(new Vector2(0,0));
		vertices.Add(new Vector2(size,0));
		vertices.Add(new Vector2(size,size));
		vertices.Add(new Vector2(0,size));
		patternPoly.init(vertices.ToArray());
		AddChild(patternPoly);
	}
}
