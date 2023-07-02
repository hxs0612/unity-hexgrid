using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Camera mainCarema;
    public Tilemap tilemap;
    /**
        0   海洋
        1   浮冰
        2   平原
        3   浅海
        4   雪原
        5   冻土
        6   沙漠
        7   草原
        8   平原丘陵
        9   雪原丘陵
        10  冻土丘陵
        11  沙漠丘陵
        12  草原丘陵
        13  绿洲
        14  沼泽
        15  森林
        16  雨林
        17  山脉
        18  冲积平原
    **/
    public TileBase[] tiles;

    public int width;
    public int height;

    public int seed;
    public bool useRandomSeed;

    public Factors factors;
    private GeneratorInterface generator;
    private int[,] mapData;

    public void GenerateMap() {
        GenerateMapData();
        DrawTileMap();
    }

    public void CleanTileMap() {
        tilemap.ClearAllTiles();
    }

    private void GenerateMapData() {
        generator = new GrowthGenerator();
        seed = useRandomSeed ? Time.time.GetHashCode() : seed;
        UnityEngine.Random.InitState(seed);
        mapData = generator.GenerateMapData(width, height, factors);
    }

    private void DrawTileMap() {
        CleanTileMap();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y), tiles[mapData[x, y]]);
            }
        }
        AdjustCamera();
    }

    private void AdjustCamera() {
        Vector3 center = tilemap.localBounds.center;
        Vector3 size = tilemap.localBounds.size;
        mainCarema.transform.position = new Vector3(center.x, center.y, -10);
        mainCarema.orthographicSize = (size.y - center.y) * (1 + (size.x / size.y - mainCarema.aspect) / 2);
        
    }
}
