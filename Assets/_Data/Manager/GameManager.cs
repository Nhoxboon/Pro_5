using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NhoxSingleton<GameManager>
{
    public bool friendlyFire;

    public bool quickStart;

    public void GameStart()
    {
        SetDefaultWeapons();
        // LevelGenerator.Instance.InitializeGeneration();
        //Start selected mission in LevelGenerator script after we're done with level generation
    }

    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void GameCompleted()
    {
        UIManager.Instance.ShowVictoryScreen();
        ControlsManager.Instance.Controls.Player.Disable();
        PlayerCtrl.Instance.Core.Stats.Health.SetInvincible(true);
    }

    public void GameOver()
    {
        TimeManager.Instance.SlowMotionIn(1.5f);
        UIManager.Instance.ShowGameOverUI();
        CameraManager.Instance.ChangeCameraDistance(5);
    }

    private void SetDefaultWeapons()
    {
        List<WeaponDataSO> newList = UIManager.Instance.UIWeaponSel.SelectedWeaponsData();
        if (newList.Count == 0) return;
        PlayerCtrl.Instance.PlayerInventory.SetDefaultWeapons(newList);
    }
}