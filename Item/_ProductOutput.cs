using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductOutput : InventoryItemBase
{
    [SerializeField] private int _quantity = 0;

    public int Quantity { get => _quantity; set => _quantity = value; }
}