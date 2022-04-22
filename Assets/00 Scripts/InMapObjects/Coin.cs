using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinAmount;
    public float rotateSpeed;
    public float lastShown;
    public float floatingDuration;
    private float remainingDuration = 1f;
    public bool isCollected = false;
    [SerializeField] private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
        if (isCollected)
        {
            UpdateFloatingCoin();
        }
    }

    public void ChangeColorAlpha(float alpha)
    {
        Color currentColor = meshRenderer.material.GetColor("_BaseColor");
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        meshRenderer.material.SetColor("_BaseColor", newColor);
    }

    public void CollideWithPlayer()
    {
        isCollected = true;
        lastShown = Time.time;
        boxCollider.enabled = false;
    }

    public void UpdateFloatingCoin()
    {
        if (Time.time - lastShown > floatingDuration)
        {
            Destroy(this.gameObject);
        }

        transform.position += Vector3.up * 3 * Time.deltaTime;
        remainingDuration -= Time.deltaTime;
        ChangeColorAlpha((floatingDuration * remainingDuration) * 255);
    }
}
