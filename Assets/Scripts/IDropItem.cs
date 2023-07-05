using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDropItem
{
    public void CollectItem(Player player);
    public void Delete();
}
