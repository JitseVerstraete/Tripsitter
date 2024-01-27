using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{

    [SerializeField] private Material _cookedMaterial;
    [SerializeField] private MeshRenderer _pizzaMesh;

    public bool Cooked { get { return _cooked; } }
    private bool _cooked = false;

    public void CookPizza()
    {
        _pizzaMesh.material = _cookedMaterial;
        _cooked = true;
    }
}
