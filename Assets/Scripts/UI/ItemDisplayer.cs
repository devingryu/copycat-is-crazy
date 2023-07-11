using System.Collections.Generic;
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
    SortedSet<int> emptyIndex;


    private void Awake()
    {
        items = new List<UsableItem>(3);
        itemImages = new List<Image>(3);
        itemAmountTexts = new List<TextMeshProUGUI>(3);
        emptyIndex = new SortedSet<int>();

        player.OnItemUse += ItemDisplayer_OnItemUse;
        for (int i = 0; i < itemDisplayers.Count; ++i)
        {
            itemAmountTexts.Add(itemDisplayers[i].GetComponentInChildren<TextMeshProUGUI>());
            itemImages.Add(itemDisplayers[i].Find("item image").GetComponent<Image>());
            emptyIndex.Add(i);
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
                emptyIndex.Add(index);
            }
        }
        else
        {
            //not in display list -> Add to the displayer
            if(e.itemType.GetAmount() <= 0)
                return;
            int i = 0;
            int value = -1;
            while(!emptyIndex.TryGetValue(i, out value))
            {
                i++;
            }

            if(value != -1)
            {
                items.Add(e.itemType);
                itemImages[i].sprite = e.itemType.GetSprite();
                itemAmountTexts[i].SetText("x " + items[i].GetAmount());
                emptyIndex.Remove(i);
            }
        }
    }
}
