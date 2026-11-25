using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToGoalManager : MonoBehaviour
{
    [Header("ステージの始点と終点")]
    private Vector3 startPos;  
    private Vector3 endPos;

    [Header("プレイヤー名（シーン内のオブジェクト名）")]
    public string Player1Name;
    public string Player2Name;

    private Transform Player;   //プレイヤー1
    private Transform Player2;  //プレイヤー2

    //UI関連
    private RectTransform uiBar;
    private RectTransform uiIcon;
    private RectTransform uiIcon2;

    //アイコンのYオフセット
    private float yOffsetPlayer1 = 15f;
    private float yOffsetPlayer2 = -15f;

    void Awake()
    {
        uiBar = transform.Find("UiBar")?.GetComponent<RectTransform>();//この上を動く
        uiIcon = transform.Find("UiIcon")?.GetComponent<RectTransform>();//実際に動くicon
        uiIcon2 = transform.Find("UiIcon2")?.GetComponent<RectTransform>();

        if (uiBar == null) Debug.LogWarning("UiBar が見つかりません！");
        if (uiIcon == null) Debug.LogWarning("UiIcon が見つかりません！");
        if (uiIcon2 == null) Debug.LogWarning("UiIcon2 が見つかりません！");
    }

    void Start()
    {
        //GoalのTransformを取得
        GameObject goalObj = GameObject.Find("GOAL");

        if (goalObj != null)
        {
            endPos = goalObj.transform.position;
        }
        else
        {
            Debug.LogWarning("GOAL オブジェクトが見つかりません");
        }

    }

    void Update()
    {
        for (int i = 0; i <= 2; i++)
        {
            KeyCode key = KeyCode.Alpha0 + i; //0ならAlpha0, 1ならAlpha1
            if (Input.GetKeyDown(key))
            {
                receivePartnerNumber(i);

            }
        }
        //プレイヤーの検索
        if (Player == null && !string.IsNullOrEmpty(Player1Name))
        {
            var obj = GameObject.Find(Player1Name);
            if (obj)
            {
                Player = obj.transform;
                Debug.Log("プレイヤー1が見つかった");
            }
        }

        if (Player2 == null && !string.IsNullOrEmpty(Player2Name))
        {
            var obj = GameObject.Find(Player2Name);
            if (obj)
            {
                Player2 = obj.transform;
                Debug.Log("プレイヤー2が見つかった");

            }

        }
        //UI更新できない条件
        if (startPos == null || endPos == null || uiBar == null) return;



        //UIバーの左右端（AnchoredPosition と SizeDelta で計算
        Vector3 barLeftLocal =
            new Vector3(uiBar.anchoredPosition.x - uiBar.sizeDelta.x * 1.5f,
                        uiBar.anchoredPosition.y,
                        0f);

        Vector3 barRightLocal =
            new Vector3(uiBar.anchoredPosition.x + uiBar.sizeDelta.x * 1.5f,
                        uiBar.anchoredPosition.y,
                        0f);

        //親のRectTransform座標 → ワールド座標へ変換
        Vector3 barLeftWorld = uiBar.parent.TransformPoint(barLeftLocal);
        Vector3 barRightWorld = uiBar.parent.TransformPoint(barRightLocal);

        //アイコン更新
        if (Player != null && uiIcon != null)
        {
            float progress1 = Mathf.Clamp01(
                (Player.position.z - startPos.z) /
                (endPos.z - startPos.z)
            );

            Vector3 iconPosWorld = Vector3.Lerp(barLeftWorld, barRightWorld, progress1);
            Vector3 iconLocal = uiIcon.parent.InverseTransformPoint(iconPosWorld);
            iconLocal.y += yOffsetPlayer1;

            uiIcon.localPosition = iconLocal;
        }

        if (Player2 != null && uiIcon2 != null)
        {
            float progress2 = Mathf.Clamp01(
                (Player2.position.z - startPos.z) /
                (endPos.z - startPos.z)
            );

            Vector3 iconPosWorld = Vector3.Lerp(barLeftWorld, barRightWorld, progress2);
            Vector3 iconLocal = uiIcon2.parent.InverseTransformPoint(iconPosWorld);
            iconLocal.y += yOffsetPlayer2;

            uiIcon2.localPosition = iconLocal;
        }

        //Debug.Log($"Start Z: {startPos.z}, Player Z: {Player.position.z}, Goal Z: {endPos.z}");

    }



    //外部から受け取るデータ
    public void receiveData(string name, Transform pos)
    {
        if (name.Contains("(Clone)")) { return; } 

            Player1Name = name;

        if (startPos == Vector3.zero)
            startPos = pos.position;  //座標だけコピーする


        //"UiIcon" という名前のオブジェクトを探す
        GameObject obj = GameObject.Find("UiIcon");

        if (obj != null)
        {
            IconManager icon = obj.GetComponent<IconManager>();
            if (icon != null)
            {
                icon.SetIcon(Player1Name); 
            }

        }
    }

    public void receivePlayer2Name()
    {

        //"UiIcon" という名前のオブジェクトを探す
        GameObject obj = GameObject.Find("UiIcon2");

        if (obj != null)
        {
            IconManager icon2 = obj.GetComponent<IconManager>();

            if (icon2 != null)
            {
                icon2.SetIcon(Player2Name); 
            }
        }
    }

    public void receivePartnerNumber(int num)
    {
        string[] names = { "Cowboy", "Mutant", "girl" };
        Player2Name = names[num];
        Player2Name += "(Clone)";

        receivePlayer2Name();

    }

}
