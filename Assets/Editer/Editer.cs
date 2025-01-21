using UnityEngine;
using UnityEditor;

public class OpenURL : EditorWindow
{
    static private string _currentOpenURL;

    [MenuItem("Tools/SetupWizard")]
    static void Open()
    {
        GetWindow<OpenURL>("OpenURL");
    }

    void OnGUI()
    {
        if(GUILayout.Button("Set AssetStore"))
        {
            _currentOpenURL = "https://assetstore.unity.com/ja-JP";
            Debug.Log("AssetStore");
        }
        else if(GUILayout.Button("Set Github"))
        {
            _currentOpenURL = "https://github.com/NodaRintaro/Roguelike";
            Debug.Log("Set Github");
        }
        else if(GUILayout.Button("Set perplexity"))
        {
            _currentOpenURL = "https://www.perplexity.ai";
            Debug.Log("Set perplexity");
        }

        if (GUILayout.Button("Open AssetStore"))
        {
            //”CˆÓ‚ÌURL‚ðŠJ‚­
            Application.OpenURL(_currentOpenURL);
        }
    }
}