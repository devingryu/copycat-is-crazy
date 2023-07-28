using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;

public class ResolutionSelect : MonoBehaviour
{
    [SerializeField] TMP_Dropdown selectDropDown;
    [SerializeField] Button okButton;
    List<Resolution> resolutions = new List<Resolution>();
    int index = -1;

    private void Awake()
    {
        resolutions.AddRange(Screen.resolutions);
        selectDropDown.options.Clear();
        int i = 0;
        foreach(var resolution in resolutions)
        {
            OptionData resolutionOptionData = new OptionData() { text = resolution.ToString() };
            selectDropDown.options.Add(resolutionOptionData);

            if(Screen.width == resolution.width && Screen.height == resolution.height)
            {
                selectDropDown.value = i;
            }

            ++i;
        }
    }

    private void Start()
    {
        selectDropDown.RefreshShownValue();
        selectDropDown.onValueChanged.AddListener(x => SetIndex(x));
        okButton.onClick.AddListener(ChangeResolution);
    }

    void SetIndex(int index)
    {
        this.index = index;
    }

    void ChangeResolution()
    {
        Debug.Log("Changed Resolution");
        Screen.SetResolution(resolutions[selectDropDown.value].width, resolutions[selectDropDown.value].height, true);
    }
}
