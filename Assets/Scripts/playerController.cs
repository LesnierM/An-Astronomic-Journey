using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    //variables expuestas
    [SerializeField] float _movementSpeed;
    [SerializeField] Vector2 _bounceForce;
    [SerializeField] Vector2 _jumpForce;
    //variables
    Vector3 _inputSpeed;
    Vector3 _movementDirection;
    bool _isGrounded;
    bool _hasJumped;//para evitar que se vuelva a saltar mientras este grounded
    //componetnes
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rb2d;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        #region Direccion
        if (_spriteRenderer.flipY)
        {
            _movementDirection = Vector3.right;
        }
        else
        {
            _movementDirection = Vector3.left;
        }
        #endregion

        #region Controlar la rotacion del rebote
        if (_inputSpeed.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -16));
        }
        else if (_inputSpeed.x < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 16));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        #endregion
    }
    private void FixedUpdate()
    {
        Debug.Log(_isGrounded);
        #region Moviemiento del player
        transform.position += _inputSpeed * _movementSpeed * Time.deltaTime;
        #endregion
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (!_hasJumped)
            {
                _rb2d.AddForce(_bounceForce, ForceMode2D.Impulse);
            }
                _isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _isGrounded = false;
            _hasJumped = false;
        }
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
        if (Context.performed && _isGrounded && !_hasJumped)
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, 0);
            _rb2d.AddForce(_jumpForce, ForceMode2D.Impulse);
            _hasJumped = true; ;
        }
    }
    #endregion

}
