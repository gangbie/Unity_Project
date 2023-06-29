using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();

    private float damageMax = 20f; // 최대 공격력
    private float damageMin = 10f; // 최소 공격력
    public Enemy enemyPrefab; // 생성할 적 AI

    private float healthMax = 110f; // 최대 체력
    private float healthMin = 100f; // 최소 체력

    [SerializeField] Transform[] spawnPoints; // 적 AI를 소환할 위치들

    public float speedMax = 7f; // 최대 속도
    public float speedMin = 3f; // 최소 속도

    public Color strongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색
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
            // 적의 세기를 0%에서 100% 사이에서 랜덤 결정
            var enemyIntensity = Random.Range(0f, 1f);
            // 적 생성 처리 실행
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
        // 사망한 적을 10 초 뒤에 파괴
        enemy.OnDeath += () => Destroy(enemy.gameObject, 2.5f);
        // 적 사망시 점수 상승
        enemy.OnDeath += () => GameManager.data.UpdateScore(10);
    }
}
