using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeScript : MonoBehaviour
{
    public string textKey;
    private Text textComponent;
    void Start()
    {   
        textComponent = GetComponent<Text>();

        if (textComponent != null)
        {
            LocalizeChanged();
            LanguageManager.Instance.LocalizeChanged += LocalizeChanged;
        }
    }
    /*
    void OnDestroy()
    {
        LanguageManager.Instance.LocalizeChanged -= LocalizeChanged;
    }*/

    void LocalizeChanged()
    {
        
        if (textComponent != null)
        {
            StartCoroutine(IEGetTextForKey());
        }
    }

    private IEnumerator IEGetTextForKey()
    {
        yield return new WaitUntil(() => LanguageManager.Instance.IsReady);

        textComponent.text = LanguageManager.Instance.GetTextByKey(textKey);
    }
}