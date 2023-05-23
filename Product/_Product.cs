using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Product : MonoBehaviour, IDamagable
{
    [SerializeField] private ProductData _data = null;

    private ProductData _currentData = null;

    private void Awake()
    {
        _currentData = _data.GetNewData<ProductData>();

        //_currentData.Initialize();
    }

    public void TakeDamage(int _amount)
    {
        _currentData.Health -= _amount;

        if (_currentData.Health > 0)
            return;

        GiveOutput();
    }

    private void GiveOutput()
    {
        Item _spawnedItem = Instantiate(_currentData.Output, transform.position, Quaternion.identity);

        _spawnedItem.Quantity = Random.Range(_currentData.MinOutputCount, _currentData.MaxOutputCount + 1);

        Destroy(gameObject);
    }
}