using UnityEngine;

public class Colorizer : MonoBehaviour
{
    public void SetRandomColor(Renderer renderer)
    {
        if (renderer != null)
        {
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}