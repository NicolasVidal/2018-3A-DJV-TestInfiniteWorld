using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainManagerScript : MonoBehaviour
{
    [SerializeField]
    private Terrain[] terrains;

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

    [SerializeField]
    private Transform playerTransform;

    // Use this for initialization
    void Start()
    {
    }

    Vector2 IdToOffset(int id)
    {
        switch (id)
        {
            case 0 : return new Vector2(-1f, -1f);
            case 1 : return new Vector2(0f, -1f);
            case 2 : return new Vector2(1f, -1f);
            case 3 : return new Vector2(-1f, 0f);
            case 4 : return new Vector2(0f, 0f);
            case 5 : return new Vector2(1f, 0f);
            case 6 : return new Vector2(-1f, 1f);
            case 7 : return new Vector2(0f, 1f);
            case 8 : return new Vector2(1f, 1f);
            default: throw new Exception();
        }
    }

    void GenerateTerrain(float x, float z)
    {
        var centers = new Vector2[terrains.Length][];
        var gammas = new float[terrains.Length][];

        for (var id = 0; id < terrains.Length; id++)
        {
            var coordinates = IdToOffset(id) * terrainSize;

            Random.InitState(globalSeed + Mathf.RoundToInt(x + coordinates.x) + Mathf.RoundToInt((z + coordinates.y) * 20000));
            centers[id] = new Vector2[gaussianPerTerrainCount];
            gammas[id] = new float[gaussianPerTerrainCount];

            for (var g = 0; g < gaussianPerTerrainCount; g++)
            {
                centers[id][g] = new Vector2(Random.Range(0f, terrainSize), Random.Range(0f, terrainSize));
                gammas[id][g] = Random.Range(gammaMinValue, gammaMaxValue);
            }
        }
        
        for (var id = 0; id < terrains.Length; id++)
        {
            var resolution = terrains[id].terrainData.heightmapResolution;
            var heights = new float[resolution, resolution];
            
            var coordinates = IdToOffset(id) * terrainSize;

            for (var i = 0; i < resolution; i++)
            {
                for (var j = 0; j < resolution; j++)
                {
                    var height = 0f;
                    
                    for (var id2 = 0; id2 < terrains.Length; id2++)
                    {
                        var coordinates2 = IdToOffset(id2) * terrainSize;
                        
                        for (var g = 0; g < gaussianPerTerrainCount; g++)
                        {
                            var vertexCoordinate = new Vector2(i, j) / resolution * terrainSize;
                            vertexCoordinate = new Vector2(Mathf.Ceil(vertexCoordinate.x), Mathf.Round(vertexCoordinate.y));
                            
                            
                            height += Mathf.Exp(-gammas[id2][g]
                                                * Vector2.SqrMagnitude(
                                                    (centers[id2][g] + coordinates2) / terrainSize - 
                                                    (vertexCoordinate + coordinates) / terrainSize)
                                                );
                        }
                    }

                    heights[j, i] = height / gaussianPerTerrainCount / 9f;
                }
            }

            terrains[id].transform.position = new Vector3(x + coordinates.x, 0f, z + coordinates.y);
            terrains[id].terrainData.SetHeights(0, 0, heights);
        }
    }


    private int lastX = int.MinValue;
    private int lastZ = int.MinValue;
    
    // Update is called once per frame
    void Update()
    {
        var x = Mathf.FloorToInt(playerTransform.position.x / terrainSize);
        var z = Mathf.FloorToInt(playerTransform.position.z / terrainSize);
        
        if (lastX != x || lastZ != z)
        {
            lastX = x;
            lastZ = z;
            GenerateTerrain(x * terrainSize, z * terrainSize);
        }
    }
}