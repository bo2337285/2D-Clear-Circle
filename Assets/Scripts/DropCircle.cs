using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCircle : Circle {
    protected override void Start () {
        base.Start ();
    }
    protected override void Update () {
        base.Update ();
    }
    void Combine (Vector3 pos) {
        GameManager.Instance.CombineCircle (pos, level);
    }

    private void OnCollisionEnter2D (Collision2D other) {
        if (!other.gameObject || this.isCombined) return;
        Circle _circle = other.gameObject.GetComponent<Circle> ();
        if (_circle == null) return;
        if (_circle.level == this.level) {
            _circle.isCombined = true;
            this.isCombined = true;
            Combine (other.contacts[0].point);
        }
    }
}