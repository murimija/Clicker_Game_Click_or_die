using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameController gameController;
    public float lifeTime = 3;
    public GameObject destroyByClick;
    public GameObject destroyByTime;
    private Quaternion spawnEffectRotation = Quaternion.identity;
    private GameObject gameControllerObject;
    

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

    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        
    }

    public void OnMouseDown()
    {
        gameControllerObject.GetComponent<GameController>().ShakeCamera();
        DestructionByClick();
    }
    
    void DestructionByClick()
    {
        Destroy(this.gameObject);
        Instantiate(destroyByClick, GetComponent<Transform>().position, spawnEffectRotation);
        gameController.UpdateScore(50);
        gameController.incremtntSummOfButtons();
    }

    void DestructionByTime()
    {
        Destroy(this.gameObject);
        Instantiate(destroyByTime, GetComponent<Transform>().position, spawnEffectRotation);
        gameController.UpdateScore(-100);
    }
}