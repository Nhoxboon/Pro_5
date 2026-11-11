using TMPro;
using UnityEngine;

public class UICarSpeed : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI carSpeedTxt;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCarSpeedTxt();
    }

    private void LoadCarSpeedTxt()
    {
        if(carSpeedTxt != null) return;
        carSpeedTxt = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(transform.name + " LoadCarSpeedTxt", gameObject);
    }

    public void UpdateSpeedTxt(float speed)
    {
        carSpeedTxt.text = Mathf.RoundToInt(speed * 10) + " km/h";
    }
}
