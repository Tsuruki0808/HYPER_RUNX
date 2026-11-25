using UnityEngine;
using System.Collections.Generic;

public class MoveToCenter : MonoBehaviour
{
    private float moveSpeed = 3500f;
    private float fadeSpeed = 8f;
    private bool isAtCenter = false;

    private List<RectTransform> allObjects = new List<RectTransform>();
    private List<RectTransform> uiObjects = new List<RectTransform>();

    private Dictionary<RectTransform, Vector2> originalPositions = new Dictionary<RectTransform, Vector2>();
    private Dictionary<RectTransform, Vector2> targets = new Dictionary<RectTransform, Vector2>();
    private Dictionary<RectTransform, CanvasGroup> canvasGroups = new Dictionary<RectTransform, CanvasGroup>();
    private Dictionary<RectTransform, float> targetAlphas = new Dictionary<RectTransform, float>();

    //ボタン、UI参照
    public Transform leftButton;
    public Transform rightButton;
    public GameObject returnUI;
    public GameObject backgroundOverlay; 

    Vector3 baseScale;

    public float MaxPage = 3;
    private float currentPage = 1;

    public float buttonScaleUp = 0.5f;
    public float scaleDuration = 0.2f;

    //背景のCanvasGroup
    private CanvasGroup bgCanvasGroup;

    void Start()
    {
        foreach (Transform child in transform)
        {
            //ReturnUI・背景 は位置制御リストに含めない
            if (returnUI != null && child.gameObject == returnUI) continue;
            if (backgroundOverlay != null && child.gameObject == backgroundOverlay) continue;

            RectTransform rt = child.GetComponent<RectTransform>();
            if (rt != null)
            {
                allObjects.Add(rt);
                originalPositions[rt] = rt.anchoredPosition;
                targets[rt] = rt.anchoredPosition;

                CanvasGroup cg = rt.GetComponent<CanvasGroup>();
                if (cg == null) cg = rt.gameObject.AddComponent<CanvasGroup>();
                cg.alpha = 0f;
                canvasGroups[rt] = cg;
                targetAlphas[rt] = 0f;

                if (child.name.StartsWith("UI("))
                {
                    uiObjects.Add(rt);
                }

                MaxPage = uiObjects.Count;
            }
        }

        //初期状態はすべて透明
        foreach (RectTransform rt in allObjects)
        {
            if (canvasGroups.ContainsKey(rt))
                canvasGroups[rt].alpha = 0f;
        }

        baseScale = rightButton.localScale;

        //ReturnUI初期非表示
        if (returnUI != null)
            returnUI.SetActive(false);

        //背景初期設定
        if (backgroundOverlay != null)
        {
            bgCanvasGroup = backgroundOverlay.GetComponent<CanvasGroup>();
            if (bgCanvasGroup == null) bgCanvasGroup = backgroundOverlay.AddComponent<CanvasGroup>();
            bgCanvasGroup.alpha = 0f;
            backgroundOverlay.SetActive(false);
        }
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReturnUI();
        }*/

        if (isAtCenter)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) MoveLeft();
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) MoveRight();

            if (leftButton != null)
                leftButton.gameObject.SetActive(currentPage > 1);

            if (rightButton != null)
                rightButton.gameObject.SetActive(currentPage < MaxPage);

            if (returnUI != null)
                returnUI.SetActive(true);
        }
        else
        {
            if (returnUI != null)
                returnUI.SetActive(false);
        }

        //位置、透明度補間処理
        foreach (RectTransform rt in allObjects)
        {
            if (targets.ContainsKey(rt))
            {
                if (canvasGroups.ContainsKey(rt))
                {
                    CanvasGroup cg = canvasGroups[rt];
                    cg.alpha = Mathf.MoveTowards(cg.alpha, targetAlphas[rt], fadeSpeed * Time.deltaTime);

                    if (cg.alpha <= 0.01f)
                    {
                        cg.interactable = false;
                        cg.blocksRaycasts = false;
                    }
                    else
                    {
                        cg.interactable = true;
                        cg.blocksRaycasts = true;
                    }

                    if (targetAlphas[rt] == 0f)
                    {
                        Vector2 pos = rt.anchoredPosition;
                        pos.y = Mathf.MoveTowards(pos.y, 0, fadeSpeed * Time.deltaTime * 50f);
                        rt.anchoredPosition = pos;

                        if (Mathf.Approximately(cg.alpha, 0f))
                        {
                            rt.anchoredPosition = originalPositions[rt];
                        }
                    }
                    else
                    {
                        rt.anchoredPosition = Vector2.MoveTowards(
                            rt.anchoredPosition,
                            targets[rt],
                            moveSpeed * Time.deltaTime
                        );
                    }
                }
            }
        }

        //背景フェード制御
        if (bgCanvasGroup != null)
        {
            float target = isAtCenter ? 0.5f : 0f; //半透明黒
            bgCanvasGroup.alpha = Mathf.MoveTowards(bgCanvasGroup.alpha, target, fadeSpeed * Time.deltaTime);
            backgroundOverlay.SetActive(bgCanvasGroup.alpha > 0.01f);
        }
    }

    //ページ移動
    public void MoveLeft()
    {
        if (currentPage - 1 > 0)
        {
            foreach (RectTransform rt in uiObjects)
            {
                Vector2 basePos = targets.ContainsKey(rt) ? targets[rt] : rt.anchoredPosition;
                targets[rt] = basePos + new Vector2(+750, 0);
            }
            currentPage--;
            if (leftButton != null) StartCoroutine(ScaleButton(leftButton));
        }
    }

    public void MoveRight()
    {
        if (currentPage + 1 <= MaxPage)
        {
            foreach (RectTransform rt in uiObjects)
            {
                Vector2 basePos = targets.ContainsKey(rt) ? targets[rt] : rt.anchoredPosition;
                targets[rt] = basePos + new Vector2(-750, 0);
            }
            currentPage++;
            if (rightButton != null) StartCoroutine(ScaleButton(rightButton));
        }
    }

    //UIを中央に表示 / 元に戻す
    public void ReturnUI()
    {
        if (!isAtCenter)
        {
            foreach (RectTransform rt in allObjects)
            {
                Vector2 current = rt.anchoredPosition;
                targets[rt] = new Vector2(current.x, 0);
                targetAlphas[rt] = 1f;
            }
            isAtCenter = true;

            if (backgroundOverlay != null)
                backgroundOverlay.SetActive(true);
        }
        else
        {
            foreach (RectTransform rt in allObjects)
            {
                targets[rt] = originalPositions[rt];
                targetAlphas[rt] = 0f;
            }
            isAtCenter = false;
            currentPage = 1;
        }
    }

    private System.Collections.IEnumerator ScaleButton(Transform button)
    {
        button.localScale = baseScale;
        Vector3 targetScale = baseScale * buttonScaleUp;

        float t = 0;
        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            button.localScale = Vector3.Lerp(baseScale, targetScale, t / scaleDuration);
            yield return null;
        }

        t = 0;
        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            button.localScale = Vector3.Lerp(targetScale, baseScale, t / scaleDuration);
            yield return null;
        }
    }
}
