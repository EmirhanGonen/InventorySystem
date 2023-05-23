using UnityEngine;

public abstract class ConsumableItemBase<T> : InventoryItemBase
{
    [SerializeField] private T _amountToChange;
    public T AmountToChange => _amountToChange;

    public override void Use()
    {
        base.Use();
        Consume();
    }


    protected abstract void Consume();
}