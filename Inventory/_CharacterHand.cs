using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterHand : MonoBehaviour, IEnable, IDisable
{
    [SerializeField] private SpriteRenderer _itemImage = null;
    private GameObject ItemObject => _itemImage.gameObject;
    private Vector2 WorldMousePosition => Utility.GetWorldPointMousePosition();


    private InventoryHolder _inventoryHolder = null;

    public SlotUI CurrentSlotUI => _inventoryHolder.GetSlotUI(CurrentSlotIndex);
    private SlotUI _lastSlot = null;

    private int CurrentSlotIndex
    {
        get => _currentSlotIndex;
        set
        {
            _lastSlot = _inventoryHolder.GetSlotUI(CurrentSlotIndex);

            //_lastSlot.CurrentSlot.OnAddedItem -= GetItemToHand;

            //_inventoryHolder.Inventory.GetItemSlot(CurrentSlotIndex).OnAddedItem -= GetItemToHand;

            _currentSlotIndex = value;

            //Hotbar sayýsý

            if (_currentSlotIndex > 6)
                _currentSlotIndex = 0;

            if (_currentSlotIndex < 0)
                _currentSlotIndex = 6;

            //_inventoryHolder.Inventory.GetItemSlot(CurrentSlotIndex).OnAddedItem += GetItemToHand;

            OnChangedSlotIndex?.Invoke();
        }
    }
    [SerializeField] private int _currentSlotIndex = 0;

    private event System.Action OnChangedSlotIndex = null;

    private bool _canChangeSlot = true;


    public void OnEnable()
    {
        Inventory _inventory = GetComponentInParent<InventoryHolder>().Inventory;
        foreach (ItemSlot slot in _inventory.Slots)
        {
            slot.OnDroppedItem += GetItemToHand;
            slot.OnEndedItem += GetItemToHand;
        }

        OnChangedSlotIndex += ReSizeSelectedSlot;
        OnChangedSlotIndex += GetItemToHand;

        ItemInfoPopup.Instance.OnOpenedPopup += () => _canChangeSlot = false;
        ItemInfoPopup.Instance.OnClosedPopup += () => _canChangeSlot = true;

        OnChangedSlotIndex += GetItemToHand;
    }
    public void OnDisable()
    {
        Inventory _inventory = _inventoryHolder.Inventory;

        foreach (ItemSlot slot in _inventory.Slots)
        {
            slot.OnDroppedItem -= GetItemToHand;
            slot.OnEndedItem -= GetItemToHand;
        }

        OnChangedSlotIndex -= ReSizeSelectedSlot;
        OnChangedSlotIndex -= GetItemToHand;

        ItemInfoPopup.Instance.OnOpenedPopup -= () => _canChangeSlot = false;
        ItemInfoPopup.Instance.OnClosedPopup -= () => _canChangeSlot = true;

        OnChangedSlotIndex -= GetItemToHand;
    }

    private void Awake()
    {
        _inventoryHolder = GetComponent<InventoryHolder>();
    }
    private void Start()
    {
        CurrentSlotIndex = 0;
    }

    private void Update()
    {
        if (CurrentSlotUI.CurrentSlot.Item != null)
            if (Input.GetMouseButtonDown(0))
                UseItem();

        if (_canChangeSlot)
            ChangeSlotIndex();

        RotateItem();
    }

    private void ChangeSlotIndex()
    {
        if (GetNumberInput(out int _number))
            CurrentSlotIndex = _number;

        if (Input.mouseScrollDelta.y > 0)
            CurrentSlotIndex--;

        if (Input.mouseScrollDelta.y < 0)
            CurrentSlotIndex++;
    }

    private bool GetNumberInput(out int number)
    {
        for (int i = 1; i <= 7; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                number = i - 1;
                return true;
            }
        }

        number = 0;
        return false;
    }
    private void RotateItem()
    {
        Vector2 _itemDirection = (WorldMousePosition - (Vector2)transform.position).normalized;

        float _itemAngle = Mathf.Atan2(_itemDirection.y, _itemDirection.x) * Mathf.Rad2Deg;

        ItemObject.transform.parent.eulerAngles = new(0, 0, _itemAngle);
    }

    private void ReSizeSelectedSlot()
    {
        SlotUI _newSlot = CurrentSlotUI;

        Utility.SetScale(_newSlot.transform, _newSlot.DefaultScale * 1.15f, 0.25f);

        if (!_newSlot.Equals(_lastSlot))
            Utility.SetScale(_lastSlot.transform, _lastSlot.DefaultScale, 0.25f);
    }

    private void GetItemToHand()
    {
        GetItemToHand(CurrentSlotUI.CurrentSlot.Item, CurrentSlotUI.CurrentSlot.Quantity);
        /*_itemImage.gameObject.SetActive(CurrentSlotUI.CurrentSlot.Item);

        if (!CurrentSlotUI.CurrentSlot.Item)
            return;

        _itemImage.sprite = CurrentSlotUI.CurrentSlot.Item.ItemSprite;*/
    }
    private void GetItemToHand(InventoryItemBase _item, int _quantity)
    {
        Utility.SetActive(_itemImage.gameObject, _item);

        if (!_item)
            return;

        _itemImage.sprite = _item.ItemSprite;
    }

    private void UseItem()
    {
        CurrentSlotUI.CurrentSlot.UseItem();
    }
}