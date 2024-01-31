using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private WeaponData weaponData;
    private float timeBetweenShots;
    private bool triggerPressed = false;
    private bool isGrabbed = false;
    private float nextFireTime = 0f;
    private AudioSource audioSource;
    private Pose originPose;
    private XRGrabInteractable grabInteractable;
    private XRBaseController baseController;


    void Awake()
    {
        InitializeAudioSource();
        grabInteractable = GetComponent<XRGrabInteractable>();
        SetOrigin();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(WeaponGrabbed);
        grabInteractable.selectExited.AddListener(WeaponReleased);
        grabInteractable.activated.AddListener(SetActiveController);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(WeaponGrabbed);
        grabInteractable.selectExited.RemoveListener(WeaponReleased);
        grabInteractable.activated.RemoveListener(SetActiveController);
    }

    void Start()
    {
        PoolManager.Instance.CreatePool(weaponData.projectilePrefab.name, weaponData.projectilePrefab, weaponData.projectilePoolSize);
        timeBetweenShots = 60f / weaponData.roundsPerMinute;
    }

    void Update()
    {
        if (weaponData.isAutomatic && triggerPressed && isGrabbed)
        {
            AutomaticFire();
        }
    }

    private void InitializeAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
    }

    public void TriggerPressed()
    {
        SendRaycast();
        triggerPressed = true;
        if (!weaponData.isAutomatic)
            Fire();
    }

    public void TriggerReleased()
    {
        triggerPressed = false;
    }

    private void AutomaticFire()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + timeBetweenShots;
        }
    }

    private void SendRaycast()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(projectileOrigin.position, projectileOrigin.forward, out rayHit, 300f))
        {
            if (rayHit.transform.GetComponent<IRaycast>() != null)
                rayHit.transform.GetComponent<IRaycast>().HitByRaycast();
        }
    }

    public void Fire()
    {
        FireSFX();
        LaunchProjectile();
        SendHapticFeedback();
    }

    private void LaunchProjectile()
    {
        GameObject projectile = PoolManager.Instance.GetFromPool(weaponData.projectilePrefab.name);
        if (projectile != null)
        {
            projectile.transform.position = projectileOrigin.position;
            projectile.transform.rotation = projectileOrigin.rotation;

            IProjectile projectileComponent = projectile.GetComponent<IProjectile>();
            if (projectileComponent != null)
            {
                projectileComponent.Launch(transform.forward, weaponData.projectileSpeed);
                (projectileComponent as Projectile).SetDamage(weaponData.projectileDamage);
            }
        }
        else
        {
            Debug.LogWarning("Projectile pool is empty or does not exist!");
        }
    }

    void FireSFX()
    {
        audioSource.PlayOneShot(fireSound);
    }



    void WeaponGrabbed(SelectEnterEventArgs args)
    {
       isGrabbed = true;
    }

    void WeaponReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        triggerPressed = false;
        ReturnToOrigin();
    }

    void SetOrigin()
    {
        originPose.position = transform.position;
        originPose.rotation = transform.rotation;
    }

    public void ReturnToOrigin()
    {
        transform.position = originPose.position;
        transform.rotation = originPose.rotation;
    }

    private void SetActiveController(ActivateEventArgs arg)
    {
        baseController = arg.interactorObject.transform.GetComponent<XRBaseController>();
    }

    private void SendHapticFeedback()
    {
        if (baseController) baseController.SendHapticImpulse(.75f, .05f);
    }
}
