using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Circle : MonoBehaviour {
    public bool isCombined = false;
    public SpriteRenderer spriteRenderer;
    public int level = 0;
    public TMP_Text text;
    protected virtual void Start () {
        spriteRenderer = GetComponent<SpriteRenderer> ();
        UpdateRenderByType (this.level);
    }
    protected virtual void Update () {
        // UpdateRenderByType ();
        if (this.isCombined) {
            gameObject.SetActive (false);
        }
    }
    public void UpdateLevel (int level) {
        this.level = level;
        UpdateRenderByType (level);
        text.text = level.ToString ();
    }

    void UpdateRenderByType (int level) {
        spriteRenderer.color = GetColor (level);
        transform.localScale = Vector3.one * Mathf.Lerp (1, GameManager.Instance.maxSizeScale, (level / GameManager.Instance.maxLevel));
    }
    Color GetColor (int level) {
        // 踩坑, Lerp和Color千万不要用int
        return Color.Lerp (GameManager.Instance.minColor, GameManager.Instance.maxColor, (level / GameManager.Instance.maxLevel));
    }
}