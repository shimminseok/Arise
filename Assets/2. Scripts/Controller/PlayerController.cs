using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerStates;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class PlayerController : BaseController<PlayerController, PlayerState>, IAttackable, IDamageable
{
    [SerializeField] public LayerMask groundMask;
    
    private InputController _inputController;
    
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isRunning;
    private bool _attackTriggered;

    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;
    public bool IsRunning => _isRunning;
    public bool AttackTriggered { get; set; }
    
    public StatBase         AttackStat       { get; private set; }
    public IDamageable      Target           { get; private set; }
    public bool             IsDead           { get; private set; }
    public Transform        Transform        => transform;


    protected override void Awake()
    {
        base.Awake();
        _inputController = GetComponent<InputController>();
        
        PlayerTable playerTable = TableManager.Instance.GetTable<PlayerTable>();
        PlayerSO playerData  = playerTable.GetDataByID(0);
        StatManager.Initialize(playerData);
    }

    protected override void Start()
    {
        base.Start();
        // LockCursor();
        
        var action = _inputController.PlayerActions;
        action.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        action.Move.canceled += context => _moveInput = Vector2.zero;
        action.Look.performed += context => _lookInput = context.ReadValue<Vector2>();
        action.Attack.performed += context => _attackTriggered = true;
        action.Run.performed += context => _isRunning = true;
        action.Run.canceled += context => _isRunning = false;
    }

    protected override void Update()
    {
        base.Update();
        Rotate();
        _lookInput = _inputController.PlayerActions.Look.ReadValue<Vector2>();
    }

    /// <summary>
    /// 플레이어의 State를 생성해주는 팩토리 입니다.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    protected override IState<PlayerController, PlayerState> GetState(PlayerState state)
    {
        return state switch
        {
            PlayerState.Idle   => new IdleState(),
            PlayerState.Move   => new MoveState(),
            PlayerState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            PlayerState.Run    => new RunState(),
            _                  => null
        };
    }

    public override void Movement()
    {
        if (_moveInput.sqrMagnitude < 0.01f)
        {
            Agent.isStopped = true;
            return;
        }

        float speed = StatManager.GetValue(StatType.MoveSpeed) * (_isRunning ? StatManager.GetValue(StatType.RunMultiplier) : 1f);
        Vector3 direction = (transform.right * _moveInput.x + transform.forward * _moveInput.y).normalized;

        Agent.isStopped = false;
        Agent.Move(direction * speed * Time.deltaTime);
    }

    public void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }
    public void Attack()
    {
        Debug.Log("공격!");
        Target?.TakeDamage(this);
    }

    public override void FindTarget()
    {

    }

    public void TakeDamage(IAttackable attacker)
    {
        if (Target == null)
            Target = attacker as IDamageable;
        
        StatManager.Consume(StatType.CurHp, attacker.AttackStat.Value);

        float curHp = StatManager.GetValue(StatType.CurHp);
        if (curHp <= 0)
        {
            Daed();
        }
    }

    public void Daed()
    {
        IsDead = true;
        print($"플레이어 사망");
    }
    
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}