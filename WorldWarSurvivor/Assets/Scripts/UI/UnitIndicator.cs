using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnitIndicator : MonoBehaviour, IDisposable
{
    [SerializeField]private MeshRenderer meshRenderer;
    private Sequence _currentSequence;

    public void SetIndicator(GameObject indicatingUnit, Vector3 offset)
    {
        meshRenderer.enabled = true;
        transform.parent = indicatingUnit.transform;
        transform.localPosition = offset;

        _currentSequence = DOTween.Sequence();

        float up = transform.localPosition.y + 0.3f;
        float down = transform.localPosition.y;

        _currentSequence
        .SetLoops(-1, LoopType.Restart)
        .Append(transform.DOMoveY(up, 1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveY(down, 1f).SetEase(Ease.Linear));

    }

    public void TurnOffIndicator()
    {
        meshRenderer.enabled = false;
        _currentSequence.Kill();
        
    }

    public void Dispose()
    {
        _currentSequence.Kill();

    }

    private void OnDestroy()
    {
        Dispose();
    }
}
