using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public UnityEvent OnAllEnemiesDefeated;
    private int enemiesAlive = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterEnemy()
    {
        enemiesAlive++;
    }

    public void UnregisterEnemy()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            OnAllEnemiesDefeated?.Invoke();
        }
    }
}
