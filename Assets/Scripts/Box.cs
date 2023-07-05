using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditorInternal;
using UnityEngine;

public class Box : MonoBehaviour
{
    bool isMoving = false;
    Vector3Int position;
    public Vector3Int Position
    {
        get { return position; }
        set
        {
            if(position != value)
            {
                StartCoroutine(MoveRoutine(TileManager.Instance.map[value].worldPos));
                position = value;
            }
        }
    }


    private void Start()
    {
        position = TileManager.Instance.WorldToCoordinate(transform.position);
        Cell standingCell = TileManager.Instance.WorldToCell(transform.position);
        standingCell.objectOnCell = true;
    }
    public void Push(Vector3Int direction)
    {
        if (isMoving) return;

        if(TileManager.Instance.map.TryGetValue(direction + Position, out Cell cell))
        {
            if(!cell.objectOnCell)
            {
                TileManager.Instance.WorldToCell(Position).objectOnCell = false;
                TileManager.Instance.map[Position + direction].objectOnCell = true;
                Position += direction;
                isMoving = true;
            }
        }
    }

    private IEnumerator MoveRoutine(Vector3 position)
    {
        while(Vector3.Distance(position, transform.position) > 0.01f)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, position, 0.1f);
        }

        transform.position = position;
        isMoving = false;
    }

    public void Delete()
    {
        Destroy(gameObject);
        TileManager.Instance.map[position].objectOnCell = false;
        ItemCache.Instance.SpawnRandomDropItem(transform.position);
    }
}
