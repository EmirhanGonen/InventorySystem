using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/ProductData")]
public class ProductData : ScriptableObject, IInitialize
{
    [SerializeField] private Item _output = null;

    [SerializeField] private int _initializeHealth = 0;

    [SerializeField] private int _minOutputCount = 0;
    [SerializeField] private int _maxOutputCount = 0;

    private int _health = 0;

    public Item Output { get => _output; set => _output = value; }

    public int Health { get => _health; set => _health = value; }


    public int MinOutputCount { get => _minOutputCount; set => _minOutputCount = value; }
    public int MaxOutputCount { get => _maxOutputCount; set => _maxOutputCount = value; }

    public void Initialize()
    {
        _health = _initializeHealth;
    }

    public T GetNewData<T>() where T: ProductData
    {
        T _new = CreateInstance<T>();

        _new.Output = Output;
        _new.Health = _initializeHealth;

        _new.MinOutputCount = MinOutputCount;
        _new.MaxOutputCount = MaxOutputCount;

        return _new;
    }

}