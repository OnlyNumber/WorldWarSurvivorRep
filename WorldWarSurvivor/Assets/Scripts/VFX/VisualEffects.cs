using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffects : MonoBehaviour
{
    public Animations whichAnimation;
    public float TimeOfVisualEffect;

    public virtual void Initialize(Animations playAnimation, float time)
    {
        whichAnimation = playAnimation;
        TimeOfVisualEffect = time;
    }

    public void Subscibe(ModelController modelController)
    {
        Debug.Log("Subscribe");
        modelController.AddAnimationAction(whichAnimation, TimeOfVisualEffect, ActivateEffect);
    }

    public void Unsubscribe(ModelController modelController)
    {
        modelController.RemoveAnimationAction(whichAnimation, TimeOfVisualEffect, ActivateEffect);

    }

    public virtual void ActivateEffect()
    {

    }
}
