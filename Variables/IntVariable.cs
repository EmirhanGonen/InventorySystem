using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(menuName ="ScriptableObjects/Variable/Int")]
public class IntVariable : VariableData<int>
{
    public override void ChangeValue(int amount) => _value += amount;
    public override void EqualToAmount(int amount) => _value = amount;

    

    public static IntVariable operator +(IntVariable left, IntVariable right)
    {
        left.ChangeValue(right.Value);

        return left;
    }
    public static IntVariable operator -(IntVariable left, IntVariable right)
    {
        left.ChangeValue(-right.Value);

        return left;
    }
    public static IntVariable operator /(IntVariable left, IntVariable right)
    {
        left.EqualToAmount(left.Value / right.Value);

        return left;
    }
    public static IntVariable operator *(IntVariable left, IntVariable right)
    {
        left.EqualToAmount(left.Value * right.Value);

        return left;
    }
}