using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class HumanAnimator : MonoBehaviour
{
    private const float CheckDelay = 0.1f;

    private const string ATTACK_ANIMATION_NAME = "Attack";
    private const string WALK_ANIMATION_NAME = "Walk";
    private const string Idle_ANIMATION_NAME = "Idle";

    public Dictionary<Animations, List<(float, Action)>> animationActions = new();
    public Animator animator;

    private Coroutine checkAnimation;


    public void PlayAnimation(Animations animation)
    {
        switch (animation)
        {
            case Animations.Idle:
                Idle();
                break;
            case Animations.Walk:
                Walk();
                break;
            case Animations.Attack:
                Attack();
                break;
        }

        if (checkAnimation != null)
            StopCoroutine(checkAnimation);

        if (animationActions.ContainsKey(animation))
            checkAnimation = StartCoroutine(CheckAnimations(animation, animator.GetCurrentAnimatorClipInfo(0).Length));
    }

    public void AddAnimationAction(Animations animation, float percentTime, Action action)
    {
        if (!animationActions.ContainsKey(animation))
            animationActions.Add(animation, new());

        animationActions[animation].Add((percentTime, action));
    }

    private IEnumerator CheckAnimations(Animations animation, float animationLength)
    {


        float timer = 0;
        float checkDelay = 0;

        List<(float, Action)> actions = new();

        foreach (var item in animationActions[animation])
            actions.Add(item);

        Debug.Log("CheckAnimations " + actions.Count);

        do
        {
            timer += Time.deltaTime;
            checkDelay += Time.deltaTime;

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].Item1 > timer / animationLength)
                {
                    actions[i].Item2.Invoke();
                    actions.Remove(actions[i]);

                    if (i - 1 >= 0)
                        i = i - 1;
                }
            }

            checkDelay = 0;

            yield return null;

        } while (timer > animationLength);

    }

    #region Animations
    [ContextMenu("Idle")]
    protected void Idle()
    {
        animator.CrossFade(Idle_ANIMATION_NAME, 0);
    }

    [ContextMenu("Walk")]
    protected void Walk()
    {
        animator.CrossFade(WALK_ANIMATION_NAME, 0);
    }

    [ContextMenu("Attack")]
    protected void Attack()
    {
        animator.CrossFade(ATTACK_ANIMATION_NAME, 0);
    }
    #endregion

    public void Dispose()
    {
        if (checkAnimation != null)
            StopCoroutine(checkAnimation);
    }

    private void OnDestroy()
    {
        Dispose();
    }
}

public enum Animations
{
    Idle,
    Walk,
    Attack
}
