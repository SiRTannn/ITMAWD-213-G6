using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreDefaultMatTime = 0.2f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        // Change material to the white flash material
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        // Revert material to the default
        spriteRenderer.material = defaultMat;
    }
}
