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

    public void OnMouseDown()
    {
        gameControllerObject.GetComponent<GameController>().ShakeCamera();
        DestructionByClick();
        Debug.Log("1");
    }

    void DestructionByClick()
    {
        Destroy(this.gameObject);
        Instantiate(destroyByClick, GetComponent<Transform>().position, Quaternion.identity);
        gameController.UpdateScore(50);
        gameController.incremtntSummOfButtons();
    }

    void DestructionByTime()
    {
        Destroy(this.gameObject);
        Instantiate(destroyByTime, GetComponent<Transform>().position, Quaternion.identity);
        gameController.UpdateScore(-100);
    }

    private void Update()
    {
        Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward * 10f);
    }
}