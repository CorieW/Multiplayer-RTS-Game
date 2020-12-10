using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;

    [Header("Attributes")]
    [SerializeField] private PlayerObject _playerObject;
    [SerializeField] private int _amount = 1;

    public PlayerObject GetPlayerObject()
    {
        return _playerObject;
    }

    public int GetAmount()
    {
        return _amount;
    }

    public void SetPlayerObject(PlayerObject playerObj)
    {
        _playerObject = playerObj;
        _image.sprite = playerObj.GetComponent<SpriteRenderer>().sprite;
    }

    public void AddOne()
    {
        _amount++;
        _text.text = _amount + "x";
    }

    public void MinusOne()
    {
        _amount--;
        _text.text = _amount + "x";
    }
}
