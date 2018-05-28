using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagerScript : MonoBehaviour
{
	[SerializeField]
	private Terrain terrain;

	// Use this for initialization
	void Start ()
	{
	}

	void GenerateTerrain()
	{
		var resolution = terrain.terrainData.heightmapResolution;
		var heights = new float[resolution, resolution];

		for (var i = 0; i < resolution; i++)
		{
			for (var j = 0; j < resolution; j++)
			{
				heights[j, i] = Random.Range(0f, 1f);
			}
		}
		
		terrain.terrainData.SetHeights(0, 0, heights);
	}
	
	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GenerateTerrain();
		}
	}
}
