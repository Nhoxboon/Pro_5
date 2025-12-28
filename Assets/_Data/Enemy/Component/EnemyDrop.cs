using UnityEngine;

public class EnemyDrop : NhoxBehaviour
{
    [SerializeField] protected GameObject missionObjKey;

    public void GiveKey(GameObject newKey) => missionObjKey = newKey;

    public void DropItems()
    {
        if(missionObjKey != null)
            CreateItem(missionObjKey);
    }

    protected void CreateItem(GameObject go)
    {
        //TODO: Replace with object pooling later
        GameObject newItem = Instantiate(go, transform.parent.position + Vector3.up, Quaternion.identity);
    }
}
