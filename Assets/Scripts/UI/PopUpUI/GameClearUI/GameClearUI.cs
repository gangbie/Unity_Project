using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearUI : PopUpUI
{
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private AudioSource knifeSound;
    [SerializeField] private AudioSource bgmSound;
    protected override void Awake()
    {
        base.Awake();
        buttons["OkButton"].onClick.AddListener(() => { GoToMainScene(); });
    }

    public void GoToMainScene()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.Scene.LoadScene("MainScene");
    }

    private void GunSoundPlay()
    {
        gunSound.Play();
    }
    private void KnifeSoundPlay()
    {
        knifeSound.Play();
    }
    private void BgmSoundPlay()
    {
        bgmSound.Play();
    }
}
