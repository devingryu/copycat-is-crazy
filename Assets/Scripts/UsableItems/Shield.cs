using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Shield : UsableItem
{
    public override Sprite GetSprite()
    {
        return ItemCache.Instance.GetUsableItemData(UsableItemName.Shield).sprite;
    }
    protected override bool CanUse(Player player)
    {
        return !player.IsShield && !player.IsTrapped;
    }

    protected override void Effect(Player player)
    {
        player.IsShield = true;
    }
}
