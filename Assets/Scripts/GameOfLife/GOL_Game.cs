using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GOL_Game : MonoBehaviour
{
    public Tilemap currentTilemap;
    public Tilemap nextTilemap;
    public Tile cellTile;

    public GOL_Pattern pattern;

    public float updateTimer = 0.05f;

    void Start()
    {
        SetPattern(pattern);
    }

    private void SetPattern(GOL_Pattern _pattern)
    {
        ClearTilemap();

        Vector2Int center = pattern.GetCenter();

        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector2Int cell = pattern.cells[i] - center;
            currentTilemap.SetTile((Vector3Int)cell, cellTile);
        }
    }

    private void ClearTilemap()
    {
        currentTilemap.ClearAllTiles();
        nextTilemap.ClearAllTiles();
    }
}
