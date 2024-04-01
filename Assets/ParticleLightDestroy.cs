using UnityEngine;
using System.Collections;
public class ParticleLightDestroy : MonoBehaviour
{
    public float destroyDelay = 2.0f; // Adjust this value to set the delay before destruction

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
