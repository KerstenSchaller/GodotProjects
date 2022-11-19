using Godot;
using System;
using System.Collections.Generic;


public class PatternPolygon : Node2D
{
	List<Vector2> vertices = new List<Vector2>();
	List<HankinLine> hankinLines = new List<HankinLine>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vertices.Add(new Vector2(0,0));
		vertices.Add(new Vector2(50,0));
		vertices.Add(new Vector2(50,50));
		vertices.Add(new Vector2(0,50));

		addHankinsLines();

	}

	float hankinsAngle = 50f;
	public float exAngle
	{
		get { return hankinsAngle; }
		set
		{
			hankinsAngle = value;
			for(int i=0;i<hankinLines.Count;i++)
			{
				if (i % 2 == 0)
				{
					hankinLines[i].Angle = value;

				}
				else
				{
					hankinLines[i].Angle = -value;
				}
			}

		}
	}

	void addHankinsLines()
	{
		GD.Print("addHankinsLines");
		for (int i = 0; i < vertices.Count - 1; i++)
		{
			var midX = (vertices[i].x + vertices[i + 1].x) / 2;
			var midY = (vertices[i].y + vertices[i + 1].y) / 2;
			var baseAngle = -(float)Math.PI/2 + (float)Math.Atan2(vertices[i].y - vertices[i + 1].y,vertices[i].x - vertices[i + 1].x);
			var hankinLine1 = new HankinLine();
			hankinLine1.init(new Vector2(midX, midY), hankinsAngle, baseAngle);
			hankinLines.Add(hankinLine1);
			var hankinLine2 = new HankinLine();
			hankinLine2.init(new Vector2(midX, midY), -hankinsAngle, baseAngle);
			hankinLines.Add(hankinLine2);
			this.AddChild(hankinLine1);
			this.AddChild(hankinLine2);
		}

		for(int i = 0; i<2; i++)
		{
			if(i==1)hankinsAngle = -1*hankinsAngle;
			var midX2 = (vertices[vertices.Count - 1].x + vertices[0].x) / 2;
			var midY2 = (vertices[vertices.Count - 1].y + vertices[0].y) / 2;
			var baseAngle2 = (float)Math.PI/2 + (float)Math.Atan2(vertices[0].y - vertices[vertices.Count - 1].y, vertices[0].x - vertices[vertices.Count - 1].x);
			var hankinLineX = new HankinLine();
			hankinLineX.init(new Vector2(midX2, midY2), hankinsAngle, baseAngle2);
			hankinLines.Add(hankinLineX);
			this.AddChild(hankinLineX);
		}

		//connect neighbours
		for(int i = 0; i < hankinLines.Count; i++ )
		{
			if(i==0)
			{
				hankinLines[i].addNeighbour(hankinLines[hankinLines.Count-1]);
				continue;
			}

			if(i%2 == 0)
			{

				hankinLines[i].addNeighbour(hankinLines[i-1]);
			}
			else
			{
				if( i+1 == hankinLines.Count)
				{
					hankinLines[i].addNeighbour(hankinLines[0]);
				}
				else
				{
					hankinLines[i].addNeighbour(hankinLines[i+1]);
				}
			}
		}

	}

	public void init(Vector2[] _vertices)
	{	
		vertices.Clear();
		vertices.AddRange( _vertices);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {s
//      
//  }

	public override void _Draw()
	{
		/*
		//DrawColoredPolygon(collisionPoly.Polygon, Colors.Aqua);
		for(int i = 0; i < vertices.Count-1; i++)
		{
			DrawLine(vertices[i],vertices[i+1], Colors.BurlyWood);
		}
		DrawLine(vertices[vertices.Count-1],vertices[0], Colors.BurlyWood);
		*/
	}
	
	
}
