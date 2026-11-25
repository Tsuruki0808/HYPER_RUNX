using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ErrorEntry
{
    public int errorCode;          //エラー番号
    [TextArea(2, 5)]
    public string message;         //エラーメッセージ
    public AudioClip voiceClip;    //対応する音声ファイル
}

public class ErrorManager : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private Text dialogueText; //UIのText
    public Image UIImage; //テキストボックス
    public Image UIIcon;  //キャラクターアイコン

    [Header("テキスト設定")]
    [SerializeField] private float typeSpeed = 0.05f; //文字が出る速さ

    [Header("エラー設定")]
    [SerializeField] private List<ErrorEntry> errorEntries = new List<ErrorEntry>();

    private Dictionary<int, ErrorEntry> errorDictionary = new Dictionary<int, ErrorEntry>();
    private Coroutine typingCoroutine;
    private AudioSource audioSource;

    void Start()
    {
        //AudioSourceを自動追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //UI初期化
        UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, 0f);
        UIIcon.color = new Color(UIIcon.color.r, UIIcon.color.g, UIIcon.color.b, 0f);
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0f);

        //List→Dictionaryに変換
        foreach (var entry in errorEntries)
        {
            if (!errorDictionary.ContainsKey(entry.errorCode))
            {
                errorDictionary.Add(entry.errorCode, entry);
            }
            else
            {
                Debug.LogWarning("同じエラー番号が重複しています: " + entry.errorCode);
            }
        }
    }

    void Update()
    {
        //テスト用（キーで切り替え）
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1)) AdvanceTutorial(1001);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AdvanceTutorial(1002);

        if (Input.GetKeyDown(KeyCode.Alpha3)) AdvanceTutorial(400);
        if (Input.GetKeyDown(KeyCode.Alpha4)) AdvanceTutorial(403);
        if (Input.GetKeyDown(KeyCode.Alpha5)) AdvanceTutorial(409);
        */
        }

    public void ShowDialogue(string message, AudioClip voiceClip)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(message));

        //音声を再生
        if (voiceClip != null)
        {
            audioSource.Stop();
            audioSource.clip = voiceClip;
            audioSource.Play();
        }
    }

    private IEnumerator TypeText(string message)
    {
        dialogueText.text = "";

        foreach (char c in message)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    //エラー番号で呼び出す
    public void AdvanceTutorial(int errorCode)
    {
        if (errorDictionary.TryGetValue(errorCode, out ErrorEntry entry))
        {
            ShowDialogue(entry.message, entry.voiceClip);

            UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, 0.7f);
            UIIcon.color = new Color(UIIcon.color.r, UIIcon.color.g, UIIcon.color.b, 1f);
            dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1f);
        }
        else
        {
            Debug.LogWarning("指定されたエラー番号が存在しません: " + errorCode);
        }
    }
}
