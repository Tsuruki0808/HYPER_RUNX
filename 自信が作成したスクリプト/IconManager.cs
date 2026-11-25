using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [Header("設定する画像")]
    public Sprite iconCow;
    public Sprite iconmyu;
    public Sprite icongirl;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("UI Image コンポーネントがアタッチされていません！");
        }
    }

    ///名前に応じてアイコンを変更
    public void SetIcon(string name)
    {
        Debug.Log("SetIcon 呼ばれた: " + name);


        if (name.Contains("Cowboy"))
        {
            image.sprite = iconCow;
        }
        else if (name.Contains("Mutant"))
        {
            image.sprite = iconmyu;
        }
        else if (name.Contains("girl"))
        {
            image.sprite = icongirl;
        }

    }
}