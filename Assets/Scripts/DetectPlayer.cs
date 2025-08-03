using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public float startTime;
    public GameObject endScreen;
    public GameObject tracker;
    public GameObject mainCamera;
    public GameObject endScreenCamera;
    void Start()
    {
        startTime = Time.time;
        endScreenCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            PlayerMovement capsuleController = other.GetComponent<PlayerMovement>();
            if (capsuleController != null)
            {
                capsuleController.frozen = true;
            }
            
            tracker.GetComponent<TrackerScript>().UpdateTimeText();
            Debug.Log("Player Died!, Time: " + (Time.time - startTime));
            endScreen.SetActive(true);
            
            if (endScreenCamera != null) endScreenCamera.SetActive(true); 
            if (mainCamera != null) mainCamera.SetActive(false);  
        }
    }
}
