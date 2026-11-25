using UnityEngine;

public class MoveToCenterToggle : MonoBehaviour
{
    [Header("MoveToCenter がアタッチされているオブジェクト")]
    public GameObject moveToCenterObject;

    private MoveToCenter moveToCenterScript;

    private void Start()
    {
        if (moveToCenterObject != null)
        {
            moveToCenterScript = moveToCenterObject.GetComponent<MoveToCenter>();
            if (moveToCenterScript == null)
            {
                Debug.LogWarning("MoveToCenter が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning("moveToCenterObject が設定されていません。");
        }
    }

    //UIを表示する
    public void ShowUI()
    {
        if (moveToCenterObject != null)
            moveToCenterObject.SetActive(true);

        if (moveToCenterScript != null)
            moveToCenterScript.ReturnUI(); //MoveToCenterの元メソッドを呼ぶ

        Debug.Log("MoveToCenter の UI を表示");
    }

    //UIを非表示にする
    public void HideUI()
    {
        if (moveToCenterObject != null)
            moveToCenterObject.SetActive(false);

        Debug.Log("MoveToCenter の UI を非表示");
    }
}
