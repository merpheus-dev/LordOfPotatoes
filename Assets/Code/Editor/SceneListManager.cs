#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Editor
{
    public class SceneListManager : EditorWindow
    {
        Vector2 _scrollPosition = Vector2.zero;

        [MenuItem("Tools/Scene Manager")]
        public static void InitWindow()
        {
            EditorWindow editorWindow = EditorWindow.GetWindow<SceneListManager>("Scene Manager");
            editorWindow.Show();
        }

        private void OnGUI()
        {
            DrawHeader();

            DrawSceneList();
        }

        void DrawHeader()
        {
            GUILayout.Space(15);
            GUIStyle style = new GUIStyle()
            {
                //fontStyle = FontStyle.Bold,
                fontSize = 25,
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUILayout.LabelField("Scene List", style);
            GUILayout.Space(10);
        }

        void DrawSceneList()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, EditorStyles.helpBox);
            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorSceneManager.GetActiveScene().path == EditorBuildSettings.scenes[i].path)
                    GUI.color = Color.green;
                if (GUILayout.Button(GetNameFromScenePath(EditorBuildSettings.scenes[i].path)))
                {
                    var scene = EditorBuildSettings.scenes[i];
                    if (EditorApplication.isPlaying)
                    {
                        SceneManager.LoadScene(scene.path);
                    }
                    else
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }
                GUILayout.Space(2);
                if (EditorSceneManager.GetActiveScene().path == EditorBuildSettings.scenes[i].path)
                    GUI.color = Color.white;
            }
            EditorGUILayout.EndScrollView();
        }

        string GetNameFromScenePath(string path)
        {
            if (!path.Contains("/")) return "UNKNOWN SCENE";

            string[] delimiters = path.Split('/');
            return delimiters[delimiters.Length - 1];
        }
    }
}
#endif