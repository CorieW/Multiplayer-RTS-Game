using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectsUI : MonoBehaviour
{
    const float X_OFFSET = 25;

    [Header("References")]
    [SerializeField] private SelectedObjectUI _selectedObjectUI;

    [Header("Attributes")]
    [SerializeField] private List<SelectedObjectUI> _selectedObjectUIs;

    private void Awake()
    {
        ObjectSelectionHandler.OnSelectObject += SelectObject;
        ObjectSelectionHandler.OnDeselectObject += DeselectObject;
    }
    
    private void OnDestroy() 
    {
        ObjectSelectionHandler.OnSelectObject -= SelectObject;
        ObjectSelectionHandler.OnDeselectObject -= DeselectObject;
    }

    private void SelectObject(PlayerObject playerObj)
    {
        foreach (SelectedObjectUI selectedObjUI in _selectedObjectUIs)
        {
            if(playerObj.GetType() != selectedObjUI.GetPlayerObject().GetType()) continue;

            // Found a selected object for object type, add to the selected object count.
            selectedObjUI.AddOne();
            return;
        }
        // There is currently no selected object UI that matches this type, create a one for this type.
        CreateNewSelectedObjectUI(playerObj);
    }

    private void DeselectObject(PlayerObject playerObj)
    {
        foreach (SelectedObjectUI selectedObjUI in _selectedObjectUIs)
        {
            if(playerObj.GetType() != selectedObjUI.GetPlayerObject().GetType()) continue;

            // Found a selected object for object type, remove from the selected object count.
            selectedObjUI.MinusOne();
            if (selectedObjUI.GetAmount() <= 0)
            { // None of this type is selected anymore, remove this type's selected object UI.
                _selectedObjectUIs.Remove(selectedObjUI);
                Destroy(selectedObjUI.gameObject);
                // Todo: I could run the below function only when the selectedObjUI is NOT the last element in the _selectedObjectsUIs list.
                ReorganiseUIPositions();
            }

            return;
        }
    }

    private void ReorganiseUIPositions()
    { // When a selected object UI is destroyed, re-organising the positions of the remaining UI is useful to prevent gaps.
        foreach (SelectedObjectUI selectedObjUI in _selectedObjectUIs)
        {
            float selectedObjUIRectWidth = ((RectTransform)_selectedObjectUI.transform).rect.width;
            // Subtracts 1 from count so it doesn't count itself. Notice when it was created, it was appended to list after positioning.
            Vector2 pos = new Vector2((selectedObjUIRectWidth + X_OFFSET) * (_selectedObjectUIs.Count - 1), 0);
            selectedObjUI.transform.localPosition = pos;
        }
    }

    private void CreateNewSelectedObjectUI(PlayerObject playerObj)
    { // Creates a new selected object UI, to show the object and the amount of the object type selected.
        SelectedObjectUI newSelectedObjUI = Instantiate(_selectedObjectUI, Vector3.zero, Quaternion.identity);
        newSelectedObjUI.SetPlayerObject(playerObj);

        newSelectedObjUI.transform.SetParent(transform);

        // Offsetting the position.
        float selectedObjUIRectWidth = ((RectTransform)_selectedObjectUI.transform).rect.width;
        Vector2 pos = new Vector2((selectedObjUIRectWidth + X_OFFSET) * _selectedObjectUIs.Count, 0);
        newSelectedObjUI.transform.localPosition = pos;

        _selectedObjectUIs.Add(newSelectedObjUI);
    }
}
