using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHighScore : MonoBehaviour
{
    public Text highScoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "with score: " + SaveScore.maxScore.ToString();
    }

}
