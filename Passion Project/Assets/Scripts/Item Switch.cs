using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    [SerializeField] private List<GameObject> items;
    [Min(0), SerializeField] private float switchCooldown = 1;

    private bool hasSwitched = false;

    private void Start()
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }

        items[0].SetActive(true);
    }

    private void Update()
    {
        bool switchedItem = Mathf.Abs(Input.mouseScrollDelta.y) > Mathf.Epsilon;

        if (switchedItem && !hasSwitched && Camera.main.fieldOfView == 60)
        {
            StartCoroutine(SwitchItem(switchCooldown));
        }
    }

    private IEnumerator SwitchItem(float cooldown)
    {
        hasSwitched = true;

        for (int i = 0; i < items.Count; i += 0)
        {
            i++;

            if (i > items.Count - 1)
            {
                i = 0;
                items[i].SetActive(true);
                items[^1].SetActive(false);
            }
            else
            {
                items[i].SetActive(true);
                items[i - 1].SetActive(false);
            }
        }

        yield return new WaitForSeconds(cooldown);

        hasSwitched = false;
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public void SwapItem(int index, GameObject item)
    {
        items.Insert(index, item);
    }
}
