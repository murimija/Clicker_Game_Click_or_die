using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissClick : MonoBehaviour
{
    [SerializeField] private GameObject cross;

    private void OnMouseDown()
    {
        Debug.Log("1");
        Vector3 spawnVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnVector.z = 0f;
        Instantiate(cross, spawnVector, Quaternion.identity);
    }
    
}