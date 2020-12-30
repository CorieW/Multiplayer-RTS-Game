using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedEntityUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _entityImage;
    [SerializeField] private Text _entityAmountText;

    [Header("Attributes")]
    [SerializeField] private PlayerEntity _playerEntity;
    [SerializeField] private int _amount = 1;

    public PlayerEntity GetPlayerEntity()
    {
        return _playerEntity;
    }

    public int GetAmount()
    {
        return _amount;
    }

    public void SetPlayerEntity(PlayerEntity playerEntity)
    {
        _playerEntity = playerEntity;
        _entityImage.sprite = playerEntity.GetComponent<SpriteRenderer>().sprite;
    }

    public void AddOne()
    {
        _amount++;
        _entityAmountText.text = _amount + "x";
    }

    public void MinusOne()
    {
        _amount--;
        _entityAmountText.text = _amount + "x";
    }
}
