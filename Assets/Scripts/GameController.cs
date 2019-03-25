using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] private GameObject button;
    [SerializeField] private SpawnArea spawnArea;
    [SerializeField] private float spawnWait;
    [SerializeField] private float accelerationOfSpawnWait;
    [SerializeField] private float minSpawnWait;

    private float buttonHalfSize;
    private Vector2 max;
    private Vector2 min;

    public Text scoreText;
    private int scoreCounter;
    private int itemCounter;

    public Camera mainCamera;

    public GameObject sceneChanger;

    void Start()
    {
        buttonHalfSize = (button.GetComponent<CircleCollider2D>().radius) / 2f;
        scoreText.text = scoreCounter.ToString();
        StartCoroutine(SpawnButtonInTine());
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
            Instantiate(button, spawnVector, Quaternion.identity);
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
        scoreCounter += plusScore + (int) Mathf.Sign(plusScore) * itemCounter * 10;
        scoreText.text = scoreCounter.ToString();
        if (scoreCounter > SaveScore.maxScore)
        {
            SaveScore.maxScore = scoreCounter;
        }

        if (scoreCounter <= -1)
        {
            sceneChanger.GetComponent<SceneChanger>().GoToScene("End_menu");
        }
    }

    public void ShakeCamera()
    {
        mainCamera.GetComponent<Animator>().SetTrigger("shake");
    }

    public void incremtntSummOfButtons()
    {
        itemCounter++;
        if (spawnWait > minSpawnWait)
        {
            spawnWait -= accelerationOfSpawnWait;
        }
    }
}