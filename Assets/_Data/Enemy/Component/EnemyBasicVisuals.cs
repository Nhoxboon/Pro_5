using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBasicVisuals : NhoxBehaviour
{
    [SerializeField] protected Enemy enemy;
    public GameObject CurrentWeaponModel { get; protected set; }

    [Header("Corruption Visuals")] [SerializeField]
    protected GameObject[] corruptionCrystals;

    [SerializeField] protected int corruptionAmount;

    [Header("Color")] [SerializeField] protected Texture[] colorTextures;
    [SerializeField] protected SkinnedMeshRenderer skinnedMR;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemy();
        LoadCorruptionCrystals();
        LoadSkinnedMeshRenderer();
    }

    protected void LoadEnemy()
    {
        if (enemy != null) return;
        enemy = GetComponentInParent<Enemy>();
        Debug.Log(transform.name + " LoadEnemy", gameObject);
    }

    protected void LoadCorruptionCrystals()
    {
        if (corruptionCrystals is { Length: > 0 }) return;
        EnemyCrystal[] enemyCrystals = transform.parent.GetComponentsInChildren<EnemyCrystal>(true);
        corruptionCrystals = new GameObject[enemyCrystals.Length];
        for (int i = 0; i < enemyCrystals.Length; i++)
            corruptionCrystals[i] = enemyCrystals[i].gameObject;
        Debug.Log(transform.name + " LoadCorruptionCrystals", gameObject);
    }

    protected void LoadSkinnedMeshRenderer()
    {
        if (skinnedMR != null) return;
        skinnedMR = transform.parent.GetComponentInChildren<SkinnedMeshRenderer>();
        Debug.Log(transform.name + " LoadSkinnedMeshRenderer", gameObject);
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCrystal();
    }

    protected void SetupRandomWeapon()
    {
        bool thisEnemyIsMelee = enemy is EnemyMelee;
        bool thisEnemyIsRange = enemy is EnemyRange;
        if (thisEnemyIsMelee)
            CurrentWeaponModel = FindMeleeWeaponModel();
        if (thisEnemyIsRange)
            CurrentWeaponModel = FindRangeWeaponModel();

        CurrentWeaponModel?.SetActive(true);
        OverrideAnimatorController();
    }

    protected GameObject FindMeleeWeaponModel()
    {
        if (enemy is not EnemyMelee enemyMelee) return null;
        EnemyWeaponModel[] weaponModels = transform.parent.GetComponentsInChildren<EnemyWeaponModel>(true);
        foreach (var model in weaponModels)
            model.gameObject.SetActive(false);

        List<EnemyWeaponModel> filteredModels =
            weaponModels.Where(model => model.weaponType == enemyMelee.WeaponType).ToList();

        int randomIndex = Random.Range(0, filteredModels.Count);
        return filteredModels[randomIndex].gameObject;
    }

    protected virtual GameObject FindRangeWeaponModel() => null;

    protected void OverrideAnimatorController()
    {
        AnimatorOverrideController overrideController =
            CurrentWeaponModel.GetComponent<EnemyWeaponModel>()?.overrideController;
        if (overrideController != null)
            enemy.EnemyAnimator.Animator.runtimeAnimatorController = overrideController;
    }

    protected void SetupRandomCrystal()
    {
        List<int> availableIndex = new();
        for (int i = 0; i < corruptionCrystals.Length; i++)
        {
            availableIndex.Add(i);
            corruptionCrystals[i].SetActive(false);
        }

        for (int i = 0; i < corruptionAmount; i++)
        {
            if (availableIndex.Count == 0) break;
            int randomIndex = Random.Range(0, availableIndex.Count);
            int crystalIndex = availableIndex[randomIndex];
            corruptionCrystals[crystalIndex].SetActive(true);
            availableIndex.RemoveAt(randomIndex);
        }
    }

    protected void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);
        Material newMat = new Material(skinnedMR.material);
        newMat.mainTexture = colorTextures[randomIndex];
        skinnedMR.material = newMat;
    }
    
    public void EnableWeaponModel(bool active) => CurrentWeaponModel?.gameObject.SetActive(active);

    public void EnableWeaponTrail(bool enable)
    {
        if (CurrentWeaponModel.TryGetComponent(out EnemyWeaponModel weaponModel))
            weaponModel.EnableTrail(enable);
    }
}