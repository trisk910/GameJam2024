using UnityEngine;

public class InteractableRay : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask itemLayer;
    public LayerMask teleportLayer;
    public LayerMask minigameLayer;
    public Material highlightMaterial;
    public Material teleportHighlightMaterial;
    public Material minigameHighlightMaterial;

    private GameObject highlightedObject;
    private Material originalMaterial;

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

                case var obj when obj.layer == LayerMask.NameToLayer("Minigames"):
                    PickupMinigameButton(obj);
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
            HighlightObject(hit.collider.gameObject, teleportHighlightMaterial);
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, itemLayer))
        {
            HighlightObject(hit.collider.gameObject, highlightMaterial);
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, minigameLayer))
        {
            HighlightObject(hit.collider.gameObject, minigameHighlightMaterial);
        }
        else
        {
            RemoveHighlight();
        }
    }

    private void HighlightObject(GameObject obj, Material highlightMat)
    {
        if (highlightedObject != obj)
        {
            RemoveHighlight();
            highlightedObject = obj;
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
                renderer.material = highlightMat;
            }
        }
    }

    private void RemoveHighlight()
    {
        if (highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
            highlightedObject = null;
        }
    }

    private void PickupMinigameButton(GameObject button)
    {
        MinigameManager minigameManager = FindObjectOfType<MinigameManager>();
        if (minigameManager == null)
        {
            UnityEngine.Debug.Log("MinigameManager no trobat a l'escena!");
            return;
        }

        int buttonIndex = minigameManager.buttons.IndexOf(button);
        if (buttonIndex != -1)
        {
            minigameManager.ButtonPressed(buttonIndex);
        }
        else
        {
            UnityEngine.Debug.Log("El botó no està assignat al MinigameManager!");
        }
    }

    private void PickupItem(GameObject item)
    {
        UnityEngine.Debug.Log("Pickup item: " + item.name);
    }

    private void Teleport(GameObject teleportCube)
    {
        UnityEngine.Debug.Log("Teleport to: " + teleportCube.name);
    }
}
