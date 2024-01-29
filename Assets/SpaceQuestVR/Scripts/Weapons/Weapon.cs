using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;

    private AudioSource audioSource;
    private Pose originPose;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        InitializeAudioSource();
        grabInteractable = GetComponent<XRGrabInteractable>();
        SetOrigin();
    }

    private void InitializeAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
    }

    void OnEnable() => grabInteractable.selectExited.AddListener(WeaponReleased);
    void OnDisable() => grabInteractable.selectExited.RemoveListener(WeaponReleased);

    public void Fire()
    {
        FireSFX();
        //raycast to hit - to be changed with projectile based damage
        RaycastHit rayHit;
        if (Physics.Raycast(projectileOrigin.position, projectileOrigin.forward, out rayHit, 300f))
        {
            if (rayHit.transform.GetComponent<EnemyManager>() != null)
            {
                rayHit.transform.GetComponent<EnemyManager>().EnemyDestroyed();
            }
            else if (rayHit.transform.GetComponent<IRaycast>() != null)
                rayHit.transform.GetComponent<IRaycast>().HitByRaycast();
        }
        GameObject projectile = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);
        //LaunchProjectile();

    }

    void FireSFX()
    {
        audioSource.PlayOneShot(fireSound);
    }

    private void LaunchProjectile()
    {
        GameObject projectile = PoolManager.Instance.GetFromPool("Projectile");

        if (projectile != null)
        {
            projectile.transform.position = projectileOrigin.position;
            projectile.transform.rotation = projectileOrigin.rotation;

            IProjectile projectileComponent = projectile.GetComponent<IProjectile>();
            if (projectileComponent != null)
            {
                projectileComponent.Launch(transform.forward, projectileSpeed);
            }
        }
        else
        {
            Debug.LogWarning("Projectile pool is empty or does not exist!");
        }
    }

    void WeaponReleased(SelectExitEventArgs args)
    {
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
}
