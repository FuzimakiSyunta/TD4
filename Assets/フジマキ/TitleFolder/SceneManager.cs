using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    //operationFadeのスクリプト
    private OperationFade OperationFadeScript;
    public GameObject OperationFade;

    public string[] nextSceneName;
    public Transform[] movingObjects;
    //シーン移動先フラグ
    public bool GameStartFlag = false;
    public bool ColorChengeFlag = false;
    //public bool GameStartFlag = false;

    private float moveSpeed = 7000f;
    private float leftEndX = -1600f;
    public Image fadeImage;
    private float fadeSpeed = 1f;
    private float delayBetweenStarts = 0.2f; // それぞれ動き出す間隔（秒）

    private float[] startTimes; // 各オブジェクトが動き出す時間
    private float startTime;
    private bool isSequenceStarted = false;
    private bool isFading = false;
    private float fadeAlpha = 0f;

    private ImageSelector ImageSelectorScript;
    public GameObject ImageSelector;

    public GameObject selectBand;

    private bool wasRightAButtonPressed = false;

    void Start()
    {
        if (ImageSelector != null)
        {
            ImageSelectorScript = ImageSelector.GetComponent<ImageSelector>();
        }
        if (OperationFade != null)
        {
            OperationFadeScript = OperationFade.GetComponent<OperationFade>();
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 0f);
            fadeImage.gameObject.SetActive(false);
        }

        startTimes = new float[movingObjects.Length];
    }

    void Update()
    {
        if (ImageSelectorScript == null) return;

        float index = ImageSelectorScript.Imageindex();

        //Joy-ConのAボタンの現在の状態を取得
        bool isRightAButtonPressed = (JCScript.Instance != null) ? JCScript.Instance.RightAButton : false;

        // 0番目のImageが選択されている場合のみ処理を行う
        //ゲームスタート
        if ((index == 0 &&!OperationFadeScript.IsOperation()&&Input.GetKeyDown(KeyCode.Return) && !isSequenceStarted)|| JCScript.Instance.RightAButton)
        {
            GameStartFlag = true; // ← ここで設定
            isSequenceStarted = true;
            startTime = Time.time;

            for (int i = 0; i < movingObjects.Length; i++)
            {
                startTimes[i] = startTime + i * delayBetweenStarts;
            }

            if (fadeImage != null)
            {
                fadeImage.gameObject.SetActive(true);
            }
        }

        // 1番目のImageが選択されている場合のみ処理を行う
        //ゲームスタート
        if ((index == 1 && !OperationFadeScript.IsOperation() && Input.GetKeyDown(KeyCode.Return) && !isSequenceStarted)|| JCScript.Instance.RightAButton)
        {

            ColorChengeFlag = true; // ← ここで設定
            isSequenceStarted = true;
            startTime = Time.time;

            for (int i = 0; i < movingObjects.Length; i++)
            {
                startTimes[i] = startTime + i * delayBetweenStarts;
            }

            if (fadeImage != null)
            {
                fadeImage.gameObject.SetActive(true);
            }
            PlayerPrefs.Save();
        }
        wasRightAButtonPressed = isRightAButtonPressed;

        if (isSequenceStarted)
        {
            //bool allStarted = true;
            selectBand.SetActive(true);

            for (int i = 0; i < movingObjects.Length; i++)
            {
                if (Time.time >= startTimes[i])
                {
                    MoveObject(movingObjects[i]);
                }
                
            }

            // 全部動き始めた後、一定時間でフェード開始
            if ( !isFading && Time.time >= startTimes[startTimes.Length - 1] + 1f)
            {
                isFading = true;
            }
        }
        else
        {
            selectBand.SetActive(false);
        }

        if (isFading)
        {
            FadeOut();
        }
    }

    void MoveObject(Transform obj)
    {
        if (obj.position.x > leftEndX)
        {
            obj.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }
    //フェードアウト処理
    void FadeOut()
    {
        if (fadeImage == null) return;

        fadeAlpha += fadeSpeed * Time.deltaTime;
        fadeAlpha = Mathf.Clamp01(fadeAlpha);
        fadeImage.color = new Color(0, 0, 0, fadeAlpha);

        if (fadeAlpha >= 1f)
        {
            if (ColorChengeFlag)
            {
                SceneManager.LoadScene(nextSceneName[1]);
            }
            else if (GameStartFlag)
            {
                SceneManager.LoadScene(nextSceneName[0]);
            }
        }
    }
}
