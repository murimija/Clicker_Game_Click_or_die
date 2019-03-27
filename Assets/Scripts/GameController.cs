using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;
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
    [SerializeField] private GameObject cross;
    [SerializeField] private Image backGraund;
    [SerializeField] private SpawnArea spawnArea;
    [SerializeField] private float spawnWait;
    [SerializeField] private float accelerationOfSpawnWait;
    [SerializeField] private float minSpawnWait;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text hitText;

    private float buttonHalfSize;
    private Vector2 max;
    private Vector2 min;


    private int scoreCounter = 50000;
    private int itemCounter;

    public Camera mainCamera;

    public GameObject sceneChanger;
    private Vector3 CrossSpavnPosition;

    private Animator backGraundAnimator;
    private Animator scoreAnimator;

    private int extraPoints;

    void Start()
    {
        buttonHalfSize = (button.GetComponent<CircleCollider2D>().radius) / 2f;
        scoreText.text = scoreCounter.ToString();
        scoreAnimator = scoreText.GetComponent<Animator>();
        backGraundAnimator = backGraund.GetComponent<Animator>();
        StartCoroutine(SpawnButtonInTine());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider.tag == "ClickButton")
            {
                DestroyButtonByClick(hit.collider);
            }
            else
            {
                MissClick();
            }
        }
    }

    void DestroyButtonByClick(Collider2D buttonObj)
    {
        buttonObj.GetComponent<ButtonController>().DestructionByClick();
        backGraundAnimator.SetTrigger("successfulClick");
        scoreAnimator.SetTrigger("scoreUp");
    }

    void MissClick()
    {
        CrossSpavnPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        CrossSpavnPosition.z = 0f;
        Instantiate(cross, CrossSpavnPosition, Quaternion.identity);
        backGraundAnimator.SetTrigger("missClick");
        scoreAnimator.SetTrigger("scoreDown");
        UpdateScore(-50);
        extraPoints = 0;
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
        while (Physics2D.OverlapAreaAll(min, max).Length > 1 && (attemptToFind > 0))
        {
            spawnVector.Set(Random.Range(spawnArea.xMin, spawnArea.xMax),
                Random.Range(spawnArea.yMin, spawnArea.yMax));
            SetMinMaxSize(spawnVector);
            attemptToFind--;
        }

        if (Physics2D.OverlapAreaAll(min, max).Length == 1)
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

    void ExtraPointsTextUpdate()
    {
        hitText.GetComponent<Animator>().SetTrigger("updete");
        hitText.text = "Extra\npoints\n<size=50>" + extraPoints + "</size>";
    }

    public void UpdateScore(int plusScore)
    {
        if (plusScore < 0)
        {
            scoreCounter += plusScore - itemCounter * 10;
            extraPoints = 0;
            ExtraPointsTextUpdate();
        }
        else
        {
            scoreCounter += plusScore + extraPoints;
            extraPoints += 50;
            ExtraPointsTextUpdate();
        }

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

    public void incremtntSummOfButtons()
    {
        itemCounter++;

        if (spawnWait > minSpawnWait)
        {
            spawnWait -= accelerationOfSpawnWait;
        }
    }

    public void ShakeCamera()
    {
        mainCamera.GetComponent<Animator>().SetTrigger("shake");
    }
}