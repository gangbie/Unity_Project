using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();

    public float damageMax = 40f; // �ִ� ���ݷ�
    public float damageMin = 20f; // �ּ� ���ݷ�
    public Enemy enemyPrefab; // ������ �� AI

    public float healthMax = 200f; // �ִ� ü��
    public float healthMin = 100f; // �ּ� ü��

    public Transform[] spawnPoints; // �� AI�� ��ȯ�� ��ġ��

    public float speedMax = 12f; // �ִ� �ӵ�
    public float speedMin = 3f; // �ּ� �ӵ�
}
