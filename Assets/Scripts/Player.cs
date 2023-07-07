using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask mask; //player can detect this mask

    //스탯 업그레이드 한계
    const int speedMaxUpgrade = 10;
    const int balloonNumberMaxUpgrade = 5;
    const int balloonRangeMaxUpgrade = 5;


    int healthAmount = 3;


    List<Stat> statList;
    // stat upgrade targets
    int balloonNumberMax = 3;
    float speed = 3;
    int balloonRange = 1;
    int balloonNumber = 0;

    //테스트용
    [SerializeField] GameObject testTrap;

    int Health
    {
        get { return healthAmount; }

        set
        {
            healthAmount = value;
            Debug.Log(gameObject.name + " 's health: " + healthAmount);
            IsTrapped = false;

            if(healthAmount <= 0)
            {
                //die
            }
        }
    }
    
    public enum State { Up, Down, Left, Right, Idle }
    State playerState = State.Idle;
    Collider2D front;
    Collider2D self;
    Vector3Int coordinate;

    [SerializeField] bool isFirstPlayer;
    bool isTrapped = false;
    bool isShield = false;

    bool IsShield
    {
        get { return isShield; }
        set
        {
            if (isShield != value)
            {
                isShield = value;
            }
        }
    }




    public bool IsTrapped
    {
        get { return isTrapped; }
        set
        {
            if (IsShield) return;
            isTrapped = value;
            testTrap.SetActive(isTrapped); //test

            if (isTrapped)
                StartCoroutine(Trap());
        }
    }


    Rigidbody2D playerRb;
    KeyCode up, down, left, right;
    KeyCode placeBalloon, item1, item2, item3;

    private void Awake()
    {
        statList = new List<Stat>();
        testTrap.SetActive(false);
        self = GetComponent<Collider2D>();
        balloonNumber = balloonNumberMax;
        playerRb = GetComponent<Rigidbody2D>();
        if (isFirstPlayer)
        {
            //1P 조작키
            up = KeyCode.R; down = KeyCode.F; left = KeyCode.D; right = KeyCode.G;
            placeBalloon = KeyCode.LeftShift; item1 = KeyCode.Alpha1; item2 = KeyCode.Alpha2; item3 = KeyCode.Alpha3;
        }
        else
        {
            //2P 조작키
            up = KeyCode.UpArrow; down = KeyCode.DownArrow; left = KeyCode.LeftArrow; right = KeyCode.RightArrow;
            placeBalloon = KeyCode.RightShift; item1 = KeyCode.Home; item2 = KeyCode.Insert; item3 = KeyCode.PageUp;
        }
    }

    private void Start()
    {
        coordinate = TileManager.Instance.WorldToCoordinate(transform.position);
        TileManager.Instance.WorldToCell(coordinate).OnCellAttacked += Player_OnCellAttacked;
    }

    private void Update()
    {
        if(Input.GetKeyDown(placeBalloon))
        {
            PlaceBalloon();
        }

        Vector2 direction = Vector2.zero;

        if (Input.GetKey(up))
        {
            playerState = State.Up;
            direction = Vector2.up;
        }

        if (Input.GetKey(down))
        {
            playerState = State.Down;
            direction = Vector2.down;
        }

        if (Input.GetKey(left))
        {
            playerState = State.Left;
            direction = Vector2.left;
        }

        if (Input.GetKey(right))
        {
            playerState = State.Right;
            direction = Vector2.right;
        }

        Vector3Int nextPos = TileManager.Instance.WorldToCoordinate(transform.position);
        if (coordinate != nextPos)
        {
            TileManager.Instance.WorldToCell(nextPos).OnCellAttacked += Player_OnCellAttacked;
            TileManager.Instance.WorldToCell(coordinate).OnCellAttacked -= Player_OnCellAttacked;
            coordinate = nextPos;
        }

        playerRb.velocity = direction * speed;

        FrontCheck(direction * 0.55f);
    }

    private void Player_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        IsTrapped = true;
    }

    private void OnBallonExplode(object sender, System.EventArgs e)
    {
        if (balloonNumber < balloonNumberMax && balloonNumber >= 0)
            ++balloonNumber;
    }

    private void FrontCheck(Vector2 direction)
    {
        self.enabled = false;
        front = Physics2D.OverlapPoint((Vector2)transform.position + direction, mask);
        self.enabled = true;

        if (front == null)
            return;


        if(front.TryGetComponent<Box>(out Box box))
        {
            switch (playerState)
            {
                case State.Up:
                    box.Push(Vector3Int.up);
                    break;
                case State.Down:
                    box.Push(Vector3Int.down);
                    break;
                case State.Left:
                    box.Push(Vector3Int.left);
                    break;
                case State.Right:
                    box.Push(Vector3Int.right);
                    break;
            }
        }
        else if(front.TryGetComponent<Balloon>(out Balloon balloon))
        {
            playerRb.velocity = Vector2.zero;
        }
        else if(front.TryGetComponent<DropItem>(out DropItem dropItem))
        {
            statList.Add(dropItem.stat);
            dropItem.Delete();
            RefreshStat();
        }
        else if(front.TryGetComponent<Player>(out Player otherPlayer))
        {
            if(otherPlayer.IsTrapped)
                otherPlayer.Health--;

            if (IsTrapped)
                Health--;
        }

    }

    private void PlaceBalloon()
    {
        // has balloon and there doesn't exist any cellObject
        if (balloonNumber > 0 && TileManager.Instance.CoordinateToCell(coordinate).cellObject == CellObject.Nothing)
        {
            Balloon balloon = Instantiate(TileManager.Instance.balloonPrefab, transform.position, Quaternion.identity);
            balloon.OnBallonExplode += OnBallonExplode;
            balloon.range = balloonRange;
            --balloonNumber;
        }
    }


    IEnumerator Trap()
    {
        float timer = 5f;
        speed = 0.5f;

        while(IsTrapped)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                Health--;
                break;
            }
            yield return null;
        }

        RefreshStat();
    }

    void RefreshStat()
    {
        InitStat();
        bool turtle = false;
        bool pirateTurtle = false;
        foreach (Stat stat in statList)
        {
            speed += stat.speed;
            balloonNumber += stat.balloonNumber;
            balloonNumberMax += stat.balloonNumber;
            balloonRange += stat.balloonRange;

            if (stat.turtle)
                turtle = true;
            if(stat.pirateTurtle)
                pirateTurtle = true;
        }

        if (turtle)
            speed *= 0.5f;
        if (pirateTurtle)
            speed = speedMaxUpgrade;
    }

    void InitStat()
    {
        speed = 3;
        balloonNumberMax = 3;
        balloonRange = 1;
        balloonNumber = 3;
    }
}