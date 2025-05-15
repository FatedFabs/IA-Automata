using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GOL_Game : MonoBehaviour
{
    [SerializeField] private Tilemap currentTilemap;
    [SerializeField] private Tilemap nextTilemap;
    [SerializeField] private Tile cellTile;

    [SerializeField] private GOL_Pattern pattern;

    [SerializeField] private float updateTimer = 0.05f;

    private bool gameStart = false;

    void Start()
    {
        //SetInitialPattern(pattern);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameStart)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mousePosInt = new Vector3Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y), 0);

            PaintCell(mousePosInt);
        }
    }

    private void PaintCell(Vector3Int _pos)
    {
        if (!currentTilemap.HasTile(_pos))
            currentTilemap.SetTile(_pos, cellTile);
        else
            currentTilemap.SetTile(_pos, null);
    }

    public void Play()
    {
        gameStart = true;

        nextTilemap = currentTilemap;
        currentTilemap.ClearAllTiles();
    }

    private void SetInitialPattern(GOL_Pattern _pattern)
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
