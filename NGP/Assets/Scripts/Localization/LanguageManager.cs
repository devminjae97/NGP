using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class Language
{
    public string languageName;
    public string languageLocalize;
    //public List<string> value = new List<string>();
    public Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
}

public class LanguageManager : Singleton<LanguageManager>
{
    const string languageURL = "https://docs.google.com/spreadsheets/d/1NPMaRKRsL6XzDRA_jxmwUFFIg4dy6BNlE97MGM9rUNo/export?format=tsv";
    public event System.Action LocalizeChanged = () => { };
    public event System.Action LocalizeSettingChanged = () => { };
    public int currentLanguageIndex;
    public List<Language> Languages;
    private bool _isReady = false;
    public bool IsReady => _isReady;

    void Awake()
    {
        InitLanguage();
        GetLanguage();
    }

    void InitLanguage()
    {
        int langIndex = PlayerPrefs.GetInt("LangIndex", -1); //언어 정보  저장
        int systemIndex = Languages.FindIndex(x => x.languageName.ToLower() == Application.systemLanguage.ToString().ToLower()); //유니티에서 제공하는 언어랑 비교
        if (systemIndex == -1) systemIndex = 0; //-1이면 영어로 기본 설정
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLanguageIndex(index);
    }

    public void SetLanguageIndex(int index)
    {
        currentLanguageIndex = index;
        PlayerPrefs.SetInt("LanguageIndex", currentLanguageIndex);
        LocalizeChanged(); // LocalizeScript.cs
        LocalizeSettingChanged(); //LocalizeSetting.cs
    }


    public string GetTextByKey(string key)
    {
        if (currentLanguageIndex >= 0 && currentLanguageIndex < Languages.Count)
        {
            if (Languages[currentLanguageIndex + 1].keyValuePairs.TryGetValue(key, out var outText))
            {
                return outText;
            }
            else
            {
                return "KeyNotFound";
            }
        }
        else
        {
            return "InvalidLanguageIndex";
        }
    }

    [ContextMenu("언어 가져오기")]
    void GetLanguage()
    {
        StartCoroutine(GetLanguageCo());
    }

    IEnumerator GetLanguageCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(languageURL);
        yield return www.SendWebRequest();
        SetLangList(www.downloadHandler.text);
    }

    void SetLangList(string tsv)
    {
        // 이차원 배열
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;
        string[,] Sentence = new string[rowSize, columnSize];

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++) Sentence[i, j] = column[j];
        }


        // 클래스 리스트
        Languages = new List<Language>();
        for (int i = 0; i < columnSize; i++)
        {
            Language lang = new Language();
            lang.languageName = Sentence[0, i];
            lang.languageLocalize = Sentence[1, i];

            for (int j = 2; j < rowSize; j++)
            {
                string key = Sentence[j, 0];
                string text = Sentence[j, i];
                lang.keyValuePairs[key] = text;
            }
            Languages.Add(lang);
        }
        _isReady = true;
    }
}