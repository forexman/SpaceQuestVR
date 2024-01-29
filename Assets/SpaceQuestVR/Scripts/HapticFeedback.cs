using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedback : MonoBehaviour
{
    [SerializeField] XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = transform.GetComponent<XRGrabInteractable>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        grabInteractable.activated.AddListener(SendHapticFeedback);
    }

    void OnDisable()
    {
        grabInteractable.activated.RemoveListener(SendHapticFeedback);
    }

    private void SendHapticFeedback(ActivateEventArgs arg)
    {
        arg.interactorObject.transform.GetComponent<XRBaseController>().SendHapticImpulse(.75f, .15f);
    }

}
