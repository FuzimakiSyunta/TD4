using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public PlayerColorChenge playerColorChengeScript; // プレイヤーのカラーチェンジスクリプト
    public GameObject playerColorChenge; // プレイヤーのカラーチェンジオブジェクト

    [Header("フェードに使うイメージ")]
    public Image fadeImage;

    [Header("次のシーン名")]
    public string nextSceneName;

    [Header("フェード速度")]
    public float fadeSpeed = 1f;

    private float fadeAlpha = 0f;
    private bool isFading = false;

    //joy-conのAボタンが前に押されていたか記録するフラグ
    private bool AButtonFlag = false;

    private bool hasStartedLoading = false;

    void Start()
    {
        // PlayerColorChengeスクリプトの参照を取得
        if (playerColorChenge != null)
        {
            playerColorChengeScript = playerColorChenge.GetComponent<PlayerColorChenge>();
        }
        if (fadeImage != null)
        {
            // 最初は透明にして非表示に
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 0f);
            fadeImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //Joy-conが押された瞬間判定
        bool AButtonState = JCScript.Instance.RightAButton;

        // Enterが押されたらフェード開始（押した瞬間1回のみ）
        if (!hasStartedLoading)
        {
            if ((Input.GetKeyDown(KeyCode.Return) && playerColorChengeScript.IsSelected() && !isFading) ||
                (AButtonState && !AButtonFlag && playerColorChengeScript.IsSelected()))
            {
                StartFadeAndLoad();
            }
        }
        AButtonFlag = AButtonState;

        // フェード処理中
        if (isFading && fadeImage != null)
        {
            fadeAlpha += fadeSpeed * Time.deltaTime;
            fadeAlpha = Mathf.Clamp01(fadeAlpha);
            fadeImage.color = new Color(0, 0, 0, fadeAlpha);

            if (fadeAlpha >= 1f)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    void StartFadeAndLoad()
    {
        if (fadeImage != null)
        {
            fadeAlpha = 0f;
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);
            isFading = true;
        }
        else
        {
            // フェード無しで即シーン切り替え
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
