using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemDisplayer : MonoBehaviour
{
    [SerializeField] Transform displayerTemplate;
    const float offsetX = -150f;
    const float offsetY = 40;
    private void Start()
    {
        displayerTemplate.gameObject.SetActive(false);

        for(int i = 0;i<ItemCache.Instance.usableItemList.list.Count;++i)
        {
            UsableItemDataSO data = ItemCache.Instance.usableItemList.list[i];
            if (data != null)
            {
                Transform displayer = Instantiate(displayerTemplate, transform);
                displayer.gameObject.SetActive(true);
                displayer.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetX * i, offsetY);
                displayer.Find("image").GetComponent<Image>().sprite = data.sprite;

            }
        }
    }
}
