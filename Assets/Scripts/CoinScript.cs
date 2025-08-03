using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private AudioClip collectSound;
    
    private Vector3 startPosition;
    public AudioSource audioSource;
    
    void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }
    
    private void CollectCoin()
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        
        CoinManager.Instance.CollectCoin();
        
        Destroy(gameObject);
    }
}