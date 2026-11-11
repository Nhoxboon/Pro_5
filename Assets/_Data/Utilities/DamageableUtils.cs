using UnityEngine;

public static class DamageableUtils
{
    public static bool TryGetDamageable(this Component source, out IDamageable damageable)
    {
        damageable = source.GetComponentInParent<IDamageable>();

        if (damageable != null)
            return true;

        damageable = source.transform.root.GetComponentInChildren<IDamageable>(true);
        return damageable != null;
    }
}
