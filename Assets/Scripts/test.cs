using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private float buttonHalfSize;
    private Vector2 max = new Vector2(2, 2);
    private Vector2 min = new Vector2(-2,-2);
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Physics2D.OverlapArea(min, max) != null);
    }
    
    void SetMinMaxSize(Vector2 vector)
    {
        max.x = vector.x + buttonHalfSize;
        max.y = vector.y + buttonHalfSize;
        min.x = vector.x - buttonHalfSize;
        min.y = vector.y - buttonHalfSize;
    }
    
}
