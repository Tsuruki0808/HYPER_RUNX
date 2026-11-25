using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialStep
{
    [TextArea(2, 5)]
    public string message;
    public AudioClip voiceClip;
}

public class TutorialManager : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image UIImage;
    [SerializeField] private Image UIIcon;

    [Header("チュートリアル設定")]
    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private TutorialStep[] tutorialSteps; //テキスト＋音声の配列


    [SerializeField] GameObject Pliya_sutandado;
    Pliya_sutandado player;

    private Coroutine typingCoroutine;
    private AudioSource audioSource;

    void Start()
    {
        player = Pliya_sutandado.GetComponent<Pliya_sutandado>();

        //AudioSourceを自動追加
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        //最初は透明
        UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, 0f);
        UIIcon.color = new Color(UIIcon.color.r, UIIcon.color.g, UIIcon.color.b, 0f);
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AdvanceTutorial(0);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AdvanceTutorial(1);
        }
    }

    //チュートリアルを進める
    public void AdvanceTutorial(int number)
    {
        if (number >= 0 && number < tutorialSteps.Length)
        {
            ShowDialogue(tutorialSteps[number]);
        }
        else
        {
            Debug.LogWarning("指定された番号が範囲外です: " + number);
        }
    }

    //テキストと音声を表示
    private void ShowDialogue(TutorialStep step)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, 0.7f);
        UIIcon.color = new Color(UIIcon.color.r, UIIcon.color.g, UIIcon.color.b, 1f);
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1f);

        //音声再生
        if (step.voiceClip != null)
        {
            audioSource.Stop();
            audioSource.clip = step.voiceClip;
            audioSource.Play();
        }

        //テキスト出現開始
        typingCoroutine = StartCoroutine(TypeText(step.message));
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
}
