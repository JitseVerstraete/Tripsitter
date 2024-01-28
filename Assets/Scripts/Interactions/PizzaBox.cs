using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBox : MonoBehaviour
{

    [SerializeField] private GameObject _pizzaPrefab;
    [SerializeField] private Interactable _interactable;

    private bool _tookPizza = false;

    void Start()
    {
        _interactable.OnInteractableClicked += TakePizza;
        _interactable.OnInteractableReleased += Reset;

    }

    private void Reset()
    {
        _tookPizza = false;
    }

    private void TakePizza()
    {
        if (_tookPizza)
        {
            return;
        }

        _tookPizza = true;
        GameObject pizzaGO = Instantiate(_pizzaPrefab);
        Interactable pizzaInteractable = pizzaGO.GetComponentInChildren<Interactable>();
        pizzaInteractable.ClickInteractable();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
