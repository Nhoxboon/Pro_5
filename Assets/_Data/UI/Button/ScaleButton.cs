using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ScaleButton : BaseBtn, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Settings")] [SerializeField]
    protected float scaleSpeed = 3f;

    [SerializeField] protected float scaleRate = 1.15f;

    protected Vector3 defaultScale;
    protected Vector3 targetScale;

    [SerializeField] protected Image buttonImage;
    [SerializeField] protected TextMeshProUGUI buttonText;

    protected override void OnDisable()
    {
        base.OnDisable();
        ResetBtn();
    }

    protected override void Start()
    {
        base.Start();
        defaultScale = transform.localScale;
        targetScale = defaultScale;
    }

    protected virtual void Update()
    {
        if (Mathf.Abs(transform.localScale.x - targetScale.x) > 0.01f)
        {
            float scaleValue = Mathf.Lerp(transform.localScale.x, targetScale.x, scaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBtnImage();
        LoadBtnTxt();
    }

    protected void LoadBtnImage()
    {
        if (buttonImage != null) return;
        buttonImage = GetComponent<Image>();
        Debug.Log(transform.name + " :LoadBtnImage", gameObject);
    }

    protected void LoadBtnTxt()
    {
        if (buttonText != null) return;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(transform.name + " :LoadBtnTxt", gameObject);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = defaultScale * scaleRate;
        ChangeBtnColor(Color.yellow);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        targetScale = defaultScale;
        ChangeBtnColor(Color.white);
    }

    protected void ChangeBtnColor(Color newColor)
    {
        if (buttonImage != null)
            buttonImage.color = newColor;
        if (buttonText != null)
            buttonText.color = newColor;
    }

    protected virtual void ResetBtn()
    {
        targetScale = defaultScale;
        transform.localScale = defaultScale;
        ChangeBtnColor(Color.white);
    }
}