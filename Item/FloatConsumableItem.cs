using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item/Consume")]
public class FloatConsumableItem : ConsumableItemBase<float>
{
    [SerializeField] private FloatVariable _targetValue = null;

    protected override void Consume()
    {
        _targetValue.Value += AmountToChange;
    }
}