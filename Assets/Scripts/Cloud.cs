using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public Material cloudMaterial; // Material das nuvens
    public float cloudSpeed = 0.1f; // Velocidade do movimento das nuvens

    private Vector2 offset; // Armazena o deslocamento atual da textura

    void Update()
    {
        // Incrementa o deslocamento no eixo X com base no tempo
        offset.x += cloudSpeed * Time.deltaTime;

        // Aplica o deslocamento ao material
        if (cloudMaterial != null)
        {
            cloudMaterial.mainTextureOffset = offset;
        }
    }
}
