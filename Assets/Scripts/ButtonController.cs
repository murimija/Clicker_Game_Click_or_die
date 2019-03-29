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
        Destroy(this.gameObject);
        Instantiate(destroyByClick, GetComponent<Transform>().position, Quaternion.identity);
        gameController.ShakeCamera();
        gameController.UpdateScore(50);
        gameController.incremtntSummOfButtons();
        gameController.BackGraunAndScoreGoodAnim();
    }

    void DestructionByTime()
    {
        Destroy(this.gameObject);
        Instantiate(destroyByTime, GetComponent<Transform>().position, Quaternion.identity);
        gameController.UpdateScore(-100);
        gameController.BackGraunAndScoreWorseAnim();
    }
}