using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiController : MonoBehaviour
{
	//Variables Expuestas
	[SerializeField] Transform _hpHolder;
	//Variables
	//Componentes
	//Clases
	playerController _playerController;
    void Awake()
    {
		_playerController = FindObjectOfType<playerController>();
    }
    private void OnEnable()
    {
        _playerController.OnHpChange += OnHpChange;
        _playerController.OnHpImprove += OnHpImprove;
    }
    private void OnDisable()
    {
		_playerController.OnHpChange -= OnHpChange;
		_playerController.OnHpImprove -= OnHpImprove;
	}

	#region Eventos
	private void OnHpImprove(int PreviousMaxHp)
	{
		_hpHolder.GetChild(PreviousMaxHp).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
	}
	private void OnHpChange(int CurrentHp, int Damage, HpCHangeTypes Type = HpCHangeTypes.Decrease)
	{
		for (int i = CurrentHp; i < Damage + CurrentHp; i++)
		{
			Transform _lostheart = _hpHolder.transform.GetChild(i);
			if (Type == HpCHangeTypes.Decrease)
			{
				_lostheart.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				_lostheart.GetChild(1).gameObject.SetActive(true);
			}
			else
			{
				_lostheart.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
				_lostheart.GetChild(1).gameObject.SetActive(false);
			}
		}
	}
	#endregion

	#region Metodos

	#endregion

	#region Propiedades

	#endregion

}
