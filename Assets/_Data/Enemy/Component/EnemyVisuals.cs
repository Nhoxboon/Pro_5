using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyVisuals : NhoxBehaviour
{
    [SerializeField] protected Enemy enemy;
    
    [Header("Weapon Visuals")] 
    [SerializeField] protected EnemyWeaponModel[] weaponModels;
    [SerializeField] protected EnemyMeleeWeaponType weaponType;
    public GameObject CurrentWeaponModel { get; protected set; }
    
    [Header("Corruption Visuals")]
    [SerializeField] protected GameObject[] corruptionCrystals;
    [SerializeField] protected int corruptionAmount;

    [Header("Color")] 
    [SerializeField] protected Texture[] colorTextures;
    [SerializeField] protected SkinnedMeshRenderer skinnedMR;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemy();
        LoadCorruptionCrystals();
        LoadWeaponModels();
        LoadSkinnedMeshRenderer();
    }

    protected void LoadEnemy()
    {
        if(enemy != null) return;
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

    protected void LoadWeaponModels()
    {
        if (weaponModels is { Length: > 0 }) return;
        weaponModels = transform.parent.GetComponentsInChildren<EnemyWeaponModel>(true);
        Debug.Log(transform.name + " LoadWeaponModels", gameObject);
    }

    protected void LoadSkinnedMeshRenderer()
    {
        if (skinnedMR != null) return;
        skinnedMR = transform.parent.GetComponentInChildren<SkinnedMeshRenderer>();
        Debug.Log(transform.name + " LoadSkinnedMeshRenderer", gameObject);
    }

    public void SetupWeaponType(EnemyMeleeWeaponType type) => weaponType = type;

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCrystal();
    }

    protected void SetupRandomWeapon()
    {
        foreach (var model in weaponModels)
            model.gameObject.SetActive(false);

        List<EnemyWeaponModel> filteredModels = weaponModels.Where(model => model.weaponType == weaponType).ToList();

        int randomIndex = Random.Range(0, filteredModels.Count);
        CurrentWeaponModel = filteredModels[randomIndex].gameObject;
        CurrentWeaponModel.SetActive(true);

        OverrideAnimatorController();
    }

    protected void OverrideAnimatorController()
    {
        AnimatorOverrideController overrideController =
            CurrentWeaponModel.GetComponent<EnemyWeaponModel>().overrideController;
        if (overrideController != null)
            enemy.EnemyAnimation.Animator.runtimeAnimatorController = overrideController;
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

    public void EnableWeaponTrail(bool enable)
    {
        if(CurrentWeaponModel.TryGetComponent(out EnemyWeaponModel weaponModel))
            weaponModel.EnableTrail(enable);
    }
}