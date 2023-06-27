using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();

    public float damageMax = 40f; // 최대 공격력
    public float damageMin = 20f; // 최소 공격력
    public Enemy enemyPrefab; // 생성할 적 AI

    public float healthMax = 200f; // 최대 체력
    public float healthMin = 100f; // 최소 체력

    public Transform[] spawnPoints; // 적 AI를 소환할 위치들

    public float speedMax = 12f; // 최대 속도
    public float speedMin = 3f; // 최소 속도
}
