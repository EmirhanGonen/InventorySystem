using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VariableData<T> : ScriptableObject , IVariable<T>
{
    [SerializeField] private T _initializeValue;
    protected T _value;

    public T Value { get => _value; set => _value = value; }
    public T InitializeValue => _initializeValue;

    public void Initialize() => EqualToAmount(_initializeValue);

    public abstract void ChangeValue(T amount);
    public abstract void EqualToAmount(T amount);
}