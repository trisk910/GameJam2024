using System.Collections;
using UnityEngine;

public class TestTargetSpawner : MonoBehaviour
{
    public GameObject testTargetPrefab; // The prefab to spawn
    private GameObject currentTarget;

    private void Start()
    {
        // Spawn the initial target
        RespawnTarget();
    }

    public void RespawnTarget()
    {
        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }
        StartCoroutine(Respawn());
        //UnityEngine.Debug.Log("A new TestTarget has been spawned!");
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        currentTarget = Instantiate(testTargetPrefab, transform.position, transform.rotation);
        currentTarget.transform.SetParent(transform);
    }
}
