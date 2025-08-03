using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnScript : MonoBehaviour { 
void Update()
{
    if (Input.GetKeyDown(KeyCode.Return)) 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
}