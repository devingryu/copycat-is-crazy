using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    Cell currentCell;

    private void Awake()
    {
        timer = timerMax;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        position = TileManager.Instance.WorldToCoordinate(transform.position);
        currentCell = TileManager.Instance.CoordinateToCell(position);
        transform.position = currentCell.worldPos;

        if(currentCell.cellObject.HasFlag(CellObject.Bush))
        {
            spriteRenderer.enabled = false;
        }

        currentCell.cellObject |= CellObject.Balloon;

        currentCell.OnCellAttacked += Balloon_OnCellAttacked;
    }

    private void Balloon_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        spriteRenderer.enabled = true;
        Explode();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Explode();
        }

        SpikeCheck();
    }

    public void Explode()
    {
        if (isBoomed)
            return;
        TileManager.Instance.CoordinateToCell(position).OnCellAttacked -= Balloon_OnCellAttacked;
        isBoomed = true;
        OnBallonExplode?.Invoke(this, EventArgs.Empty);
        TileManager.Instance.CoordinateToCell(position).cellObject &= (~CellObject.Balloon);

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
            {
                bool isWall = upCell.cellObject.HasFlag(CellObject.Wall);
                upCell.Attack();
                if (isWall) break;
            }
            else
                break;
        }

        for (down = 1; down <= range; ++down)
        {
            if (TileManager.Instance.TryGetCell(position + Vector3Int.down * down, out Cell downCell) && IsAttackable(downCell.cellObject))
            {
                bool isWall = downCell.cellObject.HasFlag(CellObject.Wall);
                downCell.Attack();
                if (isWall) break;
            }
            else
                break;
        }

        for (left = 1; left <= range; ++left)
        {
            if (TileManager.Instance.TryGetCell(position + Vector3Int.left * left, out Cell leftCell) && IsAttackable(leftCell.cellObject))
            {
                bool isWall = leftCell.cellObject.HasFlag(CellObject.Wall);
                leftCell.Attack();
                if (isWall) break;
            }

            else
                break;
        }

        for (right = 1; right <= range; ++right)
        {
            if (TileManager.Instance.TryGetCell(position + Vector3Int.right * right, out Cell rightCell) && IsAttackable(rightCell.cellObject))
            {
                bool isWall = rightCell.cellObject.HasFlag(CellObject.Wall);
                rightCell.Attack();
                if (isWall) break;
            }
            else
                break;
        }
    }

    private bool IsAttackable(CellObject obj)
    {
        return !obj.HasFlag(CellObject.Unbreakable);
    }

    public static bool CanPlaceBalloon(CellObject cellObject)
    {
        // filter : except spike and bush
        CellObject filter = ~(CellObject.Spike | CellObject.Bush);
        return (cellObject & filter) == 0;
    }

    void SpikeCheck()
    {
        if (currentCell.cellObject.HasFlag(CellObject.Spike) && currentCell.spike.IsOn())
        {
            Explode();
        }
    }
}
