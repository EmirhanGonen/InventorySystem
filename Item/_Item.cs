using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    [SerializeField] private InventoryItemBase _itemData = null;
    [SerializeField] private int _quantity = 0;


    private TextMeshProUGUI _quantityText = null;


    public InventoryItemBase ItemData { get => _itemData; set => _itemData = value; }
    public int Quantity
    {
        get => _quantity; set
        {
            _quantity = value;

            SetQuantityText();

            if (_quantity <= 0)
                Destroy(gameObject);
        }
    }

    public int InstanceID => _instanceID;
    private int _instanceID = 0;


    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = ItemData.ItemSprite;

        _quantityText = GetComponentInChildren<TextMeshProUGUI>();

        _instanceID = GetInstanceID();
    }

    private void Start()
    {
        SetQuantityText();
    }

    private void SetQuantityText() => Utility.SetText(_quantityText, Quantity.ToString());

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Stack System && Bunun burda olmasý saçma!!
        Debug.Log(collision.name);
        if (!collision.TryGetComponent(out Item _item))
            return;

        Debug.Log(ItemData.ItemName.Equals(_item.ItemData.ItemName));

        if (!ItemData.Equals(_item.ItemData))
            return;

        if (_instanceID > _item.InstanceID)
        {
            Quantity += _item.Quantity;
            _item.Quantity = 0;
        }
    }
}