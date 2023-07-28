using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    TextMeshProUGUI tutorial;
    [TextArea(3, 8)] [SerializeField] string[] tutotialTexts;
    int index = 0;

    private void Awake()
    {
        tutorial = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Show();
    }

    public void Show()
    {
        tutorial.SetText(tutotialTexts[index]);
        index = (index + 1) % tutotialTexts.Length;
    }
}