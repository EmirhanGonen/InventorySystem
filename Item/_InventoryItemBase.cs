using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItemBase : ScriptableObject, IInitialize
{
    [SerializeField] private GameObject _item = null;

    [SerializeField] private string _itemName = string.Empty;
    [SerializeField] private Sprite _itemSprite = null;
    [SerializeField, TextArea] private string _itemDescription = string.Empty;

    [SerializeField] private int _maxQuantity = 100;

    public GameObject Item => _item;

    public string ItemName => _itemName;
    public Sprite ItemSprite => _itemSprite;
    public string ItemDescription => _itemDescription;

    public int MaxQuantity => _maxQuantity;


    protected event System.Action OnUsedItem = null;

    public void AddListenerToUseAction(System.Action _action) => OnUsedItem += _action;
    public void RemoveListenerToUseAction(System.Action _action) => OnUsedItem -= _action;


    protected void InvokUsedAction() => OnUsedItem?.Invoke();

    public void Initialize()
    {
        OnUsedItem = null;
    }

    public virtual void Use()
    {
        InvokUsedAction();
    }
    public virtual void Use(Vector2 _origin , Vector2 _direction , float _distance)
    {
        InvokUsedAction();
    }
}