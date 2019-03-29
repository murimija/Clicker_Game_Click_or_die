using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Animator curtainForScenesTransition;
    private String nextScene;
    public void GoToScene(string nameOfScene)
    {
        nextScene = nameOfScene;
        curtainForScenesTransition.SetTrigger("ChangeScene");      
    }

    public void onTransitinComplite()
    {
        SceneManager.LoadScene(nextScene);
    }
}