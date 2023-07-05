using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleDropItem : MonoBehaviour, IDropItem
{
    [SerializeField] bool isPirate;

    public void CollectItem(Player player)
    {
        player.PlayerTurtle = new Turtle { isPirate = this.isPirate };
        Delete();
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
