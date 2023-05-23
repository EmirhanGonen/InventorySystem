using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryHolder : Singleton<InventoryHolder>
{
    //InventoryHolder Singleton olmamalý!
    [SerializeField] private GameObject _panel = null;
    [SerializeField] private Inventory _inventory = null;

    private List<SlotUI> _slots = new();

    public Inventory Inventory => _inventory;

    private bool _isInitialized = false;

    public void AddItem(Item item) => _inventory.AddItem(item);

    private void OnEnable()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            _inventory.Initialize(this);
        }
        _inventory.OnEnable();

        for (int i = 0; i < _inventory.InventoryLength; i++)
            _inventory.GetItemSlot(i).OnDroppedItem += Drop;
    }
    private void OnDisable()
    {
         _inventory.OnDisable();

        for (int i = 0; i < _inventory.InventoryLength; i++)
            _inventory.GetItemSlot(i).OnDroppedItem -= Drop;
    }

    private void Start()
    {
        Transform _panelParent = _panel.transform.parent;
        int _panelParentFirstChildCount = _panelParent.GetChild(0).childCount;

        for (int i = 0; i < _inventory.InventoryLength; i++)
        {
            int _childIndex = i < _panelParentFirstChildCount ? i : i - _panelParentFirstChildCount;

            int _parentChildIndex = i < _panelParentFirstChildCount ? 0 : 1;

            _slots.Add(_panelParent.GetChild(_parentChildIndex).GetChild(_childIndex).GetComponent<SlotUI>());
        }

        _panel.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _panel.SetActive(!_panel.activeSelf);
            _inventory.InvokeOpenedAction(_slots);
        }
    }

    public void Drop(InventoryItemBase _item, int _count)
    {
        Item _spawnedItem = Instantiate(_item.Item, (Vector2)transform.position + Vector2.right, Quaternion.identity).GetComponent<Item>();

        _spawnedItem.Quantity = _count;
    }

    public SlotUI GetSlotUI(int index)
    {
        if (_slots.Count < index + 1 || index < 0)
            return null;
        return _slots[index];
    }
}