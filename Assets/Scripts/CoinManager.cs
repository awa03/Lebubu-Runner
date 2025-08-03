using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int totalCoins = 10;
    [SerializeField] private float spawnRadius = 50f;
    [SerializeField] private float maxRaycastHeight = 100f;
    [SerializeField] private float coinHeightOffset = 1f;
    [SerializeField] private LayerMask terrainMask = 1; 
    
    [SerializeField] private string terrainTag = "Terrain"; 
    [SerializeField] private float minSlopeAngle = 0f; 
    [SerializeField] private float maxSlopeAngle = 45f; 
    
    [SerializeField] private Text coinCounterText;
    [SerializeField] private Text totalCoinsText;
    
    private int coinsCollected = 0;
    private Terrain terrain;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        terrain = FindObjectOfType<Terrain>();
        if (terrain == null)
        {
            Debug.LogWarning("No Terrain found in scene! Using raycast method instead.");
        }
        
        SpawnCoins();
        UpdateUI();
    }
    
    private void SpawnCoins()
    {
        int coinsSpawned = 0;
        int attempts = 0;
        int maxAttempts = totalCoins * 10; 
        
        while (coinsSpawned < totalCoins && attempts < maxAttempts)
        {
            Vector3 spawnPosition = GetRandomTerrainPosition();
            if (spawnPosition != Vector3.zero)
            {
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                coinsSpawned++;
            }
            attempts++;
        }
        
        if (coinsSpawned < totalCoins)
        {
            Debug.LogWarning($"Only spawned {coinsSpawned} out of {totalCoins} coins. Check terrain setup.");
        }
    }
    
    private Vector3 GetRandomTerrainPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        
        if (terrain != null)
        {
            spawnPosition = GetRandomPositionOnTerrain();
        }
        else
        {
            spawnPosition = GetRandomPositionWithRaycast();
        }
        
        return spawnPosition;
    }
    
    private Vector3 GetRandomPositionOnTerrain()
    {
        Vector3 terrainPos = terrain.transform.position;
        TerrainData terrainData = terrain.terrainData;
        
        float randomX = Random.Range(0f, terrainData.size.x);
        float randomZ = Random.Range(0f, terrainData.size.z);
        
        float terrainHeight = terrain.SampleHeight(new Vector3(randomX + terrainPos.x, 0, randomZ + terrainPos.z));
        
        Vector3 worldPos = new Vector3(randomX + terrainPos.x, terrainHeight + coinHeightOffset, randomZ + terrainPos.z);
        
        if (IsValidSlope(worldPos))
        {
            return worldPos;
        }
        
        return Vector3.zero;
    }
    
    private Vector3 GetRandomPositionWithRaycast()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPos = new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y + maxRaycastHeight,
            transform.position.z + randomCircle.y
        );
        
        if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, maxRaycastHeight * 2f, terrainMask))
        {
            if (IsTerrainObject(hit.collider.gameObject) && IsValidSlope(hit.point, hit.normal))
            {
                return hit.point + Vector3.up * coinHeightOffset;
            }
        }
        
        return Vector3.zero;
    }
    
    private bool IsTerrainObject(GameObject obj)
    {
        if (!string.IsNullOrEmpty(terrainTag) && obj.CompareTag(terrainTag))
        {
            return true;
        }
        
        return ((1 << obj.layer) & terrainMask) != 0;
    }
    
    private bool IsValidSlope(Vector3 position)
    {
        if (terrain == null) return true;
        
        float sampleDistance = 1f;
        Vector3[] directions = {
            Vector3.forward * sampleDistance,
            Vector3.right * sampleDistance,
            Vector3.back * sampleDistance,
            Vector3.left * sampleDistance
        };
        
        float centerHeight = terrain.SampleHeight(position);
        float maxHeightDiff = 0f;
        
        foreach (Vector3 dir in directions)
        {
            Vector3 samplePos = position + dir;
            float sampleHeight = terrain.SampleHeight(samplePos);
            float heightDiff = Mathf.Abs(sampleHeight - centerHeight);
            maxHeightDiff = Mathf.Max(maxHeightDiff, heightDiff);
        }
        
        float angle = Mathf.Atan(maxHeightDiff / sampleDistance) * Mathf.Rad2Deg;
        return angle >= minSlopeAngle && angle <= maxSlopeAngle;
    }
    
    private bool IsValidSlope(Vector3 position, Vector3 normal)
    {
        float angle = Vector3.Angle(Vector3.up, normal);
        return angle >= minSlopeAngle && angle <= maxSlopeAngle;
    }
    
    public void CollectCoin()
    {
        coinsCollected++;
        UpdateUI();
        
        if (coinsCollected >= totalCoins)
        {
            OnAllCoinsCollected();
        }
    }
    
    private void UpdateUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = "Coins: " + coinsCollected;
        }
        
        if (totalCoinsText != null)
        {
            totalCoinsText.text = coinsCollected + " / " + totalCoins;
        }
    }
    
    private void OnAllCoinsCollected()
    {
        Debug.Log("All coins collected!");
    }
    
    public int GetCoinsCollected()
    {
        return coinsCollected;
    }
    
    public int GetTotalCoins()
    {
        return totalCoins;
    }
}