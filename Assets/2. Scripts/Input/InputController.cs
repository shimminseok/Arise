using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public PlayerInput PlayerInputs { get; private set; }
    public PlayerInput.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        PlayerInputs = new PlayerInput();
        PlayerActions = PlayerInputs.Player;
    }

    private void OnEnable()
    {
        PlayerInputs.Enable();
    }

    private void OnDisable()
    {
        PlayerInputs.Disable();
    }


}
