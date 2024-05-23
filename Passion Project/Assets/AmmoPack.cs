using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    [Min(0), SerializeField] private int ammoToGive = 1;
    [SerializeField] private AudioClip pickupSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<Shooting>().AddAmmo(ammoToGive);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(pickupSFX);
            Destroy(gameObject);
        }
    }
}
