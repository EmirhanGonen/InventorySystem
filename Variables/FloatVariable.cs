using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Variable/Float")]
public class FloatVariable : VariableData<float>
{
    public override void ChangeValue(float amount) => _value += amount;
    public override void EqualToAmount(float amount) => _value = amount;


    public static FloatVariable operator +(FloatVariable left, FloatVariable right)
    {
        left.ChangeValue(right.Value);

        return left;
    }
    public static FloatVariable operator -(FloatVariable left, FloatVariable right)
    {
        left.ChangeValue(-right.Value);

        return left;
    }
    public static FloatVariable operator /(FloatVariable left, FloatVariable right)
    {
        left.EqualToAmount(left.Value / right.Value);

        return left;
    }
    public static FloatVariable operator *(FloatVariable left, FloatVariable right)
    {
        left.EqualToAmount(left.Value * right.Value);

        return left;
    }
}