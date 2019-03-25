using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToScene(string nameOfScene)
    {
        SceneManager.LoadScene(nameOfScene);
    }
}