using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("General Settings"), Space]
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool isAutomatic = false;
    [SerializeField] private ShootType shootType = ShootType.Line;
    [Min(0), SerializeField] private int damageAmount = 1;
    [Min(0), SerializeField] private float shootCooldown = 1;
    //[SerializeField] private Slider attackCooldownBar;
    [SerializeField] private AudioClip shootSFX;

    [Header("Projectile Shooting"), Space]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileLife = 1;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Line Shooting"), Space]
    [Min(0), SerializeField] private float maxDistance = 1;

    //Internal Variables
    private float shootTimer = 0;

    private void Start()
    {
        shootTimer = shootCooldown;
        //attackCooldownBar.maxValue = shootCooldown;
        //attackCooldownBar.value = attackCooldownBar.maxValue;
    }

    private void Update()
    {
        canShoot = Time.timeScale > 0;
        shootTimer += Time.deltaTime;

        //bool isCoolingDown = attackCooldownBar.value < attackCooldownBar.maxValue;
        //attackCooldownBar.gameObject.SetActive(isCoolingDown);
        /*
        if (isCoolingDown)
        {
            attackCooldownBar.value += Time.deltaTime;
        }
        */
        bool isAiming = Input.GetKey(KeyCode.Mouse1);
        GetComponent<Animator>().SetBool("isAiming", isAiming);

        bool isShooting = ((Input.GetButtonDown("Fire1") && !isAutomatic)
                            || (Input.GetButton("Fire1") && isAutomatic))
                            && shootTimer >= shootCooldown;
        if (canShoot && isShooting)
        {
            switch (shootType)
            {
                case ShootType.Projectile:
                    SpawnProjectile();
                    break;

                case ShootType.Line:
                    ShootLine();
                    break;

                default:
                    break;
            }
        }
    }

    private void SpawnProjectile()
    {
        GameObject projectileClone = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectileClone.GetComponent<Rigidbody>().velocity = 10 * projectileSpeed * Camera.main.transform.forward;
        shootTimer = 0;
        //attackCooldownBar.value = 0;
        Camera.main.GetComponent<AudioSource>().PlayOneShot(shootSFX);
        Destroy(projectileClone, projectileLife);
    }

    private void ShootLine()
    {
        Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(shootDirection, out RaycastHit hit, maxDistance) && hit.collider.CompareTag("Enemy"))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.TakeDamage(damageAmount);
            shootTimer = 0;
            //attackCooldownBar.value = 0;
        }
    }

    public int GetDamage()
    {
        return damageAmount;
    }

    private enum ShootType { Projectile, Line }
}
