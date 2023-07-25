using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] SpriteRenderer shieldTimer;
    [SerializeField] GameObject displayer;
    [SerializeField] Animator vehicle;
    [SerializeField] LayerMask mask; //player can detect this mask
    List<UsableItem> usableItemInventory; //usable item contains usable item amount
    Vector3 originPos;

    const int speedMaxUpgrade = 7;
    const int balloonNumberMaxUpgrade = 5;
    const int balloonRangeMaxUpgrade = 5;
    int healthAmount = 3;
    public static int FirKill = 0;
    public static int SecKill = 0;
    List<Stat> statList;

    public event EventHandler<OnItemUseEventArgs> OnItemUse;
    public event EventHandler OnDamage;

    public class OnItemUseEventArgs : EventArgs
    {
        public UsableItem itemType;
    }

    int balloonNumberMax = 1;
    float speed = 3.5f;
    int balloonRange = 1;
    int balloonNumber = 0;
    public static bool FirstPlayerDefeat = false;
    public static bool SecondPlayerDefeat = false;
    FirKillCount FirKillCount;
    SecKillCount SecKillCount;


    public int Health
    {
        get { return healthAmount; }

        set
        {
            if(healthAmount > value)
            {
                GameManager.Instance.PlaySound(GameManager.SFXName.Damaged);
                transform.position = originPos;
                isShield = true;
                StartCoroutine(GracePeriod()); // Turn transparent shield off after 0.5s
            }

            healthAmount = value;
            OnDamage?.Invoke(this, EventArgs.Empty);
            IsTrapped = false;

            if(GameManager.battleCount != 4) //타임어택 맵 제외
            {
            
                if(healthAmount <= 0)
                {
                    if (isFirstPlayer)
                    {
                        FirstPlayerDefeat = true;
                    }
                    else
                    {
                        SecondPlayerDefeat = true;
                    }
                    //die
                
                    GameManager.Instance.EndGame();
                }
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
                    return;

                if (turtle != Turtle.None)
                {
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
        originPos = transform.position;
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
            placeBalloon = KeyCode.RightShift; item1 = KeyCode.Insert; item2 = KeyCode.Home; item3 = KeyCode.PageUp;
        }
    }

    private void Start()
    {
        ItemInventoryInit();
        coordinate = TileManager.Instance.WorldToCoordinate(transform.position);
        TileManager.Instance.WorldToCell(coordinate).OnCellAttacked += Player_OnCellAttacked;
        
        
    }

    private void Update()
    {
        Move(out Vector2 direction);

        if(Input.GetKeyDown(placeBalloon))
            PlaceBalloon();
        if (Input.GetKeyDown(item1) && GameManager.battleCount != 3)
        {
            usableItemInventory[0].Use(this);
            OnItemUse?.Invoke(this, new OnItemUseEventArgs { itemType = usableItemInventory[0] });
        }
        if (Input.GetKeyDown(item2) && GameManager.battleCount != 3)
        {
            usableItemInventory[1].Use(this);
            OnItemUse?.Invoke(this, new OnItemUseEventArgs { itemType = usableItemInventory[1] });
        }
        if (Input.GetKeyDown(item3) && GameManager.battleCount != 3)
        {
            usableItemInventory[2].Use(this);
            OnItemUse?.Invoke(this, new OnItemUseEventArgs { itemType = usableItemInventory[2] });
        }

        Vector3Int nextPos = TileManager.Instance.WorldToCoordinate(transform.position);
        if (coordinate != nextPos)
        {
            BushCheck(nextPos);
            TileManager.Instance.CoordinateToCell(nextPos).OnCellAttacked += Player_OnCellAttacked;
            TileManager.Instance.CoordinateToCell(coordinate).OnCellAttacked -= Player_OnCellAttacked;
            coordinate = nextPos;
        }

        FrontCheck(direction);

    }


    private void BushCheck(Vector3Int nextPos)
    {
        bool notInBush = TileManager.Instance.WorldToCell(nextPos).cellObject != CellObject.Bush;
        playerRenderer.enabled = notInBush;
        displayer.SetActive(notInBush);
        shieldTimer.enabled = notInBush;
        vehicle.GetComponent<SpriteRenderer>().enabled = notInBush;
    }

    private void Move(out Vector2 direction)
    {
        direction = Vector2.zero;
        bool isMoving = false;
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

        playerAnim.SetBool("isMoving", isMoving);
        if (vehicle.runtimeAnimatorController != null)
            vehicle.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            playerAnim.SetFloat("dirX", direction.x);
            playerAnim.SetFloat("dirY", direction.y);
            if(vehicle.runtimeAnimatorController != null)
            {
                vehicle.SetFloat("dirX", direction.x);
                vehicle.SetFloat("dirY", direction.y);
            }
        }

        playerRb.velocity = direction * speed;
    }

    private void Player_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        IsTrapped = true;
        if (IsShield)
            transform.GetComponentInChildren<ShieldTimer>().timer -= 1;

        BushCheck(TileManager.Instance.WorldToCoordinate(transform.position));
    }

    private void OnBallonExplode(object sender, System.EventArgs e)
    {
        if (balloonNumber < balloonNumberMax && balloonNumber >= 0)
            ++balloonNumber;
    }

    private void FrontCheck(Vector2 direction)
    {
        if (TileManager.Instance.TryGetCell(transform.position + (Vector3)direction * 0.55f, out Cell frontCell))
        {
            if(frontCell.cellObject == CellObject.Balloon &&
                TileManager.Instance.WorldToCell(transform.position).cellObject != CellObject.Balloon)
            {
                playerRb.velocity = Vector2.zero;
                Debug.Log("cannot move");
            }

            self.enabled = false;
            front = Physics2D.OverlapPoint((Vector2)transform.position + direction * 0.52f, mask);
            self.enabled = true;

            if (front == null)
                return;

            if (front.TryGetComponent<Box>(out Box box))
            {
                if (isTrapped) return;
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
            else if (front.TryGetComponent<DropItem>(out DropItem dropItem))
            {
                if (isTrapped) return;
                GameManager.Instance.PlaySound(GameManager.SFXName.CollectItem);
                statList.Add(dropItem.stat);
                dropItem.Delete();
                RefreshStat();
            }
            else if (front.TryGetComponent<Player>(out Player otherPlayer))
            {
                if (isTrapped) return;

                if (otherPlayer.IsTrapped)
                {
                    otherPlayer.Health--;
                    if(GameManager.battleCount == 4) { 
                        if (isFirstPlayer)
                        {
                            FirKill++;
                            FirKillCount.IncreaseKillCount();
                        }
                        else
                        {
                            SecKill++;
                            SecKillCount.IncreaseKillCount();
                        }
                    }
                }
                    
                
            }
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
                GameManager.Instance.PlaySound(GameManager.SFXName.PlaceBalloon);
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
                if (GameManager.battleCount == 4)
                {
                    if (isFirstPlayer)
                    {
                        SecKill++;
                        SecKillCount.IncreaseKillCount();
                    }
                        
                    else { 
                        FirKill++;
                        FirKillCount.IncreaseKillCount();
                    }
                }
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

        balloonRange = Mathf.Clamp(balloonRange, 1, balloonRangeMaxUpgrade);
        balloonNumberMax = Mathf.Clamp(balloonNumberMax, 1, balloonNumberMaxUpgrade);
        balloonNumber = Mathf.Clamp(balloonNumber, 1, balloonNumberMaxUpgrade);
        if (turtle)
        {
            speed *= 0.5f;
            this.turtle = Turtle.Normal;
            playerAnim.SetBool("isRiding", true);
            GameManager.Instance.PlaySound(GameManager.SFXName.RideSomething);
            vehicle.runtimeAnimatorController = ItemCache.Instance.GetVehicleController(VehicleName.Turtle); 
        }

        if (pirateTurtle)
        {
            speed = speedMaxUpgrade;
            this.turtle = Turtle.Pirate;
            playerAnim.SetBool("isRiding", true);
            GameManager.Instance.PlaySound(GameManager.SFXName.RideSomething);
            vehicle.runtimeAnimatorController = ItemCache.Instance.GetVehicleController(VehicleName.Pirate);
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

        playerAnim.SetBool("isRiding", false);
        vehicle.runtimeAnimatorController = null;
    }

    public void UpgradeTurtle()
    {
        if (HasTurtle())
        {
            TurtleLose();
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
        needle.SetAmount(1);
        OnItemUse?.Invoke(this, new OnItemUseEventArgs { itemType = needle });
        shield.SetAmount(2);
        OnItemUse?.Invoke(this, new OnItemUseEventArgs { itemType = shield });
        can.SetAmount(1);
        OnItemUse?.Invoke(this, new OnItemUseEventArgs { itemType = can });
    }

    IEnumerator GracePeriod()
    {
        float timer = 2;
        WaitForSeconds blinkTime = new WaitForSeconds(0.2f);
        while(timer >= 0)
        {
            Debug.Log(timer);
            timer -= 0.4f;
            playerRenderer.color = Color.clear;
            yield return blinkTime;
            playerRenderer.color = Color.white;
            yield return blinkTime;
        }
        isShield = false;
    }
}