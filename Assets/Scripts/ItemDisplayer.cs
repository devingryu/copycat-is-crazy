using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayer : MonoBehaviour
{
    [SerializeField] Sprite emptySprite;
    [SerializeField] Player player;
    [SerializeField] List<Transform> itemDisplayers;

    List<TextMeshProUGUI> itemAmountTexts;
    List<Image> itemImages;
    List<UsableItem> items;

    private void Awake()
    {
        items = new List<UsableItem>(3);
        itemImages = new List<Image>(3);
        itemAmountTexts = new List<TextMeshProUGUI>(3);

        player.OnItemUse += ItemDisplayer_OnItemUse;
        for (int i = 0; i < itemDisplayers.Count; ++i)
        {
            itemAmountTexts.Add(itemDisplayers[i].GetComponentInChildren<TextMeshProUGUI>());
            itemImages.Add(itemDisplayers[i].Find("item image").GetComponent<Image>());
        }
    }

    private void ItemDisplayer_OnItemUse(object sender, Player.OnItemUseEventArgs e)
    {
        //get sprite of usable item and current amount, then display them.
        int index = items.FindIndex(x => ReferenceEquals(e.itemType, x));
        if (index != -1)
        {
            //already in display list
            if(items[index].GetAmount() > 0)
            {
                itemAmountTexts[index].SetText("x " + items[index].GetAmount());
            }
            else
            {
                itemImages[index].sprite = emptySprite;
                itemAmountTexts[index].SetText("");
                items.RemoveAt(index);
            }
        }
        else
        {
            //not in display list -> Add to the displayer
            if(e.itemType.GetAmount() <= 0)
                return;

            index = items.Count;
            if (index >= itemDisplayers.Count)
            {
                Debug.Log("Displayer Index Out of Range");
            }

            items.Add(e.itemType);
            itemImages[index].sprite = e.itemType.GetSprite();
            itemAmountTexts[index].SetText("x " + items[index].GetAmount());
        }
    }
}
