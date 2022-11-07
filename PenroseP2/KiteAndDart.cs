using Godot;
using System;
using System.Collections.Generic;

public static class KiteAndDart 
{


	private const float scale = 500;

	public static List<Vector2> getKiteVertices()
	{
		const double angleRad = 36 * Math.PI/180;
		List<Vector2> vertices = new List<Vector2>();
		
		vertices.Add(new Vector2(0,0));
		var x_pos = scale*(float)Math.Cos(angleRad); 
		var y_pos = scale*(float)Math.Sin(angleRad);
		vertices.Add(new Vector2(x_pos,y_pos));
		vertices.Add(new Vector2(scale*1,0));
		vertices.Add(new Vector2(x_pos,-y_pos));
		
		//shift to origin
		var xShift = vertices[2].x/2;

		List<Vector2> shiftedVertices = new List<Vector2>();
		for(int i=0;i<vertices.Count;i++)
		{
			var vec = new Vector2(vertices[i].x - xShift, vertices[i].y);
			shiftedVertices.Add(vec);
		}

		return shiftedVertices;
	}


	public static List<Vector2> getDartVertices()
	{
		const double angleRad = 36 * Math.PI/180;
		List<Vector2> vertices = new List<Vector2>();
		
		vertices.Add(new Vector2(0,0));
		var x_pos = scale*(float)Math.Cos(angleRad); 
		var y_pos = scale*(float)Math.Sin(angleRad);
		vertices.Add(new Vector2(x_pos,y_pos));
		vertices.Add(new Vector2(scale*2/(1+(float)Math.Sqrt(5)) , 0));
		vertices.Add(new Vector2(x_pos,-y_pos));
		
		//shift to origin
		var xShift = vertices[2].x/2;

		List<Vector2> shiftedVertices = new List<Vector2>();
		for(int i=0;i<vertices.Count;i++)
		{
			var vec = new Vector2(vertices[i].x - xShift, vertices[i].y);
			shiftedVertices.Add(vec);
		}

		return shiftedVertices;
	}



/*
	public static List<Vector2> getDartVertices()
	{
			
		List<Vector2> kiteVertices = getKiteVertices();
		List<Vector2> vertices = new List<Vector2>();

		// 2/(1 + (float)Math.Sqrt(5)) is 1/phi where phi is golden ratio
		vertices.Add(new Vector2(kiteVertices[2].x + scale*2/(1 + (float)Math.Sqrt(5)) , 0));

		// all other vertices are shared with kite
		vertices.Add( kiteVertices[1]);
		vertices.Add( kiteVertices[2]);
		vertices.Add( kiteVertices[3]);

		//shift to origin
		var xShift = (vertices[0].x/2) + kiteVertices[2].x;

		List<Vector2> shiftedVertices = new List<Vector2>();
		for(int i=0;i<vertices.Count;i++)
		{
			var vec = new Vector2(vertices[i].x - xShift, vertices[i].y);
			shiftedVertices.Add(vec);
		}
		//return shiftedVertices;
		return vertices;
	}

	*/



}
