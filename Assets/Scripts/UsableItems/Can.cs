using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : UsableItem
{
    public override Sprite GetSprite()
    {
        return ItemCache.Instance.GetUsableItemData(UsableItemName.Can).sprite;
    }
    protected override bool CanUse(Player player)
    {
        return player.HasTurtle();
    }

    protected override void Effect(Player player)
    {
        player.UpgradeTurtle();
    }
}
