using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoUI : SceneUI
{
    private LivingEntity player;
    private Gun gun;
    public int life;
    public int score;
    public float hp;
    public int bullet;

    protected override void Awake()
    {
        base.Awake();

        life = GameManager.data.Life;
        hp = player.health;
        score = GameManager.data.Score;
        bullet = GameManager.data.Bullet;
    }
}
