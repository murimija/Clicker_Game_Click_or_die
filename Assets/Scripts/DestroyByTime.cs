using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.5f;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}