using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();
    private readonly BossZombie bossZombie = new BossZombie();

    private float damageMax = 20f; // 최대 공격력
    private float damageMin = 10f; // 최소 공격력
    public Enemy enemyPrefab; // 생성할 적 AI
    public BossZombie bossZombiePrefab;
    public AidKit aidKitPrefab;
    public Portal portalPrefab;
    public Gun famasGunPrefab;

    private float healthMax = 110f; // 최대 체력
    private float healthMin = 100f; // 최소 체력

    [SerializeField] Transform[] spawnPoints; // 적 AI를 소환할 위치들
    [SerializeField] Transform bossZombiePoint;
    [SerializeField] Transform portalPoint;

    private float speedMax = 7f; // 최대 속도
    private float speedMin = 3f; // 최소 속도

    public Color strongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색
    private int wave;

    private void Awake()
    {
        wave = GameManager.Instance.curStageNum;
        SpawnWave();
        GameManager.data.UpdateEnemy(enemies.Count);
    }

    private void Update()
    {
        if (GameManager.data.isGameover) return;
    }

    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 10f);

        CreateBossEnemy(1f);
        for (int i = 0; i < spawnCount; i++)
        {
            // 적의 세기를 0%에서 100% 사이에서 랜덤 결정
            var enemyIntensity = Random.Range(0f, 1f);
            // 적 생성 처리 실행
            CreateEnemy(enemyIntensity, i);
        }
    }

    private void CreateEnemy(float intensity, int point)
    {
        var health = Mathf.Lerp(healthMin, healthMax, intensity);
        var damage = Mathf.Lerp(damageMin, damageMax, intensity);
        var speed = Mathf.Lerp(speedMin, speedMax, intensity);

        var skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        var spawnPoint = spawnPoints[point];

        var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        

        enemy.Setup(health, damage, speed, speed * 0.3f, skinColor);

        enemies.Add(enemy);

        if (enemy.health > healthMax - 2)
        {
            enemy.OnDeath += () => enemies.Remove(enemy);
            // 사망한 적을 n초 뒤에 파괴
            enemy.OnDeath += () => Destroy(enemy.gameObject, 2f);
            enemy.OnDeath += () => Instantiate(aidKitPrefab, enemy.transform.position, enemy.transform.rotation);
            // 적 사망시 점수 상승
            enemy.OnDeath += () => GameManager.data.UpdateScore(10);
            enemy.OnDeath += () => GameManager.data.UpdateEnemy(enemies.Count);
            enemy.OnDeath += () => { if (enemies.Count <= 0) Instantiate(portalPrefab, portalPoint.position, portalPoint.rotation); };
        }
        else
        {
            enemy.OnDeath += () => enemies.Remove(enemy);
            // 사망한 적을 n초 뒤에 파괴
            enemy.OnDeath += () => Destroy(enemy.gameObject, 2f);
            // 적 사망시 점수 상승
            enemy.OnDeath += () => GameManager.data.UpdateScore(10);
            enemy.OnDeath += () => GameManager.data.UpdateEnemy(enemies.Count);
            enemy.OnDeath += () => { if (enemies.Count <= 0) Instantiate(portalPrefab, portalPoint.position, portalPoint.rotation); };
        }
    }

    private void CreateBossEnemy(float intensity)
    {
        var health = 300 * GameManager.Instance.curStageNum;
        var damage = 30;
        var speed = 10;

        var skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        var bossEnemy = Instantiate(bossZombiePrefab, bossZombiePoint.position, bossZombiePoint.rotation);

        bossEnemy.Setup(health, damage, speed, speed * 0.5f, skinColor);

        enemies.Add(bossEnemy);

        bossEnemy.OnDeath += () => enemies.Remove(bossEnemy);
        bossEnemy.OnDeath += () => Destroy(bossEnemy.gameObject, 2.5f);
        bossEnemy.OnDeath += () => GameManager.data.UpdateScore(50);
        bossEnemy.OnDeath += () => GameManager.data.UpdateEnemy(enemies.Count);
        bossEnemy.OnDeath += () => Instantiate(famasGunPrefab, bossEnemy.transform.position, bossEnemy.transform.rotation);
        bossEnemy.OnDeath += () => { if (enemies.Count <= 0) Instantiate(portalPrefab, portalPoint.position, portalPoint.rotation); };
    }
}
