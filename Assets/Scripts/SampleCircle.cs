using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCircle : Circle {
    public static SampleCircle Instance;

    private void Awake () {
        Instance = this;
    }
    protected override void Start () {
        base.Start ();
        GetRandomCircle ();
    }
    protected override void Update () {
        base.Update ();
    }

    public void GetRandomCircle () {
        // Random.Range用int则会左开右闭,想获取到边缘最好边缘+0.5f再转int
        int _level = (int) Random.Range (0f, GameManager.Instance.maxRandomLevel + 0.5f);
        // while (_level == level) {
        //     _level = (int) Random.Range (0f, GameManager.Instance.maxRandomLevel + 0.5f);
        // }
        UpdateLevel (_level);
    }

}