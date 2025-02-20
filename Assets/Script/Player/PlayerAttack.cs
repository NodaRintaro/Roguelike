using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    private List<Button> _skillButtonList = new();

    Ray _ray;
    private RaycastHit _hit;
    private MapGenerator _mapGenerator;
    private Animator _animator;

    private void Start()
    {
        _mapGenerator = FindFirstObjectByType<MapGenerator>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _ray = new Ray(this.transform.position, transform.forward);
    }

    
}
