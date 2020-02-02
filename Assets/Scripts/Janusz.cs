﻿using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Janusz : MonoBehaviour
{
    private Vector3 _januszPosition;
    private Image _image;
    private ScrewEventInvoker _eventInvoker;

    private int _anyEventOccuredCounter = 0;
    private bool _alreadyWalkedIn = false;

    [Header("Before Walk In")] [SerializeField]
    private int _eventsNeededToInvokeJanuszWalkIn = 3;

    [Header("Reactions")] [SerializeField] [Range(0, 100)]
    private int chanceToReact = 70;

    [Header("Happy Janusz Anim")] [SerializeField]
    private float _happyDuration = 2;

    [SerializeField] private float _happyPower = 2;
    [SerializeField] private AnimationCurve _happyCurve;

    [SerializeField] private Sprite _normalJanuszFace;
    [SerializeField] private List<Sprite> unfinishedScrewJanuszFaces;
    [SerializeField] private List<Sprite> okScrewJanuszFaces;
    [SerializeField] private List<Sprite> breakScrewJanuszFaces;

    void Start()
    {
        _januszPosition = transform.position;
        transform.position = _januszPosition + Vector3.right * 300;

        _eventInvoker = FindObjectOfType<ScrewEventInvoker>();
        _eventInvoker.AddScrewBreakListener(OnScrewBreak);
        _eventInvoker.AddScrewOkListener(OnScrewOK);
        _eventInvoker.AddScrewUnfinishedListener(OnScrewUnfinished);

        _image = GetComponent<Image>();
    }

    // Janusz reactions to events
    private void OnScrewOK()
    {
        OnAnyEventOccured();
        if (!_alreadyWalkedIn || !ShouldReact())
            return;

        _image.sprite = GetRandomImage(okScrewJanuszFaces);
        HappyJanuszAnimation();
    }

    private void OnScrewUnfinished()
    {
        OnAnyEventOccured();
        if (!_alreadyWalkedIn || !ShouldReact())
            return;
        
        _image.sprite = GetRandomImage(unfinishedScrewJanuszFaces);
    }

    private void OnScrewBreak()
    {
        OnAnyEventOccured();
        if (!_alreadyWalkedIn || !ShouldReact())
            return;
        
        _image.sprite = GetRandomImage(breakScrewJanuszFaces);

        GetComponent<Shaker>().Shake();
    }

    private void OnAnyEventOccured()
    {
        _anyEventOccuredCounter++;
        if (_anyEventOccuredCounter >= _eventsNeededToInvokeJanuszWalkIn && !_alreadyWalkedIn)
            WalkIn();

        _image.sprite = _normalJanuszFace;
    }

    // Other

    private void WalkIn()
    {
        print("WalkIn");
        transform.DOMove(_januszPosition, 3f).OnComplete(() => _alreadyWalkedIn = true);
    }

    private void HappyJanuszAnimation()
    {
        transform.DOMoveY(transform.position.y + _happyPower, _happyDuration).SetEase(_happyCurve);
    }

    private bool ShouldReact()
    {
        return Random.Range(0, 100) > (100 - chanceToReact);
    }

    private Sprite GetRandomImage(List<Sprite> sprites)
    {
        return sprites[(int) Random.Range(0, sprites.Count - 1)];
    }
}