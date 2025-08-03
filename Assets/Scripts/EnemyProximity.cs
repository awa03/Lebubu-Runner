using UnityEngine;

public class EnemyProximitySound : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 10f;

    [SerializeField] public AudioClip proximitySound1;
    [SerializeField] public AudioClip proximitySound2;
    public AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < triggerDistance)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(proximitySound1);
                audioSource.PlayOneShot(proximitySound2);
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
