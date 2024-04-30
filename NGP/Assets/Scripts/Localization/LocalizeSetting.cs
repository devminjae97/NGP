using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeSetting : MonoBehaviour
{
    Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        if (dropdown.options.Count != LanguageManager.Instance.Languages.Count)
        {
            SetLangOption();
        }

        dropdown.onValueChanged.AddListener((d) => LanguageManager.Instance.SetLanguageIndex(dropdown.value));

        LocalizeSettingChanged();
        LanguageManager.Instance.LocalizeSettingChanged += LocalizeSettingChanged;
    }

    /*
    void OnDestroy()
    {
        LanguageManager.Instance.LocalizeSettingChanged -= LocalizeSettingChanged;
    }
    */

    void SetLangOption()
    {
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        for (int i = 1; i < LanguageManager.Instance.Languages.Count; i++)
        {
            optionDatas.Add(new Dropdown.OptionData() { text = LanguageManager.Instance.Languages[i].languageLocalize });
        }
        dropdown.options = optionDatas;
    }

    void LocalizeSettingChanged()
    {
        dropdown.value = LanguageManager.Instance.currentLanguageIndex;
    }
}