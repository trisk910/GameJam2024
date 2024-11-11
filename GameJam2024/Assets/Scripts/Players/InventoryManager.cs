using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    public Transform leftHand; 
    public Transform rightHand;
    public Transform dropPoint;
    public float dropOffset = 0.2f;

    private GameObject heldItem; 
    private GameObject flashlight;
    private GameObject shotgun;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
    }

    public void EquipItem(GameObject item)
    {
        switch (item.tag)
        {
            case "Flashlight":
                EquipFlashlight(item);
                break;

            case "Shotgun":
                EquipShotgun(item);
                break;
            case "Ammo":
                //EquipShotgun(item);
                break;

            default:
                EquipOtherItem(item);
                break;
        }
    }
    public void EquipFlashlight(GameObject item)
    {
        if (flashlight != null)
        {
            DropItem();
        }

        flashlight = item;
        flashlight.transform.SetParent(leftHand);
        flashlight.transform.localPosition = Vector3.zero;
        flashlight.transform.localRotation = Quaternion.identity;
        flashlight.SetActive(true);
    }

    public void EquipShotgun(GameObject item)
    {
        if (shotgun != null)
        {
            DropItem();
        }

        shotgun = item;
        shotgun.transform.SetParent(rightHand);
        shotgun.transform.localPosition = Vector3.zero;
        shotgun.transform.localRotation = Quaternion.identity;
        shotgun.SetActive(true);
        Shotgun shotgunS = shotgun.GetComponent<Shotgun>();
        shotgunS.SetEquipped(true);
    }

    public void EquipOtherItem(GameObject item)
    {
        if (heldItem != null)
        {
            DropItem();
        }

        heldItem = item;
        heldItem.transform.SetParent(rightHand);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;
        heldItem.SetActive(true);
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);

            RaycastHit hit;
            if (Physics.Raycast(dropPoint.position, Vector3.down, out hit))
            {
                heldItem.transform.position = hit.point + Vector3.up * dropOffset;
            }
            else
            {
                heldItem.transform.position = dropPoint.position + Vector3.down * dropOffset;
            }
            heldItem.SetActive(true);
            heldItem = null;
        }
    }
}