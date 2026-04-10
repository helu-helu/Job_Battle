using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    private int currentSpawnedGemCount = 0;
    void OnEnable()
    {
        EnergyGemController.OnGemCollected += UpdateGemEnergyCount;
    }
    void OnDisable()
    {
        EnergyGemController.OnGemCollected -= UpdateGemEnergyCount;
    }
    private void UpdateGemEnergyCount()
    {
        currentSpawnedGemCount -= 1;
    }
    [Header("Spawn Settings")]
    // ScriptableObject chứa thông tin spawn (có thể là EnergyGem hoặc các loại khác)
    public EnergyGemScriptableObject spawnGemData;
    public int spawnCount = 20;
    // Bán kính an toàn của object để không đè lên viền hoặc mép vùng cấm
    public float objectRadius = 0.5f;

    [Header("Arena Settings")]
    public BoxCollider2D arenaBounds;
    public LayerMask exclusionLayer; // Chọn layer "ExclusionZone" trong Inspector

    // Giới hạn số lần tìm kiếm điểm hợp lệ để tránh vòng lặp vô hạn (Infinite Loop)
    // khi vùng cấm chiếm quá nhiều diện tích
    public int maxAttempts = 30;
    private Vector2 spawnPos;


    void Start()
    {
        StartCoroutine(SpawnInfiniteObjects(1f));
    }
    void Update()
    {

    }

    IEnumerator SpawnInfiniteObjects(float timeToWait)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToWait);
            // cần sửa
            if (currentSpawnedGemCount < spawnCount)
            {
                currentSpawnedGemCount++;
                SpawnObject();
            }

        }

    }
    private void SpawnObject()
    {
        spawnPos = GetValidSpawnPosition();
        // Kiểm tra xem có tìm được vị trí hợp lệ không
        if (spawnPos != new Vector2(float.MaxValue, float.MaxValue))
        {
            int randomGemType = Random.Range(0, 3); // 0: Blue, 1: Red, 2: Green
            switch (randomGemType)
            {
                case 0:
                    Instantiate(spawnGemData.blueGemPrefab, spawnPos, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(spawnGemData.redGemPrefab, spawnPos, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(spawnGemData.greenGemPrefab, spawnPos, Quaternion.identity);
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"Không tìm được vị trí hợp lệ để spawn.");
        }
    }

    private Vector2 GetValidSpawnPosition()
    {
        Bounds bounds = arenaBounds.bounds;

        for (int i = 0; i < maxAttempts; i++)
        {
            // 1. Lấy một tọa độ Random X, Y bên trong BoxCollider của đấu trường
            // Trừ đi objectRadius để object không bị sinh ra nằm lấp lửng ngoài viền đấu trường
            float randomX = Random.Range(bounds.min.x + objectRadius, bounds.max.x - objectRadius);
            float randomY = Random.Range(bounds.min.y + objectRadius, bounds.max.y - objectRadius);
            Vector2 randomPoint = new Vector2(randomX, randomY);

            // 2. Kiểm tra xem điểm này có chạm vào Exclusion Zone không
            // Dùng OverlapCircle với objectRadius để đảm bảo nguyên cả body của object an toàn
            Collider2D hit = Physics2D.OverlapCircle(randomPoint, objectRadius, exclusionLayer);

            // Nếu hit == null nghĩa là vùng đó trống, không đụng vùng cấm
            if (hit == null)
            {
                return randomPoint;
            }
        }

        // Nếu chạy hết vòng lặp vẫn không tìm được (do xui xẻo hoặc vùng cấm quá chật)
        // Trả về một vector cờ (flag) để báo lỗi
        return new Vector2(float.MaxValue, float.MaxValue);
    }
}