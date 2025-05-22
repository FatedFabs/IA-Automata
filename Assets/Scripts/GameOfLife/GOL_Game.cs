using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GOL_Game : MonoBehaviour
{
    [SerializeField] private Tilemap currentTilemap;
    [SerializeField] private Tilemap nextTilemap;
    [SerializeField] private Tile cellTile;
    [SerializeField] private float updateTimer = 0.1f;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;
    private bool gameStart = false;

    private void Awake()
    {
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
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
        {
            currentTilemap.SetTile(_pos, cellTile);
            aliveCells.Add(_pos);
        }
        else
        {
            currentTilemap.SetTile(_pos, null);
            aliveCells.Remove(_pos);
        }
    }

    public void Play()
    {
        StartCoroutine(Simulate());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public void Reset()
    {
        Stop();
        ClearTilemap();
        aliveCells.Clear();
        cellsToCheck.Clear();
    }

    private IEnumerator Simulate()
    {
        while (enabled)
        {
            UpdateState();
            yield return new WaitForSeconds(updateTimer);
        }
    }

    private void UpdateState()
    {
        //Checks for cells in adjacent tiles
        cellsToCheck.Clear();

        foreach (var cell in aliveCells)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    cellsToCheck.Add(cell + new Vector3Int(x, y, 0));
                }
            }
        }

        //Set tiles to the next state
        foreach (var cell in cellsToCheck)
        {
            int neighbors = countNeighbors(cell);
            bool alive = isAlive(cell);

            if(!alive &&  neighbors == 3)
            {
                nextTilemap.SetTile(cell, cellTile);
                aliveCells.Add(cell);
            }
            else if(alive && neighbors < 2 || neighbors > 3)
            {
                nextTilemap.SetTile(cell, null);
                aliveCells.Remove(cell);
            }
            else
            {
                nextTilemap.SetTile(cell, currentTilemap.GetTile(cell));
            }
        }

        Tilemap temp = currentTilemap;
        currentTilemap = nextTilemap;
        nextTilemap = temp;
        nextTilemap.ClearAllTiles();
    }

    private int countNeighbors(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbor = cell + new Vector3Int(x, y, 0);

                if(x == 0 && y == 0)
                    continue;
                else if(isAlive(neighbor))
                    count++;
            }
        }

        return count;
    }

    private bool isAlive(Vector3Int cell)
    {
        return currentTilemap.HasTile(cell);
    }

    private void ClearTilemap()
    {
        currentTilemap.ClearAllTiles();
        nextTilemap.ClearAllTiles();
    }
}
