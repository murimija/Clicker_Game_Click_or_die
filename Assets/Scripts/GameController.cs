using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public static class SaveScore
{
    public static int maxScore;
}

[System.Serializable]
public class SpawnArea
{
    public float xMax, xMin, yMax, yMin;
}

public class GameController : MonoBehaviour
{
    public GameObject button;
    public SpawnArea spawnArea;
    public float spawnWait;
    public float accelerationFromSumm;
    Quaternion spawnRotation = Quaternion.identity;

    private float buttonHalfSize;
    private Vector2 max = new Vector2();
    private Vector2 min = new Vector2();

    public Text scoreText;
    private int scoreCounter = 0;
    private int itemCounter = 0;
   

    public Camera mainCamera;

    public GameObject sceneChanger;

// Start is called before the first frame update
    void Start()
    {
        buttonHalfSize = (button.GetComponent<CircleCollider2D>().radius) / 2f;
        scoreText.text = scoreCounter.ToString();
        StartCoroutine(SpawnButtonInTine());
    }

    private void Update()
    {
        
        
        if (scoreCounter <= -1)
        {
            sceneChanger.GetComponent<SceneChanger>().GoToScene("End_menu");
        }
    }

    IEnumerator SpawnButtonInTine()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            SpawnButton();
            yield return new WaitForSeconds(spawnWait);
        }
    }

    void SpawnButton()
    {
        Vector2 spawnVector = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),
            Random.Range(spawnArea.yMin, spawnArea.yMax));
        SetMinMaxSize(spawnVector);
        int attemptToFind = 20;
        while (Physics2D.OverlapArea(min, max) != null && (attemptToFind > 0))
        {
            spawnVector.Set(Random.Range(spawnArea.xMin, spawnArea.xMax),
                Random.Range(spawnArea.yMin, spawnArea.yMax));
            SetMinMaxSize(spawnVector);
            attemptToFind--;
        }

        if (Physics2D.OverlapArea(min, max) == null)
        {
            Instantiate(button, spawnVector, spawnRotation);
        }
    }

    void SetMinMaxSize(Vector2 vector)
    {
        max.x = vector.x + buttonHalfSize;
        max.y = vector.y + buttonHalfSize;
        min.x = vector.x - buttonHalfSize;
        min.y = vector.y - buttonHalfSize;
    }

    public void UpdateScore(int plusScore)
    {
        scoreCounter += plusScore + (int)Mathf.Sign(plusScore)*itemCounter * 10;
        scoreText.text = scoreCounter.ToString();
        if (scoreCounter > SaveScore.maxScore) {
            SaveScore.maxScore = scoreCounter;
        }
    }

    public void ShakeCamera()
    {
        mainCamera.GetComponent<Animator>().SetTrigger("shake");
    }

    public void incremtntSummOfButtons()
    {
        itemCounter++;
        if (spawnWait > 0.25) {
            spawnWait -= accelerationFromSumm;
        }
    }

}