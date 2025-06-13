using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [field:SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public CharacterController CharacterController { get; private set; }

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponent<Animator>();
        PlayerController = GetComponent<PlayerController>();
        CharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        
    }
}
