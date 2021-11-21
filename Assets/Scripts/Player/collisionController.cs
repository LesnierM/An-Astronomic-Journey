using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionController : MonoBehaviour
{
    //Variables Expuestas
    [SerializeField] LayerMask _groundDetectionLayers;
    [SerializeField] LayerMask _playerColliderIgnoreLayer;
    [SerializeField] float _backToNormalStateDelay;
    //Variables
    Collider2D[] _overlapColliders = new Collider2D[5];

    ContactFilter2D _groundFilter;
    ContactFilter2D _playerColliderFilter;
    //Componentes
    CircleCollider2D _playerCollider;
    CircleCollider2D _playerBounceCollider;
    CapsuleCollider2D _groundCheckCollider;
    //Clases
    playerController _playerController;
    void Awake()
    {
        _playerController = GetComponent<playerController>();
        CircleCollider2D[] _circleCOlliders = GetComponents<CircleCollider2D>();
        foreach (var collider in _circleCOlliders)
        {
            if (collider.isTrigger)
            {
                _playerBounceCollider = collider;
            }
            else
            {
                _playerCollider = collider;
            }
        }
        _groundCheckCollider = GetComponent<CapsuleCollider2D>();

        _playerColliderFilter = new ContactFilter2D();
        _playerColliderFilter.useTriggers = true;
        _playerColliderFilter.SetLayerMask(_playerColliderIgnoreLayer);

        _groundFilter = new ContactFilter2D();
        _groundFilter.useTriggers = true;
        _groundFilter.SetLayerMask(_groundDetectionLayers);
    }
    void Update()
    {
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Collicionador de rebote
        #region Rebote
        for (int i = 0; i < _playerBounceCollider.OverlapCollider(_groundFilter, _overlapColliders); i++)
        {
            _playerController.bounce();
        }
        #endregion

        #endregion

        #region Colicionador principal
        for (int i = 0; i < _playerCollider.OverlapCollider(_playerColliderFilter, _overlapColliders); i++)
        {
            #region Daño por enemigo
            if (_overlapColliders[i].gameObject.layer == 7)
            {
                if (_playerController.State == PlayerStates.Normal)
                {
                    _playerController.damage(1);
                    Invoke("backToNormalState", _backToNormalStateDelay);
                }
            }
            #endregion

            #region Mejora de vida maxima
            else if (_overlapColliders[i].tag == "Hp+")
            {
                _playerController.improveHp();
                Destroy(_overlapColliders[i].gameObject);
            }
            #endregion

            #region Llenar vida
            else if (_overlapColliders[i].tag == "Heal")
            {
                if (_playerController.heal(1))
                {
                    Destroy(_overlapColliders[i].gameObject);
                }
            }
            #endregion

            #region Muerte Por Agujero
            else if (_overlapColliders[i].tag == "DeathHole")
            {
                _playerController.damage(_playerController.MaxHp);
            }
            #endregion
        }
        #endregion
    }

    #region Eventos

    #endregion

    #region Metodos
    void backToNormalState()
    {
        _playerController.State = PlayerStates.Normal;
    }
    public bool isGrounded()
    {
       return _groundCheckCollider.OverlapCollider(_groundFilter, new Collider2D[5])!=0?true:false;
    }
    #endregion

    #region Propiedades

    #endregion

}
