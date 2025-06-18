using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerStates;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ForceReceiver))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerController : BaseController<PlayerController, PlayerState>, IAttackable
{
    [SerializeField] private WeaponController weaponController;
    private InputController _inputController;
    private CharacterController _characterController;
    private ForceReceiver _forceReceiver;

    private Vector2 _moveInput;
    private bool _isRunning;
    private bool _attackTriggered;

    private List<IDamageable> _targets = new List<IDamageable>();
    public bool IsTargetExists => _targets.Count > 0;

    public Vector2 MoveInput => _moveInput;
    public bool    IsRunning => _isRunning;

    public bool AttackTriggered
    {
        get => _attackTriggered;
        set => _attackTriggered = value;
    }

    public PlayerAnimation PlayerAnimation;


    public StatBase    AttackStat { get; private set; }
    public IDamageable Target     { get; private set; }
    public Transform   Transform  => transform;

    public WeaponController WeaponController => weaponController;


    protected override void Awake()
    {
        base.Awake();
        _inputController = GetComponent<InputController>();
        _characterController = GetComponent<CharacterController>();
        _forceReceiver = GetComponent<ForceReceiver>();

        PlayerAnimation = GetComponent<PlayerAnimation>();

        PlayerTable playerTable = TableManager.Instance.GetTable<PlayerTable>();
        PlayerSO    playerData  = playerTable.GetDataByID(0);
        StatManager.Initialize(playerData, null);
    }

    protected override void Start()
    {
        base.Start();
        // LockCursor();

        var action = _inputController.PlayerActions;
        action.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        action.Move.canceled += context => _moveInput = Vector2.zero;
        action.Attack.performed += context => _attackTriggered = true;
        action.Run.performed += context => _isRunning = true;
        action.Run.canceled += context => _isRunning = false;

        AttackStat = weaponController.StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
        
        UIManager.Instance.ConnectStatUI(gameObject, weaponController.gameObject);
        }

    protected override void Update()
    {
        base.Update();

        Rotate();
        FindTarget();
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
            PlayerState.Attack => new AttackState(weaponController.StatManager.GetValue(StatType.AttackSpd), weaponController.StatManager.GetValue(StatType.AttackRange)),
            PlayerState.Run    => new RunState(),
            _                  => null
        };
    }

    public override void Movement()
    {
        if (_moveInput.sqrMagnitude < 0.01f)
        {
            _characterController.Move(_forceReceiver.Movement * Time.deltaTime);
            return;
        }

        float   speed = StatManager.GetValue(StatType.MoveSpeed) * (_isRunning ? StatManager.GetValue(StatType.RunMultiplier) : 1f);
        Vector3 move  = (Vector3.right * _moveInput.x + Vector3.forward * _moveInput.y).normalized;

        Vector3 totalMovement = move * speed + _forceReceiver.Movement;

        _characterController.Move(totalMovement * Time.deltaTime);
    }

    public void Rotate()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    public void Attack()
    {
        Target?.TakeDamage(this);
    }

    public void AttackAllTargets()
    {
        if (_targets.Count == 0) return;

        foreach (var damageable in _targets.Where(x => !x.IsDead))
        {
            Target = damageable;
            Attack();
        }

        Target = null;
    }

    public override void FindTarget()
    {
        Target = null;
        _targets.Clear();

        Collider[] results = new Collider[5];
        var        size    = Physics.OverlapSphereNonAlloc(transform.position, weaponController.StatManager.GetValue(StatType.AttackRange), results, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < size; i++)
        {
            if (results[i].TryGetComponent(out IDamageable damageable))
            {
                Vector3 toTarget = (results[i].transform.position - transform.position).normalized;
                float   angle    = Vector3.Angle(transform.forward, toTarget);

                if (angle < 60f)
                {
                    _targets.Add(damageable);
                }
            }
        }
    }
    

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponController.StatManager?.GetValue(StatType.AttackRange) ?? 1f);
    }
}