using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public enum HpCHangeTypes
{
    Increase,
    Decrease
}
public enum PlayerStates
{
    Normal,
    Injured
}
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    public delegate void OnHpChangeEventhandler(int CurrentHp, int Damage, HpCHangeTypes Type = HpCHangeTypes.Decrease);
    public delegate void OnHpImproveEventHandler(int PreviousMaxHp);
    public event OnHpImproveEventHandler OnHpImprove;
    public event OnHpChangeEventhandler OnHpChange;
    //variables expuestas
    [SerializeField] float _movementSpeed;
    [SerializeField] float _jumpHigh;
    [SerializeField] float _gravityScale;
    [SerializeField] float _bounceSpeed;


    //variables
    float _gravity;
    float _yVelocity;

    int _maxHp = 2;
    int _currentHp = 2;

    Vector3 _inputSpeed;

    PlayerStates _state;

    bool _isGrounded;
    bool _jump;
    bool _hasJumped;//para evitar que se vuelva a saltar mientras este grounded

    ContactFilter2D _filter;

    Collider2D[] _collidersOverlappingPlayer=new Collider2D[5];
    //componetnes
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rb2d;
    //Clases
    collisionController _collicionController;
    void Awake()
    {
        _gravity = Physics2D.gravity.y;
        _collicionController = GetComponent<collisionController>();
        _filter = new ContactFilter2D();
        _filter.useTriggers = true;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        #region Ground Check
        _isGrounded = _collicionController.isGrounded();
        //Debug.Log(_isGrounded);
        #endregion

        #region Gravedad
        _yVelocity += (_gravity * _gravityScale) * Time.deltaTime;
        #endregion

        #region Moviemiento del player
        float _Speed = _movementSpeed * Time.deltaTime;
        transform.position += new Vector3(_inputSpeed.x*_Speed, _yVelocity*Time.deltaTime, _inputSpeed.y*_Speed);
        #endregion
    }
 
   
    #region Input
    public void onMove(InputAction.CallbackContext Context)
    {
        _inputSpeed = new Vector3(Context.ReadValue<Vector2>().x, 0, 0);
        if (_inputSpeed.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_inputSpeed.x < 0)
        {
            _spriteRenderer.flipX = true; ;
        }

    }
    public void onJump(InputAction.CallbackContext Context)
    {
        if (_isGrounded && !_hasJumped)
        {
            _yVelocity = _jumpHigh * Time.deltaTime;
            _hasJumped = true;
        }
    }
    #endregion

    #region Metodos
    public void damage(int Damage)
    {
        _state = PlayerStates.Injured;
        _currentHp = Mathf.Clamp(_currentHp - Damage, 0, _maxHp);
        if (_currentHp <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (OnHpChange != null)
            {
                OnHpChange(_currentHp,Damage);
            }
        }
    }
    public void improveHp()
    {
        OnHpImprove(_maxHp);
        _maxHp++;
        _currentHp++;
    }
    public bool heal(int Hearts)
    {
        if (_currentHp != _maxHp)
        {
            if (OnHpChange != null)
            {
                OnHpChange(_currentHp, Hearts, HpCHangeTypes.Increase);
            }
            _currentHp++;
            return true;
        }
        return false;
    }
    public void bounce()
    {
        _yVelocity = _bounceSpeed*Time.deltaTime;
        _hasJumped = false;
    }
   
    #endregion

    #region Propiedades
    public PlayerStates State { get => _state; set => _state = value; }
    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    #endregion
}
