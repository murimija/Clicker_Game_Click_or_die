using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject destroyByClick;
    [SerializeField] private GameObject destroyByTime;

    public GameController GameController
    {
        set => gameController = value;
    }

    public void DestructionByClick()
    {
        Destroy(gameObject);
        Instantiate(destroyByClick, GetComponent<Transform>().position, Quaternion.identity);
        gameController.ShakeCamera();
        gameController.UpdateScore(50);
        gameController.incrementSumOfButtons();
        gameController.BackGroundAndScoreGoodAnim();
    }

    // ReSharper disable once UnusedMember.Local
    private void DestructionByTime()
    {
        Destroy(gameObject);
        Instantiate(destroyByTime, transform.position, Quaternion.identity);
        gameController.UpdateScore(-100);
        gameController.BackGroundAndScoreWorseAnim();
    }
}