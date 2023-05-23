using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SlotSwap : Singleton<SlotSwap>
{
    //[SerializeField] private ItemSlot _emptySlot = new();

    [SerializeField] private Image _itemImage = null;

    [SerializeField] private TextMeshProUGUI _itemQuantityText = null;

    public event System.Action<ItemSlot> OnSwappedAction = null;


    private Sprite _emptySprite = null;

    private ItemSlot EmptySlot => new(null, 0);

    public ItemSlot CurrentSlot
    {
        get => _currentSlot;
        set
        {
            CurrentSlot.ChangeSlot(value);

            Utility.SetImageSprite(_itemImage, CurrentSlot.Item != null ? CurrentSlot.Item.ItemSprite : _emptySprite);
            Utility.SetText(_itemQuantityText, CurrentSlot.Quantity.Equals(0) ? string.Empty : CurrentSlot.Quantity.ToString());
        }
    }
    private ItemSlot _currentSlot = null;


    private ItemSlot _tempSlot = new(null, 0);

    private void Start()
    {
        _emptySprite = Resources.Load<Sprite>("Empty");
        _currentSlot = EmptySlot;
    }

    private void Update()
    {
        if (CurrentSlot == null)
            return;

        // PointerEventData pointerData

        transform.position = Input.mousePosition;
    }

    public void OnClick(SlotUI _slotUI)
    {
        Debug.Log("<color=green> Girdi </color>");

        if (CurrentSlot.Item == null)
        {
            Debug.Log("<color=blue> Elimiz Boþtu </color>");

            CurrentSlot = _slotUI.CurrentSlot;

            _slotUI.CurrentSlot.ChangeSlot(EmptySlot);
        }
        else
        {
            Debug.Log("<color=red> Elimiz Dolu </color>");

            Debug.Log(_tempSlot.Item);

            _tempSlot = new(_slotUI.CurrentSlot.Item, _slotUI.CurrentSlot.Quantity);

            ItemSlot _tempCurrentSlot = new(CurrentSlot.Item, CurrentSlot.Quantity);

            _slotUI.CurrentSlot.ChangeSlot(_tempCurrentSlot);

            CurrentSlot = _tempSlot;

        }
        OnSwappedAction?.Invoke(_slotUI.CurrentSlot);

        /*if (_lastSlot.Item != null) //Elimiz Doluysa
        {
            Debug.Log("<color=red> ElimizDolu </color>");

            SlotUI _tempSlot = _slot;

            _slot.CurrentSlot = _lastSlot;

            if (_tempSlot.CurrentSlot.Item == null)
            {
                _lastSlot = null;
                CurrentSlot = null;
                return;
            }

            _lastSlot = _tempSlot.CurrentSlot;
        }
        else
        {
            Debug.Log("<color=white> ElimizBoþ </color>");

            CurrentSlot = _slot.CurrentSlot;
            _lastSlot = CurrentSlot;

            _slot.CurrentSlot = new(null, 0);
        }

        if (_slot.CurrentSlot.Item != null) //Týkladýðýmýz yer boþsa ve elimiz boþsa
        {
            Debug.Log("<color=blue> týkladýðýmýz yer boþ </color>");

            CurrentSlot = new(null, 0);
            _lastSlot = CurrentSlot;
        }
        else
        {
            Debug.Log("<color=gray> týkladýðýmýz yer dolu </color>");

            CurrentSlot = _slot.CurrentSlot;
            _lastSlot = CurrentSlot;
        }*/
    }
}