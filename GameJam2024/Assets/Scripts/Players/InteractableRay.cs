using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class InteractableRay : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask itemLayer;
    public LayerMask teleportLayer;
    public Material highlightMaterial;
    public Material teleportHighlightMaterial;
    public InventoryManager inventoryManager;
    private GameObject highlightedObject;
    private Material originalMaterial;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = Camera.main.transform;
    }

    void Update()
    {
        DetectItem();

        if (Input.GetKeyDown(KeyCode.E) && highlightedObject != null)
        {
            switch (highlightedObject)
            {
                case var obj when obj.layer == LayerMask.NameToLayer("Teleports"):
                    Teleport(obj);
                    break;

                case var obj when obj.CompareTag("Flashlight"):
                    GameObject flashObject = obj.transform.parent.gameObject;
                    PickupFlashlight(flashObject);
                    break;

                default:
                    PickupItem(highlightedObject);
                    break;
            }
        }
    }

    private void DetectItem()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, rayDistance, teleportLayer))
        {
            if (hit.collider != null && highlightedObject != hit.collider.gameObject)
            {
                RemoveHighlight();
                highlightedObject = hit.collider.gameObject;
                AddHighlight(highlightedObject, true);
            }
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, itemLayer))
        {
            if (hit.collider != null && highlightedObject != hit.collider.gameObject)
            {
                RemoveHighlight();
                highlightedObject = hit.collider.gameObject;
                AddHighlight(highlightedObject, false); 
            }
        }
        else
        {
            RemoveHighlight();
        }
    }

    private void AddHighlight(GameObject obj, bool isTeleport)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            renderer.material = isTeleport ? teleportHighlightMaterial : highlightMaterial;
        }
    }

    private void RemoveHighlight()
    {
        if (highlightedObject != null && originalMaterial != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
            highlightedObject = null;
        }
    }

    private void PickupFlashlight(GameObject flashlight)
    {
        //UnityEngine.Debug.Log($"Picked up: {flashlight.name}");
        inventoryManager.EquipFlashlight(flashlight);
        Flashlight flashlightScript = flashlight.GetComponent<Flashlight>();
        flashlightScript.SetEquipped(true);
        RemoveHighlight();
    }

    private void PickupItem(GameObject item)
    {
        if (highlightedObject.layer == LayerMask.NameToLayer("Teleports"))
            return;

        UnityEngine.Debug.Log($"Picked up: {item.name}");
        inventoryManager.EquipOtherItem(item);
        RemoveHighlight();
    }

    private void Teleport(GameObject teleportCube)
    {
        if (teleportCube.CompareTag("TeleportA"))
        {
            this.transform.position = GameObject.FindWithTag("TeleportB").transform.position;
        }
        else if (teleportCube.CompareTag("TeleportB"))
        {
            this.transform.position = GameObject.FindWithTag("TeleportA").transform.position;
        }

        RemoveHighlight();
    }
}