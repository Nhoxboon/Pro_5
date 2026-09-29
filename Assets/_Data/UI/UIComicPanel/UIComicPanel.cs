using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIComicPanel : NhoxBehaviour, IPointerDownHandler
{
    [SerializeField] protected Image[] comicPanel;
    [SerializeField] protected int imageIndex;
    [SerializeField] protected PlayGameBtn playBtn;

    protected Image myImage;
    [SerializeField] protected bool comicShowOver;

    protected override void Start()
    {
        base.Start();
        myImage = GetComponent<Image>();
        ShowNextImage();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayBtn();
    }

    protected void LoadPlayBtn()
    {
        if(playBtn != null) return;
        playBtn = GetComponentInChildren<PlayGameBtn>();
        Debug.Log(transform.name + " :LoadPlayBtn", gameObject);
    }

    protected void ShowNextImage()
    {
        if(comicShowOver) return;
        StartCoroutine(ChangeImageAlpha(1, 1.5f, ShowNextImage));
    }

    protected IEnumerator ChangeImageAlpha(float targetAlpha, float duration, Action onComplete)
    {
        float time = 0;
        Color currentColor = comicPanel[imageIndex].color;
        float startAlpha = currentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            comicPanel[imageIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        comicPanel[imageIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        imageIndex++;
        if (imageIndex >= comicPanel.Length)
            EnablePlayBtn();

        onComplete?.Invoke();
    }

    protected void EnablePlayBtn()
    {
        StopAllCoroutines();
        comicShowOver = true;
        playBtn.gameObject.SetActive(true);
        myImage.raycastTarget = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowNextImageOnClick();
    }

    protected void ShowNextImageOnClick()
    {
        if(imageIndex >= comicPanel.Length)
            EnablePlayBtn();
        if(comicShowOver)
            return;

        comicPanel[imageIndex].color = Color.white;
        imageIndex++;

        ShowNextImage();
    }
}
