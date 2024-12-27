using UnityEngine;

public class Fruit : MonoBehaviour
{

    private SpriteRenderer sr;
    private CircleCollider2D circle;
    public GameObject collected;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        circle = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            AudioManager.instance.PlayPickupSound();
            sr.enabled = false;
            circle.enabled = false;
            collected.SetActive(true);

            FruitManager.instance.AddFruit(1); // Adiciona 1 fruta
            Destroy(gameObject, 0.25f); // Destrói a fruta
        }
    }
}
