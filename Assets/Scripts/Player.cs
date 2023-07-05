using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask mask; //player can detect this mask
    public int balloonNumberMax = 3;
    public int speedMax = 10;
    public int speed = 3;
    public int balloonRangeMax = 5;
    public int balloonRange = 1;
    [SerializeField] GameObject test_trap;
    [SerializeField] int shieldTime = 2;
    

    Dictionary<UsableItemDataSO, int> usableItemInventory;

    public int balloonNumber = 0;
    int healthAmount = 3;
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
    bool calcBySpeedMax = false;

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
            test_trap.SetActive(isTrapped);

            if (isTrapped)
            {
                StartCoroutine(Trap());
            }



        }
    }

    Turtle playerTurtle = null;
    public Turtle PlayerTurtle
    {
        get { return playerTurtle; }

        set
        {
            if(value.isPirate)
            {
                //ride pirate turtle
            }
            else
            {
                //ride normal turtle
            }
        }
    }
    Rigidbody2D playerRb;
    KeyCode up, down, left, right;
    KeyCode placeBalloon, item1, item2, item3;

    private void Awake()
    {
        test_trap.SetActive(false);
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
        usableItemInventory = new Dictionary<UsableItemDataSO, int>();
        foreach(UsableItemDataSO usableItemData in ItemCache.Instance.usableItemList.list)
        {
            usableItemInventory[usableItemData] = 1; //초기 사용 아이템 개수
        }

        coordinate = TileManager.Instance.WorldToCoordinate(transform.position);
        TileManager.Instance.WorldToCell(coordinate).OnCellAttacked += Player_OnCellAttacked;
    }

    private void Update()
    {
        if(Input.GetKeyDown(placeBalloon))
        {
            PlaceBalloon();
        }

        if(Input.GetKeyDown(item1) && isTrapped)
        {
            int amount = usableItemInventory[ItemCache.Instance.usableItemList.list[0]];
            if(amount > 0)
            {
                usableItemInventory[ItemCache.Instance.usableItemList.list[0]]--;
                IsTrapped = false;
            }
        }
        /*
        if(Input.GetKeyDown(item2) && !isShield)
        {
            int amount = usableItemInventory[ItemCache.Instance.usableItemList.list[1]];
            if(amount > 0)
            {
                usableItemInventory[ItemCache.Instance.usableItemList.list[1]]--;
                StartCoroutine(UseShield());
            }    
        }*/


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

        if(calcBySpeedMax)
        {
            playerRb.velocity = direction * speedMax;
        }
        else
        {
            playerRb.velocity = direction * speed;
        }

        FrontCheck(direction * 0.6f);
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

        if (front == null) //there are no obstacles in front of player
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
        else if(front.TryGetComponent<IDropItem>(out IDropItem dropItem))
        {
            dropItem.CollectItem(this);
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
        // has balloon and there doesn't exist any object
        if (balloonNumber > 0 && !TileManager.Instance.map[coordinate].objectOnCell)
        {
            Balloon balloon = Instantiate(TileManager.Instance.balloonPrefab, transform.position, Quaternion.identity);
            balloon.OnBallonExplode += OnBallonExplode;
            balloon.range = balloonRange;
            --balloonNumber;
        }
    }



    IEnumerator UseShield()
    {
        IsShield = true;
        WaitForSeconds wait = new WaitForSeconds(shieldTime);
        yield return wait;
        IsShield = false;
    }

    IEnumerator Trap()
    {
        float timer = 5f;
        int tempSpeed = speed;
        speed = 1;

        while(IsTrapped)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                Debug.Log("trap activated");
                Health--;
                break;
            }
            yield return null;
        }

        speed = tempSpeed;
    }
}