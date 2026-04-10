using UnityEngine;
using System;
public class EnergyGemController : MonoBehaviour
{
    public static event Action OnGemCollected; // Sự kiện khi viên ngọc được thu thập
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectGem();
        }
    }
    public void CollectGem()
    {
        // Gọi sự kiện để thông báo rằng viên ngọc đã được thu thập
        OnGemCollected?.Invoke();
        // Sau khi thu thập, có thể thêm hiệu ứng hoặc logic khác ở đây (Sửa cơ chế này để tối ưu hơn)
        Destroy(gameObject); // Xóa viên ngọc khỏi scene sau khi thu thập
    }

}