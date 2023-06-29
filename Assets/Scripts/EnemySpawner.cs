using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();

    private float damageMax = 20f; // �ִ� ���ݷ�
    private float damageMin = 10f; // �ּ� ���ݷ�
    public Enemy enemyPrefab; // ������ �� AI

    private float healthMax = 110f; // �ִ� ü��
    private float healthMin = 100f; // �ּ� ü��

    [SerializeField] Transform[] spawnPoints; // �� AI�� ��ȯ�� ��ġ��

    public float speedMax = 7f; // �ִ� �ӵ�
    public float speedMin = 3f; // �ּ� �ӵ�

    public Color strongEnemyColor = Color.red; // ���� �� AI�� ������ �� �Ǻλ�
    private int wave;

    private void Awake()
    {
        wave = 1;
    }

    private void Update()
    {
        if (GameManager.data.isGameover) return;

        if (enemies.Count <= 0) SpawnWave();
    }

    private void SpawnWave()
    {
        wave++;

        var spawnCount = Mathf.RoundToInt(wave * 5f);

        for (var i = 0; i < spawnCount; i++)
        {
            // ���� ���⸦ 0%���� 100% ���̿��� ���� ����
            var enemyIntensity = Random.Range(0f, 1f);
            // �� ���� ó�� ����
            CreateEnemy(enemyIntensity);
        }
    }

    private void CreateEnemy(float intensity)
    {
        var health = Mathf.Lerp(healthMin, healthMax, intensity);
        var damage = Mathf.Lerp(damageMin, damageMax, intensity);
        var speed = Mathf.Lerp(speedMin, speedMax, intensity);

        var skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(health, damage, speed, speed * 0.3f, skinColor);

        enemies.Add(enemy);

        enemy.OnDeath += () => enemies.Remove(enemy);
        // ����� ���� 10 �� �ڿ� �ı�
        enemy.OnDeath += () => Destroy(enemy.gameObject, 2.5f);
        // �� ����� ���� ���
        enemy.OnDeath += () => GameManager.data.UpdateScore(10);
    }
}
