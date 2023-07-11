using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : UsableItem
{
    public override Sprite GetSprite()
    {
        return ItemCache.Instance.GetUsableItemData(UsableItemName.Needle).sprite;
    }

    protected override bool CanUse(Player player)
    {
        return player.IsTrapped;
    }

    protected override void Effect(Player player)
    {
        player.IsTrapped = false;
        Debug.Log(player.gameObject.name + " is Using Needle");
    }
}
