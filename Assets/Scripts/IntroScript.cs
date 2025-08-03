using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene(0); 
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(1);
    }
}