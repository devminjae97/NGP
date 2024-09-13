using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorSceneButton : MonoBehaviour
{
    [SerializeField] private Button _scene1Button;
    [SerializeField] private Button _scene2Button;

    void Start()
    {
        _scene1Button.onClick.AddListener( () => OnClickEditorSceneButton( 0 ) );
        _scene2Button.onClick.AddListener( () => OnClickEditorSceneButton( 1 ) );
    }

    public void OnClickEditorSceneButton( int sceneNum )
    {
        EditorManager.Instance.SetScene( sceneNum );
    }
}
