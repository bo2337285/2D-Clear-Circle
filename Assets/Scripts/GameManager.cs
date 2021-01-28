using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    GameObject circleGroupGo;
    Camera mainCamera;
    public GameObject CircleGo;
    public SpriteRenderer bgGoRender;
    public TMP_Text scoreText, comboInfoText;
    public GameObject resultDialog;
    public GameObject comboInfo;
    public List<Vector2> windowPoints = new List<Vector2> ();
    public float maxLevel = 10f;
    public int maxSizeScale = 1, maxRandomLevel = 5;
    public Color minColor, maxColor, comboColor;
    Color bgRenderSourceColor;
    public int score = 0;
    [SerializeField] float comboScore = 0;
    public float timer = 0f;
    public float comboInterval = 0.5f;
    bool isGameOver, isComboing, isComboStarted;

    private void Awake () {
        Instance = this;
    }

    void Start () {
        Time.timeScale = 1;
        mainCamera = Camera.main;
        windowPoints.Add (mainCamera.ViewportToWorldPoint (new Vector2 (0, 1))); // 左上
        windowPoints.Add (mainCamera.ViewportToWorldPoint (new Vector2 (1, 1))); // 右上
        windowPoints.Add (mainCamera.ViewportToWorldPoint (new Vector2 (1, 0))); // 右下
        windowPoints.Add (mainCamera.ViewportToWorldPoint (new Vector2 (0, 0))); // 左下
        circleGroupGo = new GameObject ("CircleGroup");
        bgRenderSourceColor = bgGoRender.color;
    }

    void Update () {
        ShowResult ();
        ListenInput ();
        CheckCombo ();
        UpdateScoreText ();
    }

    void UpdateScoreText () {
        scoreText.text = score.ToString ();
    }

    void CheckCombo () {
        if (timer > Time.deltaTime) {
            timer -= Time.deltaTime;
            isComboing = true;
        } else {
            timer = 0f;
            isComboing = false;
        }
        if (isComboing) {
            comboScore += 2 * Time.deltaTime;
        } else if (isComboStarted) {
            int totalComboScore = (int) Mathf.Floor (comboScore);
            score += totalComboScore;
            comboScore = 0f;
            isComboStarted = false;
            comboInfoText.text = totalComboScore.ToString ();
            StartCoroutine (ShowComboInfo ());

        }
        bgGoRender.color = Color.Lerp (bgRenderSourceColor, comboColor, (timer / comboInterval));
    }
    IEnumerator ShowComboInfo () {
        comboInfo.SetActive (true);
        yield return new WaitForSeconds (1f);
        comboInfo.SetActive (false);
    }

    public void GameOver () {
        isGameOver = true;
        Time.timeScale = 0;
    }

    public void ReStart () {
        Time.timeScale = 1;
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
    void ShowResult () {
        if (!isGameOver) return;
        resultDialog.SetActive (true);
    }

    void ListenInput () {
        if (isGameOver) return;
        if (Input.GetMouseButtonDown (0) && !IsTouchedUI () && IsMouseInScreen () && !DeathArea.Instance.isTouching) {
            AddTopCircle ();
        }
    }

    IEnumerator AddCircle (Vector3 pos, int level) {
        GameObject _circle = Instantiate (CircleGo, pos, Quaternion.identity, circleGroupGo.transform);
        yield return new WaitForEndOfFrame ();
        _circle.GetComponent<Circle> ().UpdateLevel (level);
    }
    void AddTopCircle () {
        StartCoroutine (AddCircle (GetTopMouseXPos (), SampleCircle.Instance.level));
        SampleCircle.Instance.GetRandomCircle ();
    }
    public void CombineCircle (Vector3 pos, int level) {
        StartCoroutine (AddCircle (pos, level + 1));
        score++;
        // 重置连击时间
        timer = comboInterval;
        // 开始连击开关
        if (!isComboStarted) isComboStarted = true;
    }

    Vector2 GetTopMouseXPos () {
        Vector2 pos = mainCamera.ScreenToWorldPoint (Input.mousePosition);
        return new Vector2 (pos.x, windowPoints[0].y);
    }
    bool IsMouseInScreen () {
        Vector2 pos = mainCamera.ScreenToWorldPoint (Input.mousePosition);
        return (pos.x >= windowPoints[0].x &&
            pos.x <= windowPoints[1].x &&
            pos.y >= windowPoints[2].y &&
            pos.y <= windowPoints[1].y);

    }
    // 鼠标事件防穿透
    bool IsTouchedUI () {
        bool touchedUI = false;
        if (Application.isMobilePlatform) {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId)) {
                touchedUI = true;
            }
        } else if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ()) {
            touchedUI = true;
        }
        return touchedUI;
    }
}