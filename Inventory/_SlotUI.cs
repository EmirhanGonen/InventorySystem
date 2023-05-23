using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    private int _slotIndex = 0;

    private Image _itemImage = null;
    private TextMeshProUGUI _itemQuantityText = null;

    private Sprite _emptySprite = null;

    public Vector3 DefaultScale => _defaultScale;
    private Vector3 _defaultScale = Vector3.zero;

    public event System.Action<SlotUI> OnSelected = null;
    public event System.Action<SlotUI> OnSwapItem = null;

    public ItemSlot CurrentSlot
    {
        get
        {
            if (_currentSlot != null)
                return _currentSlot;

            InventoryHolder _holder = FindObjectOfType<InventoryHolder>();

            if (InventoryHolder.Instance == null)
                return _holder.Inventory.GetItemSlot(_slotIndex);

            return InventoryHolder.Instance.Inventory.GetItemSlot(_slotIndex);
        }
        set => CurrentSlot.ChangeSlot(value);
    }
    private ItemSlot _currentSlot = null;

    public bool IsSelected { set => _selected = value; }
    private bool _selected = false;


    private void OnEnable()
    {
        ItemInfoPopup _popUp = FindObjectOfType<ItemInfoPopup>();
        SlotSwap _swap = FindObjectOfType<SlotSwap>();

        OnSelected += _popUp.ChangeCurrentSlot;
        OnSelected += _popUp.Execute;

        OnSwapItem += _popUp.ChangeCurrentSlot;
        OnSwapItem += _swap.OnClick;

        CurrentSlot.OnAddedItem += SetItemImage;
        CurrentSlot.OnAddedItem += SetItemQuantityText;

        //CurrentSlot.AddListenerToAddedItem(SetItemImage);
        //CurrentSlot.AddListenerToAddedItem(SetItemQuantityText);

        CurrentSlot.OnUsedItem += SetItemQuantityText;


        // CurrentSlot.AddListenerToAddedItem(() => CurrentSlot.Item.AddListenerToUseAction(() => SetItemQuantityText()));

        //CurrentSlot.AddListenerToEndedItem(SetItemImage);
        //CurrentSlot.AddListenerToEndedItem(SetItemQuantityText);

        CurrentSlot.OnEndedItem += SetItemImage;
        CurrentSlot.OnEndedItem += SetItemQuantityText;

        //CurrentSlot.Item.AddListenerToUseAction(() => SetItemQuantityText());
        CurrentSlot.OnDroppedItem += SetItemQuantityText;

    }
    private void OnDisable()
    {
        ItemInfoPopup _popUp = FindObjectOfType<ItemInfoPopup>();
        SlotSwap _swap = FindObjectOfType<SlotSwap>();

        OnSelected -= _popUp.ChangeCurrentSlot;
        OnSelected -= _popUp.Execute;

        OnSwapItem -= _popUp.ChangeCurrentSlot;
        OnSwapItem -= _swap.OnClick;

        CurrentSlot.OnAddedItem -= SetItemImage;

        CurrentSlot.OnAddedItem -= SetItemQuantityText;

        // CurrentSlot.RemoveListenerToAddedItem(SetItemImage);
        //CurrentSlot.RemoveListenerToAddedItem(SetItemQuantityText);

        //CurrentSlot.AddListenerToAddedItem(() => CurrentSlot.Item.AddListenerToUseAction(() => SetItemQuantityText()));

        CurrentSlot.OnUsedItem -= SetItemQuantityText;
        //CurrentSlot.Item.RemoveListenerToUseAction(SetItemQuantityText);

        CurrentSlot.OnEndedItem -= SetItemImage;
        CurrentSlot.OnEndedItem -= SetItemQuantityText;
        //CurrentSlot.RemoveListenerToEndedItem(SetItemImage);
        //CurrentSlot.RemoveListenerToEndedItem(SetItemQuantityText);

        CurrentSlot.OnDroppedItem -= SetItemQuantityText;
    }


    private void Awake()
    {
        _defaultScale = transform.localScale;

        int _parentSiblingIndex = transform.parent.GetSiblingIndex();

        _slotIndex = _parentSiblingIndex.Equals(0) ? transform.GetSiblingIndex() : transform.GetSiblingIndex() + 7;

        _emptySprite = Resources.Load<Sprite>("Empty");

        _itemImage = transform.GetChild(0).GetComponentInChildren<Image>();
        _itemQuantityText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        //Bunlarý buraya aldým çünkü Inventorynin içindeki Foreach'e Addlistenerlar yetiþemiyordu ve çoðu yenilenmiyordu;
        SetItemImage();
        SetItemQuantityText();

        _currentSlot = InventoryHolder.Instance.Inventory.GetItemSlot(_slotIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_selected || ItemInfoPopup.Instance.IsElementActive)
            OnSwapItem?.Invoke(this);

        OnSelected?.Invoke(this);

        if (!_selected)
            _selected = true;
    }


    private void SetItemImage() => Utility.SetImageSprite(_itemImage, CurrentSlot.Item == null ? _emptySprite : CurrentSlot.Item.ItemSprite);

    private void SetItemQuantityText() => Utility.SetText(_itemQuantityText, CurrentSlot.Quantity <= 0 ? string.Empty : CurrentSlot.Quantity.ToString());
    private void SetItemQuantityText(InventoryItemBase _item, int _count)
    {
        int _currentQuantity = CurrentSlot.Quantity;

        string _newText = (_currentQuantity).ToString();

        Utility.SetText(_itemQuantityText, _newText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selected = false;
    }
}