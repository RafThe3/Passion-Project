using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shooting : MonoBehaviour
{
    [Header("General Settings"), Space]
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool isAutomatic = false;
    [SerializeField] private ShootType shootType = ShootType.Line;
    [Min(0), SerializeField] private int damageAmount = 1;
    [Min(0), SerializeField] private float shootCooldown = 1;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] private Image crosshair;
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Projectile Shooting"), Space]
    [SerializeField] private bool hasInfiniteAmmo = false;
    [Min(0), SerializeField] private int startingAmmo = 30;
    [Min(0), SerializeField] private int maxAmmo = 150;
    [Min(0), SerializeField] private int startingRounds = 1;
    [Min(0), SerializeField] private float reloadCooldown = 1;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileLife = 1;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Line Shooting"), Space]
    [Min(0), SerializeField] private float maxDistance = 1;

    //Internal Variables
    private float shootTimer = 0;
    private int currentAmmo = 0, reserveAmmo = 0;
    private bool isReloading, isShooting;

    private void Start()
    {
        shootTimer = shootCooldown;
        currentAmmo = startingAmmo;
        reserveAmmo = startingAmmo * startingRounds;
        if (maxAmmo == 0)
        {
            maxAmmo = reserveAmmo;
        }
    }

    private void Update()
    {
        canShoot = Time.timeScale > 0;
        shootTimer += Time.deltaTime;

        UpdateAnimation();

        isShooting = ((Input.GetButtonDown("Fire1") && !isAutomatic)
                            || (Input.GetButton("Fire1") && isAutomatic))
                            && shootTimer >= shootCooldown
                            && !isReloading;

        if (canShoot && isShooting)
        {
            switch (shootType)
            {
                case ShootType.Projectile:
                    Shoot();
                    break;

                case ShootType.Line:
                    ShootLine();
                    break;

                default:
                    break;
            }
        }

        if (Input.GetButtonDown("Reload") && !isReloading && currentAmmo < startingAmmo)
        {
            StartCoroutine(Reload(reloadCooldown));
        }

        ammoText.text = $"{currentAmmo} / {reserveAmmo}";
    }

    private void UpdateAnimation()
    {
        bool isAiming = Input.GetKey(KeyCode.Mouse1);
        GetComponent<Animator>().SetBool("isAiming", isAiming);
        Camera.main.GetComponent<Animator>().SetBool("isAiming", isAiming);
        crosshair.enabled = !isAiming;
    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            GameObject projectileClone = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            projectileClone.GetComponent<Rigidbody>().velocity = 10 * projectileSpeed * Camera.main.transform.forward;
            currentAmmo--;
            shootTimer = 0;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(shootSFX);
            Destroy(projectileClone, projectileLife);
        }
    }

    public IEnumerator Reload(float reloadInterval)
    {
        bool hasReloaded = ((!hasInfiniteAmmo && reserveAmmo > 0) || hasInfiniteAmmo) && currentAmmo < startingAmmo;
        if (hasReloaded)
        {
            isReloading = true;
        }

        yield return new WaitForSeconds(reloadInterval);

        
        if (hasReloaded)
        {
            Camera.main.GetComponent<AudioSource>().PlayOneShot(reloadSFX);
        }
        

        if (hasInfiniteAmmo)
        {
            if (currentAmmo < startingAmmo)
            {
                currentAmmo = startingAmmo;
            }
        }
        else
        {
            if (currentAmmo < startingAmmo && reserveAmmo > 0)
            {
                int reloadAmount = startingAmmo - currentAmmo;
                reloadAmount = (reserveAmmo - reloadAmount) < 0 ? reserveAmmo : reloadAmount;
                currentAmmo += reloadAmount;
                reserveAmmo -= reloadAmount;
            }
        }

        isReloading = false;
    }

    private void ShootLine()
    {
        Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(shootDirection, out RaycastHit hit, maxDistance) && hit.collider.CompareTag("Enemy"))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.TakeDamage(damageAmount);
            shootTimer = 0;
        }
    }

    public int GetDamage()
    {
        return damageAmount;
    }

    public void AddAmmo(int ammo)
    {
        if (reserveAmmo < maxAmmo)
        {
            reserveAmmo += ammo;
        }

        if (reserveAmmo > maxAmmo)
        {
            reserveAmmo = maxAmmo;
        }
    }

    private enum ShootType { Projectile, Line }
}
