using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private Text _foodText;
    [SerializeField] private Text _woodText;
    [SerializeField] private Text _stoneText;
    [SerializeField] private Text _goldText;

    private void Awake()
    {
        //Resources.OnResourcesChange += HandleResourcesChange;
    }

    private void OnDestroy()
    {
        //Resources.OnResourcesChange -= HandleResourcesChange;
    }

    private void HandleResourcesChange(Resources resources)
    {
        // Todo: This needs to changed to count all the resources in the player's stockpile buildings.
        // Todo: Or the current Resources events can be used to count the total resources on the Player.
        // Todo: Whenever the player's total resources changes, update the text values here.
        /*_foodText.text = resources.GetResources()[ResourceType.Food].ToString();
        _woodText.text = resources.GetResources()[ResourceType.Wood].ToString();
        _stoneText.text = resources.GetResources()[ResourceType.Stone].ToString();
        _goldText.text = resources.GetResources()[ResourceType.Gold].ToString();*/
    }
}
