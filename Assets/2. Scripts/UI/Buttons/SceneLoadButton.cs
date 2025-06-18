using UnityEngine;
using UnityEngine.UI;

public class SceneLoadButton : MonoBehaviour
{
    private SceneLoadEventChannelSO _sceneLoadEvent;
    private string _sceneToLoad;
    private Button _button;

    public void SetData(SceneLoadEventChannelSO sceneLoadEvent, string sceneToLoad)
    {
        _sceneLoadEvent = sceneLoadEvent;
        _sceneToLoad = sceneToLoad;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickLoadScene);
    }

    private void OnClickLoadScene()
    {
        
        PlayerPrefs.SetString("SelectedScene", _sceneToLoad);
        PlayerPrefs.Save();

        _sceneLoadEvent.Raise(_sceneToLoad);
    }


}