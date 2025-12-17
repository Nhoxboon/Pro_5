using System;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public SnapPointType pointType;

    protected void OnValidate()
    {
        gameObject.name = "SnapPoint " + pointType.ToString();
    }
}