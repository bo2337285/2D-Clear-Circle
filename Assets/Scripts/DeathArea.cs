using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour {
    public static DeathArea Instance;
    public GameObject sampleCircle;
    public float maxTimerSeconds = 2f;
    public bool isTouching;
    public float timer = 0f;
    public float clearTimerSeconds = 0.5f;
    public Color minColor, maxColor;
    SpriteRenderer spriteRenderer;
    private void Awake () {
        Instance = this;
    }
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }
    private void Update () {
        if (isTouching) {
            timer += Time.deltaTime;
        } else {
            if (timer > Time.deltaTime) {
                timer -= Time.deltaTime;
            } else {
                timer = 0f;
            }
        }
        spriteRenderer.color = Color.Lerp (minColor, maxColor, (timer / maxTimerSeconds));
        if (timer >= maxTimerSeconds) {
            GameManager.Instance.GameOver ();
        }
    }

    void OnTriggerStay2D (Collider2D other) {
        if (other.tag != sampleCircle.tag) return;
        isTouching = true;
    }
    void OnTriggerExit2D (Collider2D other) {
        if (other.tag != sampleCircle.tag) return;
        isTouching = false;
    }

}