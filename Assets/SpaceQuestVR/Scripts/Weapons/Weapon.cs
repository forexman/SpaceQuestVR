using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
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
    private int currentAmmo;
    private bool isReloading = false;
    private ControllerInputListener currentControllerListener;


    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image ammoImage;


    void Awake()
    {
        InitializeAudioSource();
        grabInteractable = GetComponent<XRGrabInteractable>();
        SetOrigin();
        currentAmmo = weaponData.magazineCapacity;
        timeBetweenShots = 60f / weaponData.roundsPerMinute;
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(WeaponGrabbed);
        grabInteractable.selectExited.AddListener(WeaponReleased);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(WeaponGrabbed);
        grabInteractable.selectExited.RemoveListener(WeaponReleased);
    }

    void Start()
    {
        PoolManager.Instance.CreatePool(weaponData.projectilePrefab.name, weaponData.projectilePrefab, weaponData.projectilePoolSize);
        timeBetweenShots = 60f / weaponData.roundsPerMinute;
    }

    void Update()
    {
        if (weaponData.firingMode == FiringMode.Automatic && triggerPressed && isGrabbed && !isReloading && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + timeBetweenShots;
        }

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }

        if (ammoImage != null)
        {
            ammoImage.fillAmount = (float)currentAmmo / weaponData.magazineCapacity;
        }
    }

    private void InitializeAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TriggerPressed()
    {
        triggerPressed = true;
        if (!isReloading)
        {
            SendRaycast();
            switch (weaponData.firingMode)
            {
                case FiringMode.Manual:
                    Fire();
                    break;
                case FiringMode.Burst:
                    if (Time.time >= nextFireTime)
                    {
                        StartCoroutine(FireBurst());
                        nextFireTime = Time.time + (timeBetweenShots * 3); // Adjust for burst delay
                    }
                    break;
                case FiringMode.Automatic:
                    // Automatic fire handled in Update
                    break;
            }
        }
        else
        {
            EmptySFX();
        }
    }

    private IEnumerator FireBurst()
    {
        int shots = 3;
        while (shots > 0 && currentAmmo > 0)
        {
            Fire();
            shots--;
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    public void TriggerReleased()
    {
        triggerPressed = false;
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
        if (currentAmmo <= 0 || isReloading)
        {
            return;
        }

        FireSFX();
        currentAmmo--;
        LaunchProjectile();
        SendHapticFeedback();

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        float reloadStartTime = Time.time;

        while (Time.time < reloadStartTime + weaponData.reloadTime)
        {
            // Gradually refill the ammo image as the weapon reloads
            if (ammoImage != null)
            {
                ammoImage.fillAmount = Mathf.Lerp(0, 1, (Time.time - reloadStartTime) / weaponData.reloadTime);
            }
            yield return null;
        }

        currentAmmo = weaponData.magazineCapacity;
        isReloading = false;
        UpdateAmmoUI();
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
        audioSource.PlayOneShot(weaponData.firingSound);
    }

    void EmptySFX()
    {
        audioSource.PlayOneShot(weaponData.emptySound);
    }

    void WeaponGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        baseController = args.interactorObject.transform.GetComponent<XRBaseController>();

        // Attach to the controller's input listener
        currentControllerListener = args.interactorObject.transform.GetComponent<ControllerInputListener>();
        if (currentControllerListener != null)
        {
            currentControllerListener.onPrimaryButtonPressed.AddListener(ReloadWeapon);
        }
    }

    void WeaponReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        triggerPressed = false;
        ReturnToOrigin();

        // Detach from the controller's input listener
        if (currentControllerListener != null)
        {
            currentControllerListener.onPrimaryButtonPressed.RemoveListener(ReloadWeapon);
            currentControllerListener = null;
        }

        ReloadWeapon();
    }

    private void ReloadWeapon()
    {
        if (!isReloading && currentAmmo != weaponData.magazineCapacity) StartCoroutine(Reload());
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

    private void SendHapticFeedback()
    {
        if (baseController) baseController.SendHapticImpulse(.75f, .05f);
    }
}
