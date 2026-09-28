using TMPro;
using UnityEngine;

public class UIGameOver : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI gameOverTxt;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGameOverTxt();
    }

    protected void LoadGameOverTxt()
    {
        if(gameOverTxt != null) return;
        gameOverTxt = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(transform.name + ": LoadGameOverTxt", gameObject);
    }

    public void ShowGameOverMessage(string msg) => gameOverTxt.text = msg;
}
