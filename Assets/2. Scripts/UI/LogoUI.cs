using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoUI : MonoBehaviour
{
    [SerializeField] private GameObject _LogoUI;
    void Start()
    {
        StartCoroutine(ActivateLogoAfterDelay());
    }

    private IEnumerator ActivateLogoAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        _LogoUI.SetActive(true);
    }
}
