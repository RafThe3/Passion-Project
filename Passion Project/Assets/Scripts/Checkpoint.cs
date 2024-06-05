using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;
    [Min(0), SerializeField] private float interactDistance = 1;

    private void Start()
    {
        interactText.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position - transform.position;
        bool isPlayerNear = playerPos.magnitude < interactDistance;

        if (isPlayerNear)
        {
            bool hasInteracted = Input.GetButtonDown("Interact");

            interactText.enabled = true;
            UpdateInteractText();

            if (hasInteracted)
            {
                //player.GetComponent<SaveSystem>().Save();
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }

    private void UpdateInteractText()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer | RuntimePlatform.WindowsEditor:
                interactText.text = "Press E to save";
                break;

            case RuntimePlatform.XboxOne:
                interactText.text = "Press Y to save";
                break;

            case RuntimePlatform.PS4 | RuntimePlatform.PS5:
                interactText.text = "Press Triangle to save";
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
