using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyVisuals : NhoxBehaviour
{
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

    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(SetupLook), 0, 1.5f);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCorruptionCrystals();
        LoadWeaponModels();
        LoadSkinnedMeshRenderer();
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
}