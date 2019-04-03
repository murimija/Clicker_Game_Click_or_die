using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.5f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}