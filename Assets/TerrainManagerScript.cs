using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagerScript : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;

    [SerializeField]
    private int gaussianPerTerrainCount = 9;

    [SerializeField]
    private int globalSeed = 42;

    [SerializeField]
    private float terrainSize;

    [SerializeField]
    private float gammaMinValue;

    [SerializeField]
    private float gammaMaxValue;

    // Use this for initialization
    void Start()
    {
    }

    void GenerateTerrain()
    {
        var resolution = terrain.terrainData.heightmapResolution;
        var heights = new float[resolution, resolution];


        Random.InitState(globalSeed);

        var centers = new Vector2[gaussianPerTerrainCount];
        var gamma = new float[gaussianPerTerrainCount];

        for (var g = 0; g < gaussianPerTerrainCount; g++)
        {
            centers[g] = new Vector2(Random.Range(0f, terrainSize), Random.Range(0f, terrainSize));
            gamma[g] = Random.Range(0f, terrainSize);
        }


        for (var i = 0; i < resolution; i++)
        {
            for (var j = 0; j < resolution; j++)
            {
                var height = 0f;

                for (var g = 0; g < gaussianPerTerrainCount; g++)
                {
                    height += Mathf.Exp(-gamma[g]
                                        * Vector2.SqrMagnitude(centers[g] / terrainSize - new Vector2(i, j) / resolution
                                        ));
                }

                heights[j, i] = height / gaussianPerTerrainCount;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateTerrain();
        }
    }
}