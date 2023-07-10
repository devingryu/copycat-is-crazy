using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform shieldTimer;
    [SerializeField] LayerMask mask; //player can detect this mask
    List<UsableItem> usableItemInventory; //usable item contains usable item amount


    const int speedMaxUpgrade = 8;
    const int balloonNumberMaxUpgrade = 5;
    const int balloonRangeMaxUpgrade = 5;
    int healthAmount = 3;
    List<Stat> statList;


    int balloonNumberMax = 3;
    float speed = 3;
    int balloonRange = 1;
    int balloonNumber = 0;
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
                GameManager.Instance.SetWinner(isFirstPlayer);
                GameManager.Instance.EndGame();
            }
        }
    }
    
    public enum State { Up, Down, Left, Right, }
    public enum Turtle { None, Normal, Pirate}
    Turtle turtle = Turtle.None;
    State playerState = State.Down;
    Animator playerAnim;
    Collider2D front;
    Collider2D self;
    SpriteRenderer playerRenderer;
    Vector3Int coordinate;

    [SerializeField] bool isFirstPlayer;
    bool isTrapped = false;
    bool isShield = false;

    public bool IsShield
    {
        get { return isShield; }
        set
        {
            if(value)
            {
                isShield = true;
                shieldTimer.gameObject.SetActive(true);
            }
            else
            {
                isShield = false;
            }
        }
    }

    public bool IsTrapped
    {
        get { return isTrapped; }
        set
        {
            if(value)
            {
                if (IsShield)
                {
                    Debug.Log(gameObject.name + " blocked attack by shield!");
                    return;
                }

                if (turtle != Turtle.None)
                {
                    Debug.Log("Turtle lost");
                    TurtleLose();
                    RefreshStat();
                    return;
                }
                isTrapped = true;
                playerAnim.SetBool("Trap", true);
                StartCoroutine(TrapTimer());
            }
            else
            {
                isTrapped = false;
                playerAnim.SetBool("Trap", false);
            }
        }
    }


    Rigidbody2D playerRb;
    KeyCode up, down, left, right;
    KeyCode placeBalloon, item1, item2, item3;

    private void Awake()
    {
        ItemInventoryInit();
        playerAnim = GetComponent<Animator>();
        statList = new List<Stat>();
        playerRenderer = GetComponent<SpriteRenderer>();
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
        Vector2 direction = Vector2.zero;
        bool isMoving = false;
        if(Input.GetKeyDown(placeBalloon))
            PlaceBalloon();
        if (Input.GetKeyDown(item1))
            usableItemInventory[0].Use(this);
        if (Input.GetKeyDown(item2))
            usableItemInventory[1].Use(this);
        if (Input.GetKeyDown(item3))
            usableItemInventory[2].Use(this);

        if (Input.GetKey(up))
        {
            playerState = State.Up;
            direction = Vector2.up;
            isMoving = true;
        }
        if (Input.GetKey(down))
        {
            playerState = State.Down;
            direction = Vector2.down;
            isMoving = true;
        }
        if (Input.GetKey(left))
        {
            playerState = State.Left;
            direction = Vector2.left;
            isMoving = true;
        }
        if (Input.GetKey(right))
        {
            playerState = State.Right;
            direction = Vector2.right;
            isMoving = true;
        }

        Vector3Int nextPos = TileManager.Instance.WorldToCoordinate(transform.position);
        if (coordinate != nextPos)
        {
            playerRenderer.enabled = TileManager.Instance.WorldToCell(nextPos).cellObject != CellObject.Bush;
            TileManager.Instance.WorldToCell(nextPos).OnCellAttacked += Player_OnCellAttacked;
            TileManager.Instance.WorldToCell(coordinate).OnCellAttacked -= Player_OnCellAttacked;
            coordinate = nextPos;
        }

        playerAnim.SetBool("isMoving", isMoving);
        if(isMoving)
        {
            playerAnim.SetFloat("dirX", direction.x);
            playerAnim.SetFloat("dirY", direction.y);
        }

        playerRb.velocity = direction * speed;
        FrontCheck(direction * 0.51f);
    }

    private void Player_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        IsTrapped = true;
        if(IsShield)
            transform.GetComponentInChildren<ShieldTimer>().timer -= 1;
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
        // has balloon and there doesn't exist any cellObject (except bush)
        if (balloonNumber > 0)
        {
            CellObject cellobj = TileManager.Instance.CoordinateToCell(coordinate).cellObject;
            if(cellobj == CellObject.Nothing || cellobj == CellObject.Bush)
            {
                Balloon balloon = Instantiate(ItemCache.Instance.balloonPrefab, transform.position, Quaternion.identity);
                balloon.OnBallonExplode += OnBallonExplode;
                balloon.range = balloonRange;
                --balloonNumber;
            }
        }
    }


    IEnumerator TrapTimer()
    {
        float timer = 5f;
        speed = 0.1f;

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
        {
            speed *= 0.5f;
            this.turtle = Turtle.Normal;
        }

        if (pirateTurtle)
        {
            speed = speedMaxUpgrade;
            this.turtle = Turtle.Pirate;
        }

    }

    void InitStat()
    {
        speed = 3;
        balloonNumberMax = 3;
        balloonRange = 1;
        balloonNumber = 3;
        turtle = Turtle.None;
    }

    void TurtleLose()
    {
        for(int i = statList.Count - 1;i >= 0;--i)
        {
            if (statList[i].turtle)
                statList.RemoveAt(i);
        }
    }

    public void UpgradeTurtle()
    {
        if (HasTurtle())
        {
            TurtleLose();

            Debug.Log("Upgraded Your Turtle");
            statList.Add(new Stat { turtle = true, pirateTurtle = true , 
                balloonNumber = 0, balloonRange = 0, speed = 0});
            RefreshStat();
        }
    }

    public bool HasTurtle()
    {
        return turtle != Turtle.None;
    }

    void ItemInventoryInit()
    {
        Needle needle = new Needle();
        Shield shield = new Shield();
        Can can = new Can();
        usableItemInventory = new List<UsableItem> { needle, shield, can };
        //set init amount
        needle.SetAmount(2); shield.SetAmount(2); can.SetAmount(2);
    }
}