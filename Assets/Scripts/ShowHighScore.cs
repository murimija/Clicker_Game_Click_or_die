using UnityEngine;
using UnityEngine.UI;

public class ShowHighScore : MonoBehaviour
{
    public Text highScoreText;
    private void Start()
    {
        highScoreText.text = SaveScore.maxScore.ToString();
    }
}