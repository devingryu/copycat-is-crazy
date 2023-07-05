using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] Tilemap floor;
    [SerializeField] Tilemap waveMap;
    [SerializeField] TileBase waveTile;

    public Balloon balloonPrefab;

    //===========================================================================

    public Dictionary<Vector3Int, Cell> map;
    public static TileManager Instance { get; private set; }

    WaitForSeconds waveLiveTime = new WaitForSeconds(0.2f);

    private void Awake()
    {
        Instance = this;
        map = new Dictionary<Vector3Int, Cell>();
        foreach(Vector3Int pos in floor.cellBounds.allPositionsWithin)
        {
            if (!floor.HasTile(pos))
                continue;

            Vector3 worldPos = floor.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);
            Collider2D collider = Physics2D.OverlapPoint(worldPos);
            map[pos] = new Cell { worldPos = worldPos, cellPos = pos ,objectOnCell = (collider != null), IsAttacked = false };
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
    

    IEnumerator WaitWaveLiveTime(Vector3Int position)
    {
        yield return waveLiveTime;
        waveMap.SetTile(position, null);
    }
}
