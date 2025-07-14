using UnityEngine;

public class OpenWhenEnemiesDefeated : MonoBehaviour
{
    private void Start()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.OnAllEnemiesDefeated.AddListener(DestroyDoor);
        }
    }

    public void DestroyDoor()
    {
        Destroy(gameObject);
    }
}
