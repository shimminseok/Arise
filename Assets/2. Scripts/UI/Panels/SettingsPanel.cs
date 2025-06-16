using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Button saveButton;

    private float _bgmValue = 0f;
    private float _sfxValue = 0f;
    private float _sensitivityValue = 0f;

    public void Show()
    {
        gameObject.SetActive(true);

        bgmSlider.value = _bgmValue;
        sfxSlider.value = _sfxValue;
        sensitivitySlider.value = _sensitivityValue;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        bgmSlider.value = _bgmValue;
        sfxSlider.value = _sfxValue;
        sensitivitySlider.value = _sensitivityValue;

        bgmSlider.onValueChanged.AddListener(v => _bgmValue = v);
        sfxSlider.onValueChanged.AddListener(v => _sfxValue = v);
        sensitivitySlider.onValueChanged.AddListener(v => _sensitivityValue = v);

        saveButton.onClick.AddListener(() =>
        {
            Debug.Log($"[세팅 저장] BGM: {_bgmValue}, SFX: {_sfxValue}, 감도: {_sensitivityValue}");
            // 추후에 저장 시스템을 만들면 여기서 관리하게 
        });
    }
}
