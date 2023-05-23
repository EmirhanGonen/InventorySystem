using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory")]
public class Inventory : ScriptableObject, IEnable, IDisable
{
    [SerializeField] private List<ItemSlot> _slots = new();

    private event System.Action<Item> AddItemAction = null;
    private System.Action<List<SlotUI>> OpenedInventoryAction = null;

    private List<ItemSlot> _willRefreshSlots = new();

    public List<ItemSlot> Slots => _slots;
    public int InventoryLength => _slots.Count;


    public void OnEnable()
    {

        AddItemAction += AddItemToInventory;
        OpenedInventoryAction += RefreshSlots;
    }

    public void OnDisable()
    {
        AddItemAction -= AddItemToInventory;
        OpenedInventoryAction -= RefreshSlots;

    }

    public void Initialize(InventoryHolder _holder)
    {
        _willRefreshSlots = new();

        foreach (ItemSlot slot in _slots)
        {
            slot.Item = null;
            slot.Quantity = 0;
            slot.Holder = _holder;

            _willRefreshSlots.Add(slot);
        }

        AddItemAction = null;
        OpenedInventoryAction = null;
    }

    public void AddItem(Item _item) => AddItemAction?.Invoke(_item);
    public ItemSlot GetItemSlot(int _index) => _slots[_index];


    private void AddItemToInventory(Item _item)
    {
        if (!HaveAFreeSlot())
            return;

        ItemSlot _tempSlot = GetAvailableSlot(_item);
        _tempSlot.AddItem(_item);

        _willRefreshSlots.Add(_tempSlot);
    }

    private ItemSlot GetAvailableSlot(Item _item)
    {
        if (GetSameAvailableItemSlot(out ItemSlot _tempSlot, _item.ItemData))
        {
            Debug.Log("Returned Available Same");

            return _tempSlot;
        }

        return GetFreeSlot();
    }

    private bool GetSameAvailableItemSlot(out ItemSlot _slot, InventoryItemBase _item)
    {
        foreach (ItemSlot slot in _slots)
        {
            if (!slot.HasItem)
                continue;

            if (!slot.Item.Equals(_item) || slot.Quantity >= slot.Item.MaxQuantity)
                continue;

            Debug.Log("Has a Same Available");

            _slot = slot;
            return true;
        }

        Debug.Log("Has not a Same Available");
        _slot = null;
        return false;
    }

    private ItemSlot GetFreeSlot()
    {
        foreach (ItemSlot slot in _slots)
        {
            if (slot.HasItem)
                continue;

            return slot;
        }

        return null;
    }

    private bool HaveAFreeSlot()
    {
        foreach (ItemSlot item in _slots)
        {
            if (item.HasItem)
                continue;

            return true;
        }
        return false;
    }

    private void RefreshSlots(List<SlotUI> _slots)
    {
        //Hýzlý yazýlmýþ bir sistem yenilenebilir

        for (int i = 0; i < _willRefreshSlots.Count; i++)
        {
            _slots[this._slots.IndexOf(_willRefreshSlots[i])].CurrentSlot.InvokeAddedAction();

            Debug.Log(i);
        }
        /*foreach (SlotUI slot in _slots)
        {
            slot.CurrentSlot.InvokeAction();
            Debug.Log(slot.name);
        }*/

        _willRefreshSlots = new();
    }

    public void InvokeOpenedAction(List<SlotUI> _slots) => OpenedInventoryAction.Invoke(_slots);
}

[System.Serializable]
public class ItemSlot
{
    [SerializeField] private InventoryItemBase _item = null;
    [SerializeField] private int _quantity = 0;
    private InventoryHolder _holder = null;


    public InventoryItemBase Item { get => _item; set => _item = value; }
    public int Quantity { get => _quantity; set => _quantity = value; }

    public InventoryHolder Holder { get => _holder; set => _holder = value; }

    public int MaxQuantity => _item.MaxQuantity;
    public bool HasItem => _item != null;


    public event System.Action OnAddedItem = null;

    public event System.Action OnUsedItem = null;

    public event System.Action<InventoryItemBase, int> OnDroppedItem = null;

    public event System.Action OnEndedItem = null;

    public ItemSlot(InventoryItemBase _item, int _quantity, InventoryHolder _holder)
    {
        Item = _item;
        Quantity = _quantity;
        Holder = _holder;

        OnAddedItem = null;
        OnUsedItem = null;
        OnDroppedItem = null;
        OnEndedItem = null;
    }
    public ItemSlot(InventoryItemBase _item, int _quantity)
    {
        Item = _item;
        Quantity = _quantity;

        OnAddedItem = null;
        OnUsedItem = null;
        OnDroppedItem = null;
        OnEndedItem = null;
    }


    public void InvokeEndedAction() => OnEndedItem?.Invoke();
    public void InvokeAddedAction() => OnAddedItem?.Invoke();


    public void AddItem(Item _item)
    {
        if (Item == null)
            Item = _item.ItemData;

        //Item.Initialize();
        //Item.AddListenerToUseAction(() => UseItem());

        Quantity += _item.Quantity;

        if (Quantity <= Item.MaxQuantity)
        {
            _item.Quantity -= _item.Quantity;

            InvokeAddedAction();
            return;
        }

        int remainingQuantity = Quantity - Item.MaxQuantity;

        Quantity = Item.MaxQuantity;
        _item.Quantity = remainingQuantity;

        InvokeAddedAction();

        //item alýndýðýnda kalanýda otomatik envantere eklesin
    }

    public void ChangeSlot(ItemSlot _slot)
    {
        Item = _slot.Item;
        Quantity = _slot.Quantity;

        InvokeAddedAction();
    }


    public void UseItem()
    {
        if (Item is WeaponData)
        {
            Holder.GetComponentInChildren<WeaponHolder>().SendRay(Item as WeaponData);
            // (Item as WeaponData).Use();
            return;
        }

        Item.Use();

        Quantity--;
        OnUsedItem?.Invoke();

        if (Quantity > 0)
            return;

        Item.RemoveListenerToUseAction(UseItem);
        Item = null;
        OnEndedItem?.Invoke();
    }

    public void DropItem(InventoryItemBase _item, int _count)
    {
        Quantity -= _count;

        OnDroppedItem?.Invoke(_item, _count);

        if (!Quantity.Equals(0))
            return;

        Item = null;
        OnEndedItem?.Invoke();
    }
}