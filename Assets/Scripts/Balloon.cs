using System;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public event EventHandler OnBallonExplode;
    [SerializeField] float timerMax = 5f;
    [HideInInspector] public int range = 1;
    float timer;
    bool isBoomed = false;
    Vector3Int position;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        timer = timerMax;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        position = TileManager.Instance.WorldToCoordinate(transform.position);
        Cell currentCell = TileManager.Instance.CoordinateToCell(position);
        transform.position = currentCell.worldPos;

        if(currentCell.cellObject == CellObject.Bush)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            currentCell.cellObject = CellObject.Balloon;
        }

        currentCell.OnCellAttacked += Balloon_OnCellAttacked;
    }

    private void Balloon_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        spriteRenderer.enabled = true;
        Explode();
        TileManager.Instance.CoordinateToCell(position).OnCellAttacked -= Balloon_OnCellAttacked;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (isBoomed)
            return;

        isBoomed = true;
        OnBallonExplode?.Invoke(this, EventArgs.Empty);
        TileManager.Instance.CoordinateToCell(position).cellObject = CellObject.Nothing;

        AttackValidCell();
        GameManager.Instance.PlaySound(GameManager.SFXName.Explode);
        Destroy(gameObject);
    }

    private void AttackValidCell()
    {
        int up, down, left, right;
        for (up = 0; up <= range; ++up)
        {
            if (TileManager.Instance.TryGetCell(position + Vector3Int.up * up, out Cell upCell) && IsAttackable(upCell.cellObject))
                upCell.Attack();
            else
                break;
        }

        for (down = 1; down <= range; ++down)
        {
            if (TileManager.Instance.TryGetCell(position + Vector3Int.down * down, out Cell downCell) && IsAttackable(downCell.cellObject))
                downCell.Attack();
            else
                break;
        }

        for (left = 1; left <= range; ++left)
        {
            if (TileManager.Instance.TryGetCell(position + Vector3Int.left * left, out Cell leftCell) && IsAttackable(leftCell.cellObject))
                    leftCell.Attack();
            else
                break;
        }

        for (right = 1; right <= range; ++right)
        {
            if (TileManager.Instance.TryGetCell(position + Vector3Int.right * right, out Cell rightCell)
                && IsAttackable(rightCell.cellObject))
                rightCell.Attack();
            else
                break;
        }
    }

    private bool IsAttackable(CellObject obj)
    {
        return obj != CellObject.UnBreakable;
    }
}
