using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject destroyByClick;
    [SerializeField] private GameObject destroyByTime;
    [SerializeField] private GameObject gameControllerObject;

    void Start()
    {
        gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }
    
    public void DestructionByClick()
    {
        
        Destroy(this.gameObject);
        Instantiate(destroyByClick, GetComponent<Transform>().position, Quaternion.identity);
        gameController.GetComponent<GameController>().ShakeCamera();
        gameController.UpdateScore(50);
        gameController.incremtntSummOfButtons();
    }

    void DestructionByTime()
    {
        Destroy(this.gameObject);
        Instantiate(destroyByTime, GetComponent<Transform>().position, Quaternion.identity);
        gameController.UpdateScore(-100);
    }
}