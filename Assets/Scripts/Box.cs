using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] float itemDropPercentage = 0.2f; //0 ~ 1 --> 0% ~ 100%

    bool isMoving = false;
    Vector3Int position;
    public Vector3Int Position
    {
        get { return position; }
        set
        {
            if(position != value)
            {
                StartCoroutine(MoveRoutine(TileManager.Instance.CoordinateToCell(value).worldPos));
                position = value;
            }
        }
    }


    private void Start()
    {
        position = TileManager.Instance.WorldToCoordinate(transform.position);
        Cell standingCell = TileManager.Instance.WorldToCell(transform.position);
        standingCell.cellObject = CellObject.Breakable;
        standingCell.OnCellAttacked += Box_OnCellAttacked;
    }
    public void Push(Vector3Int direction)
    {
        if (isMoving) return;

        if(TileManager.Instance.TryGetCell(direction + Position, out Cell movingCell))
        {
            if(movingCell.cellObject == CellObject.Nothing)
            {
                Cell prevCell = TileManager.Instance.WorldToCell(Position);
                prevCell.cellObject = CellObject.Nothing;
                prevCell.OnCellAttacked -= Box_OnCellAttacked;

                movingCell.OnCellAttacked += Box_OnCellAttacked;
                movingCell.cellObject = CellObject.Breakable;
                Position += direction;
                isMoving = true;
            }
        }
    }

    private void Box_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        Delete();
    }

    private IEnumerator MoveRoutine(Vector3 position)
    {
        GameManager.Instance.PlaySound(GameManager.SFXName.PushBox);
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
        Cell currentCell = TileManager.Instance.CoordinateToCell(position);
        currentCell.cellObject = CellObject.Nothing;
        currentCell.OnCellAttacked -= Box_OnCellAttacked;
        if (Random.value < itemDropPercentage && GameManager.battleCount != 3)
            ItemCache.Instance.SpawnRandomDropItem(transform.position);
    }
}
