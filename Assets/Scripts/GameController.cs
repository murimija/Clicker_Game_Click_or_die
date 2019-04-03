using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class SaveScore
{
    public static int maxScore;
}

[Serializable]
public class SpawnArea
{
    public float xMax, xMin, yMax, yMin;
}

public class GameController : MonoBehaviour
{
    [SerializeField] private ButtonController button;
    [SerializeField] private GameObject cross;
    private Vector3 CrossSpawnPosition;
    
    [SerializeField] private Image backGround;
    [SerializeField] private SpawnArea spawnArea;
    [SerializeField] private float spawnWait;
    [SerializeField] private float accelerationOfSpawnWait;
    [SerializeField] private float minSpawnWait;
    [SerializeField] private Text scoreText;
    
    private float buttonHalfSize;
    private Vector2 max;
    private Vector2 min;
    private Vector2 ButtonSpawnPosition = Vector2.zero;

    private int scoreCounter = 200;
    private int itemCounter;

    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject sceneChanger;
    

    private Animator backGroundAnimator;
    private Animator scoreAnimator;
    
    [SerializeField] private Text extraPointsText;
    private Animator extraPointsTextAnimator;
    private int extraPoints;
    private bool isGameContinue = true;
    
    private static readonly int Shake = Animator.StringToHash("shake");
    private static readonly int Update1 = Animator.StringToHash("update");
    private static readonly int SuccessfulClick = Animator.StringToHash("successfulClick");
    private static readonly int ScoreUp = Animator.StringToHash("scoreUp");
    private static readonly int ScoreDown = Animator.StringToHash("scoreDown");
    private static readonly int Click = Animator.StringToHash("missClick");

    private readonly Collider2D[] resOfOverlapAreaNonAlloc = new Collider2D[2];

    private void Start()
    {
        buttonHalfSize = button.GetComponent<CircleCollider2D>().radius / 2f;
        scoreText.text = scoreCounter.ToString();
        scoreAnimator = scoreText.GetComponent<Animator>();
        backGroundAnimator = backGround.GetComponent<Animator>();
        extraPointsTextAnimator = extraPointsText.GetComponent<Animator>();
        StartCoroutine(SpawnButtonInTine());
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        var hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider.CompareTag("ClickButton"))
            hit.collider.GetComponent<ButtonController>().DestructionByClick();
        else
            MissClick();
    }

    public void BackGroundAndScoreGoodAnim()
    {
        backGroundAnimator.SetTrigger(SuccessfulClick);
        scoreAnimator.SetTrigger(ScoreUp);
    }

    public void BackGroundAndScoreWorseAnim()
    {
        backGroundAnimator.SetTrigger(Click);
        scoreAnimator.SetTrigger(ScoreDown);
    }

    private void MissClick()
    {
        CrossSpawnPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        CrossSpawnPosition.z = 0f;
        Instantiate(cross, CrossSpawnPosition, Quaternion.identity);
        BackGroundAndScoreWorseAnim();
        UpdateScore(-50);
        extraPoints = 0;
    }

    private IEnumerator SpawnButtonInTine()
    {
        yield return new WaitForSeconds(2);
        while (isGameContinue)
        {
            SpawnButton();
            yield return new WaitForSeconds(spawnWait);
        }
    }

    private void SpawnButton()
    {
        ButtonSpawnPosition.Set(Random.Range(spawnArea.xMin, spawnArea.xMax),
            Random.Range(spawnArea.yMin, spawnArea.yMax));
        SetMinMaxSize(ButtonSpawnPosition);
        var attemptToFind = 20;
        while (Physics2D.OverlapAreaNonAlloc(min, max, resOfOverlapAreaNonAlloc) > 1 && attemptToFind > 0)
        {
            ButtonSpawnPosition.Set(Random.Range(spawnArea.xMin, spawnArea.xMax),
                Random.Range(spawnArea.yMin, spawnArea.yMax));
            SetMinMaxSize(ButtonSpawnPosition);
            attemptToFind--;
        }

        if (Physics2D.OverlapAreaNonAlloc(min, max, resOfOverlapAreaNonAlloc) != 1) return;
        var btn = Instantiate(button, ButtonSpawnPosition, Quaternion.identity);
        btn.GameController = this;
    }

    private void SetMinMaxSize(Vector2 vector)
    {
        max.x = vector.x + buttonHalfSize;
        max.y = vector.y + buttonHalfSize;
        min.x = vector.x - buttonHalfSize;
        min.y = vector.y - buttonHalfSize;
    }

    private void ExtraPointsTextUpdate()
    {
        extraPointsTextAnimator.SetTrigger(Update1);
        extraPointsText.text = "Extra\npoints\n<size=50>" + extraPoints + "</size>";
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

    private void GameOver()
    {
        isGameContinue = false;
        StartCoroutine(waitBeforeGameOver());
    }

    private IEnumerator waitBeforeGameOver()
    {
        yield return new WaitForSeconds(3);
        sceneChanger.GetComponent<SceneChanger>().GoToScene(SaveScore.maxScore >= 80000 ? "Win_screen" : "End_menu");
    }

    public void incrementSumOfButtons()
    {
        itemCounter++;
        if (spawnWait > minSpawnWait)
        {
            spawnWait -= accelerationOfSpawnWait / itemCounter;
        }
    }

    public void ShakeCamera()
    {
        mainCamera.GetComponent<Animator>().SetTrigger(Shake);
    }
}
