using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("MoveToCenter がアタッチされているオブジェクト")]
    public GameObject moveToCenterObject; //インスペクターで指定

    [Header("表示したい Image オブジェクト")]
    public GameObject imageObject; //インスペクターで指定（最初は非表示）

    private MoveToCenter moveToCenterScript;

    private void Start()
    {
        //不要オブジェクト破棄処理
        var obj = GameObject.Find("APIManager");
        if (obj != null)
        {
            Destroy(obj);
            Debug.Log("APIManager を破棄しました");
        }

        obj = GameObject.Find("WebSocket");
        if (obj != null)
        {
            Destroy(obj);
            Debug.Log("WebSocket を破棄しました");
        }

        //MoveToCenterスクリプトを取得
        if (moveToCenterObject != null)
        {
            moveToCenterScript = moveToCenterObject.GetComponent<MoveToCenter>();
            if (moveToCenterScript == null)
            {
                Debug.LogWarning("指定されたオブジェクトに MoveToCenter スクリプトが見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning("moveToCenterObject がインスペクターで設定されていません。");
        }

        //Imageオブジェクトを非表示でスタート
        if (imageObject != null)
        {
            imageObject.SetActive(false);
        }
    }

    void Update()
    {
        /*
        if (Input.anyKeyDown) //どのキーでも押された瞬間
        {
            SceneManager.LoadScene("niikawa");
            Debug.Log("s");
        }
        */
    }

    //シーン遷移関数
    public void GoGame(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ShowImage()
    {
        if (imageObject != null)
        {
            imageObject.SetActive(true);
            Debug.Log("Imageオブジェクトを表示しました。");
        }
        else
        {
            Debug.LogWarning("imageObject が設定されていません。");
        }
    }

    //MoveToCenterのUI復帰関数
    public void viewTutorial()
    {
        if (moveToCenterScript != null)
        {
            moveToCenterScript.ReturnUI();
            Debug.Log("ReturnUI() を呼び出しました。");
        }
        else
        {
            Debug.LogWarning("MoveToCenter スクリプトが設定されていないため、ReturnUI() を呼び出せません。");
        }
    }
}
