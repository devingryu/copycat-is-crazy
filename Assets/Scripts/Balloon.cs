using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Balloon : MonoBehaviour
{
    public event EventHandler OnBallonExplode;
    [SerializeField] float timerMax = 5f;

    [HideInInspector] public int range = 1;
    float timer;
    bool isBoomed = false;
    Vector3Int position;
    LayerMask wallMask;

    private void Awake()
    {
        timer = timerMax;
    }

    private void Start()
    {
        wallMask = LayerMask.GetMask("Wall");
        position = TileManager.Instance.WorldToCoordinate(transform.position);
        transform.position = TileManager.Instance.map[position].worldPos;
        TileManager.Instance.map[position].objectOnCell = true;
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
        TileManager.Instance.map[position].objectOnCell = false;
        //Spawn Wave
        AttackValidCell();
        Destroy(gameObject);
    }

    private void AttackValidCell()
    {
        int up, down, left, right;

        RaycastHit2D hitUp = Physics2D.Raycast(
            (Vector2)transform.position + Vector2.up * 0.45f, Vector2.up, range, wallMask);
        RaycastHit2D hitDown = Physics2D.Raycast(
            (Vector2)transform.position + Vector2.down * 0.45f, Vector2.down, range, wallMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(
            (Vector2)transform.position + Vector2.left * 0.45f, Vector2.left, range, wallMask);
        RaycastHit2D hitRight = Physics2D.Raycast(
            (Vector2)transform.position + Vector2.right * 0.45f, Vector2.right, range, wallMask);

        if (hitUp.transform == null)
            up = range;
        else
            up = (int)hitUp.distance;

        if (hitDown.transform == null)
            down = range;
        else
            down = (int)hitDown.distance;

        if (hitLeft.transform == null)
            left = range;
        else
            left = (int)hitLeft.distance;

        if (hitRight.transform == null)
            right = range;
        else
            right = (int)hitRight.distance;

        for(int i = 0;i<=up;++i)
        {
            if(TileManager.Instance.map.TryGetValue(position + Vector3Int.up * i, out Cell cell))
                cell.IsAttacked = true;
        }

        for (int i = 0; i <= down; ++i)
        {
            if (TileManager.Instance.map.TryGetValue(position + Vector3Int.down * i, out Cell cell))
                cell.IsAttacked = true;
        }

        for (int i = 0; i <= left; ++i)
        {
            if (TileManager.Instance.map.TryGetValue(position + Vector3Int.left * i, out Cell cell))
                cell.IsAttacked = true;
        }

        for (int i = 0; i <= right; ++i)
        {
            if (TileManager.Instance.map.TryGetValue(position + Vector3Int.right * i, out Cell cell))
                cell.IsAttacked = true;
        }
    }

}
