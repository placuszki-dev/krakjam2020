﻿using System;
using UnityEngine;
using Random = System.Random;

public class Screw : MonoBehaviour
{
    [SerializeField] private ScrewCanvas _screwCanvas;
    [SerializeField] private GameObject _view;

    private bool active = false;

    private float _level;
    public int Level => (int) _level;

    [SerializeField] private float _multiplier = 0.5f;
    private int _previousPotValue;

    [SerializeField] private float _visualRotateMultiplier = 1f;
    private int _diff;

    private void OnEnable()
    {
        _previousPotValue = (int) Potentiometer.MaxVal;
        _level = new Random().Next(0, 80);
    }

    void Update()
    {
        if (!active)
            return;

        CalculateNewLevel();
        RotateView();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Enter");
        if(other.GetComponent<ScrewActivator>())
            Activate();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        print("Exit");

        if(other.GetComponent<ScrewActivator>())
            Deactivate();
    }

    private void CalculateNewLevel()
    {
        _diff = Potentiometer.Value - _previousPotValue;
        _previousPotValue = Potentiometer.Value;

        if (_diff > 0)
            _level += _diff * _multiplier;
    }

    private void RotateView()
    {
        if (_diff > 0)
            _view.transform.Rotate(Vector3.forward, -_visualRotateMultiplier * _diff);
    }

    public void Activate()
    {
        active = true;
        _screwCanvas.gameObject.SetActive(true);
    }
    
    public void Deactivate()
    {
        active = false;
        _screwCanvas.Deactivate();
    }
}