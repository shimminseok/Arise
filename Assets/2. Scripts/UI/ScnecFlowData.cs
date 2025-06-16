using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Scene Flow Data")]
public class SceneFlowData : ScriptableObject
{
    [System.Serializable]
    public class SceneLink
    {
        public string currentScene;
        public string nextScene;
    }

    public SceneLink[] sceneLinks;

    public string GetNextScene(string currentScene)
    {
        foreach (var link in sceneLinks)
        {
            if (link.currentScene == currentScene)
                return link.nextScene;
        }
        return null;
    }
}