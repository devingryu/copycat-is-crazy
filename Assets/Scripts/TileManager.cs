using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] Tilemap floor;
    [SerializeField] Tilemap waveMap;
    [SerializeField] TileBase waveTile;

    //===========================================================================
    Dictionary<Vector3Int, Cell> map;
    public static TileManager Instance { get; private set; }

    WaitForSeconds waveLiveTime = new WaitForSeconds(0.2f);

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            if (Instance != this)
                Destroy(gameObject);
        }

        map = new Dictionary<Vector3Int, Cell>();
        foreach(Vector3Int pos in floor.cellBounds.allPositionsWithin)
        {
            if (!floor.HasTile(pos))
                continue;

            Vector3 worldPos = floor.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);
            map[pos] = new Cell { worldPos = worldPos, cellPos = pos };
            map[pos].OnCellAttacked += TileManager_OnCellAttacked;
        }
    }

    private void TileManager_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        waveMap.SetTile(e.position, waveTile);
        StartCoroutine(WaitWaveLiveTime(e.position));
    }

    public Vector3Int WorldToCoordinate(Vector3 position)
    {
        return floor.WorldToCell(position);
    }

    
    public Cell WorldToCell(Vector3 position)
    {
        return map[floor.WorldToCell(position)];
    }

    public Cell CoordinateToCell(Vector3Int coordinate)
    {
        return map[coordinate];
    }

    public bool TryGetCell(Vector3Int position, out Cell cell)
    {
        return map.TryGetValue(position, out cell);
    }
    

    IEnumerator WaitWaveLiveTime(Vector3Int position)
    {
        yield return waveLiveTime;
        waveMap.SetTile(position, null);
    }
}
