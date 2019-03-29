using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
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
    [SerializeField] private ButtonController button;
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
    private Vector2 ButtonSpavnPosition;

    private int scoreCounter = 200;
    private int itemCounter;

    public Camera mainCamera;

    public GameObject sceneChanger;
    private Vector3 CrossSpawnPosition;

    private Animator backGraundAnimator;
    private Animator scoreAnimator;

    private int extraPoints;
    private bool isGameContinue = true;

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
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider.tag == "ClickButton")
                hit.collider.GetComponent<ButtonController>().DestructionByClick();
            else
                MissClick();
        }
    }

    public void BackGraunAndScoreGoodAnim()
    {
        backGraundAnimator.SetTrigger("successfulClick");
        scoreAnimator.SetTrigger("scoreUp");
    }

    public void BackGraunAndScoreWorseAnim()
    {
        backGraundAnimator.SetTrigger("missClick");
        scoreAnimator.SetTrigger("scoreDown");
    }

    void MissClick()
    {
        CrossSpawnPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        CrossSpawnPosition.z = 0f;
        Instantiate(cross, CrossSpawnPosition, Quaternion.identity);
        BackGraunAndScoreWorseAnim();
        UpdateScore(-50);
        extraPoints = 0;
    }

    IEnumerator SpawnButtonInTine()
    {
        yield return new WaitForSeconds(2);
        while (isGameContinue)
        {
            SpawnButton();
            yield return new WaitForSeconds(spawnWait);
        }
    }

    void SpawnButton()
    {
        ButtonSpavnPosition.Set(Random.Range(spawnArea.xMin, spawnArea.xMax),
            Random.Range(spawnArea.yMin, spawnArea.yMax));
        SetMinMaxSize(ButtonSpavnPosition);
        int attemptToFind = 20;
        while (Physics2D.OverlapAreaAll(min, max).Length > 1 && (attemptToFind > 0))
        {
            ButtonSpavnPosition.Set(Random.Range(spawnArea.xMin, spawnArea.xMax),
                Random.Range(spawnArea.yMin, spawnArea.yMax));
            SetMinMaxSize(ButtonSpavnPosition);
            attemptToFind--;
        }

        if (Physics2D.OverlapAreaAll(min, max).Length == 1)
        {
            var btn = Instantiate(button, ButtonSpavnPosition, Quaternion.identity);
            btn.GameController = this;
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
        if (isGameContinue)
        {
            if (plusScore < 0)
            {
                scoreCounter += plusScore - itemCounter * 10;
                if (extraPoints != 0)
                {
                    extraPoints = 0;
                    ExtraPointsTextUpdate();
                }
            }
            else
            {
                scoreCounter += plusScore + extraPoints;
                extraPoints += 50;
                ExtraPointsTextUpdate();
            }
        }

        if (scoreCounter <= 0)
        {
            scoreText.text = "0";
            GameOver();
            return;
        }

        scoreText.text = scoreCounter.ToString();

        if (scoreCounter > SaveScore.maxScore)
        {
            SaveScore.maxScore = scoreCounter;
        }
    }

    void GameOver()
    {
        isGameContinue = false;
        StartCoroutine(vaitBeforeGameOver());
    }

    IEnumerator vaitBeforeGameOver()
    {
        yield return new WaitForSeconds(3);
        if (SaveScore.maxScore >= 80000)
        {
            sceneChanger.GetComponent<SceneChanger>().GoToScene("Win_screen");
        }
        else
        {
            sceneChanger.GetComponent<SceneChanger>().GoToScene("End_menu");
        }
    }

    public void incremtntSummOfButtons()
    {
        itemCounter++;
        if (spawnWait > minSpawnWait)
        {
            spawnWait -= accelerationOfSpawnWait / itemCounter;
        }
    }

    public void ShakeCamera()
    {
        mainCamera.GetComponent<Animator>().SetTrigger("shake");
    }
}