using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemInfoPopup : Singleton<ItemInfoPopup>, IDragHandler
{
    #region Drag Variables

    [Header("Drag")]
    [SerializeField] private Canvas _canvas = null;

    private RectTransform _rectTransform = null;

    #endregion

    #region Info Elements

    [Header("InfoElements")]
    [SerializeField] private TextMeshProUGUI _itemNameText = null;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText = null;
    [SerializeField] private TextMeshProUGUI _itemMaxQuantityText = null;
    [SerializeField] private TextMeshProUGUI _damageText = null;


    [SerializeField] private TMP_InputField _dropCountField = null;

    private GameObject _elements = null;

    #endregion

    public event System.Action OnOpenedPopup = null;
    public event System.Action OnClosedPopup = null;

    private ItemSlot _currentSlot = null;

    public bool IsElementActive => transform.GetChild(0).gameObject.activeSelf.Equals(true);
    public InventoryItemBase InfoItem => _currentSlot.Item;

    private void OnEnable()
    {
        SlotSwap.Instance.OnSwappedAction += ChangeCurrentSlot;
    }

    private void OnDisable()
    {
        SlotSwap.Instance.OnSwappedAction -= ChangeCurrentSlot;
    }

    protected override void Awake()
    {
        base.Awake();
        _rectTransform = GetComponent<RectTransform>();
        _elements = transform.GetChild(0).gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void Execute<T>(T _slot) where T : SlotUI
    {
        if (_slot.CurrentSlot.Item == null)
            return;

        OnOpenedPopup?.Invoke();

        //_currentSlot = _slot;
        ChangeCurrentSlot(_slot);

        Utility.SetText(_itemNameText, _slot.CurrentSlot.Item.ItemName);

        Utility.SetText(_itemDescriptionText, _slot.CurrentSlot.Item.ItemDescription);

        Utility.SetText(_itemMaxQuantityText, "Max Quantity{" + _slot.CurrentSlot.Item.MaxQuantity + "}");


        Utility.SetActive(_damageText.gameObject, true);

        if (_slot.CurrentSlot.Item is WeaponData)
            Utility.SetText(_damageText, "Damage{" + (_slot.CurrentSlot.Item as WeaponData).Damage + "}");
        else
            Utility.SetActive(_damageText.gameObject , false);

        Utility.SetActive(_elements, true);
    }

    public void Drop()
    {
        int _dropCount = int.Parse(_dropCountField.text);

        if (_currentSlot.Quantity < _dropCount)
            return;

        _currentSlot.DropItem(_currentSlot.Item, _dropCount);
    }

    public void InvokeDisabledAction() => OnClosedPopup?.Invoke();

    public void ChangeCurrentSlot(ItemSlot _slot) => _currentSlot = _slot;
    public void ChangeCurrentSlot(SlotUI _slotUI) => _currentSlot = _slotUI.CurrentSlot;

}