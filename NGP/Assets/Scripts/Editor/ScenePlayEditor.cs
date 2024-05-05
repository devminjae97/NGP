#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

public class ScenePlayEditor : Editor

{
    [MenuItem("Edit/PlaySplash %#&P")]
    private static void PlaySplash()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/SplashScene.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}

#endif //UNITY_EDITOR