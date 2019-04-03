using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Animator curtainForScenesTransition;
    private string nextScene;
    private static readonly int ChangeScene = Animator.StringToHash("ChangeScene");

    public void GoToScene(string nameOfScene)
    {
        nextScene = nameOfScene;
        curtainForScenesTransition.SetTrigger(ChangeScene);      
    }

    // ReSharper disable once UnusedMember.Global
    public void onTransitionComplete()
    {
        SceneManager.LoadScene(nextScene);
    }
}