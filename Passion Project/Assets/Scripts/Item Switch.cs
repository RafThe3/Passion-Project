using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    [SerializeField] private GameObject item1, item2;
    [Min(0), SerializeField] private float switchCooldown = 1;

    private bool hasSwitched = false;

    private void Start()
    {
        item1.SetActive(true);
        item2.SetActive(false);
    }

    private void Update()
    {
        bool switchedItem = Mathf.Abs(Input.mouseScrollDelta.y) > Mathf.Epsilon;

        if (switchedItem && !hasSwitched && Camera.main.fieldOfView == 60 && item2 && Time.timeScale > 0)
        {
            StartCoroutine(SwitchItem(switchCooldown));
        }
    }

    private IEnumerator SwitchItem(float cooldown)
    {
        hasSwitched = true;
        
        if (item1.activeSelf)
        {
            item1.SetActive(false);
            item2.SetActive(true);
        }
        else if (item2.activeSelf)
        {
            item1.SetActive(true);
            item2.SetActive(false);
        }

        yield return new WaitForSeconds(cooldown);

        hasSwitched = false;
    }

    /*
    public void AddItem(GameObject item)
    {
        items.Add(item);
    }
    */

    /*
    public void SwapItem(int index, GameObject item)
    {
        items.Insert(index, item);
    }
    */

    public void SwapItem(int itemNum, GameObject item)
    {
        if (itemNum is not 1 or 2)
        {
            throw new ArgumentException("Item number must be either 1 or 2.");
        }

        switch (itemNum)
        {
            case 1:
                item1 = item;
                break;

            case 2:
                item2 = item;
                break;

            default:
                break;
        }
    }
}
