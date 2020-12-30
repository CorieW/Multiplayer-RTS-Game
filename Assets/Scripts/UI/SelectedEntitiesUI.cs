using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedEntitiesUI : MonoBehaviour
{
    const float X_OFFSET = 25;

    [Header("References")]
    [SerializeField] private SelectedEntityUI _selectedEntityUI;

    [Header("Attributes")]
    [SerializeField] private List<SelectedEntityUI> _selectedEntityUIs;

    private void Awake()
    {
        EntitySelectionHandler.OnSelectEntity += SelectEntity;
        EntitySelectionHandler.OnDeselectEntity += DeselectEntity;
    }
    
    private void OnDestroy() 
    {
        EntitySelectionHandler.OnSelectEntity -= SelectEntity;
        EntitySelectionHandler.OnDeselectEntity -= DeselectEntity;
    }

    private void SelectEntity(PlayerEntity playerEntity)
    {
        foreach (SelectedEntityUI selectedEntityUI in _selectedEntityUIs)
        {
            if(playerEntity.GetType() != selectedEntityUI.GetPlayerEntity().GetType()) continue;

            // Found a selected entity for entity type, add to the selected entity count.
            selectedEntityUI.AddOne();
            return;
        }
        // There is currently no selected entity UI that matches this type, create a one for this type.
        CreateNewSelectedEntityUI(playerEntity);
    }

    private void DeselectEntity(PlayerEntity playerEntity)
    {
        foreach (SelectedEntityUI selectedEntityUI in _selectedEntityUIs)
        {
            if(playerEntity.GetType() != selectedEntityUI.GetPlayerEntity().GetType()) continue;

            // Found a selected entity for entity type, remove from the selected entity count.
            selectedEntityUI.MinusOne();
            if (selectedEntityUI.GetAmount() <= 0)
            { // None of this type is selected anymore, remove this type's selected entity UI.
                _selectedEntityUIs.Remove(selectedEntityUI);
                Destroy(selectedEntityUI.gameObject);
                // Todo: I could run the below function only when the selectedEntityUI is NOT the last element in the _selectedEntityUIs list.
                ReorganiseUIPositions();
            }

            return;
        }
    }

    private void ReorganiseUIPositions()
    { // When a selected entity UI is destroyed, re-organising the positions of the remaining UI is useful to prevent gaps.
        foreach (SelectedEntityUI selectedEntityUI in _selectedEntityUIs)
        {
            float selectedEntityUIRectWidth = ((RectTransform)_selectedEntityUI.transform).rect.width;
            // Subtracts 1 from count so it doesn't count itself. Notice when it was created, it was appended to list after positioning.
            Vector2 pos = new Vector2((selectedEntityUIRectWidth + X_OFFSET) * (_selectedEntityUIs.Count - 1), 0);
            selectedEntityUI.transform.localPosition = pos;
        }
    }

    private void CreateNewSelectedEntityUI(PlayerEntity playerEntity)
    { // Creates a new selected entity UI, to show the entity and the amount of the entity type selected.
        SelectedEntityUI newSelectedEntityUI = Instantiate(_selectedEntityUI, Vector3.zero, Quaternion.identity);
        newSelectedEntityUI.SetPlayerEntity(playerEntity);

        newSelectedEntityUI.transform.SetParent(transform);

        // Offsetting the position.
        float selectedEntityUIRectWidth = ((RectTransform)_selectedEntityUI.transform).rect.width;
        Vector2 pos = new Vector2((selectedEntityUIRectWidth + X_OFFSET) * _selectedEntityUIs.Count, 0);
        newSelectedEntityUI.transform.localPosition = pos;

        _selectedEntityUIs.Add(newSelectedEntityUI);
    }
}
