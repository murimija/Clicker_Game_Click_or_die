using UnityEngine;
using UnityEngine.UI;

public class ShowHighScore : MonoBehaviour
{
    public Text highScoreText;
    void Start()
    {
        highScoreText.text = SaveScore.maxScore.ToString();
    }
}