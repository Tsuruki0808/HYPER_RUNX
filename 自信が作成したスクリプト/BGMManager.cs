using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [Header("Scene 名 → BGM の割り当て")]
    public AudioClip titleBGM;   //Title シーン
    public AudioClip[] gameBGMs; //Game シーンの候補（ランダム）
    public AudioClip resultBGM;  //Result シーン

    private static BGMManager instance;
    private AudioSource audioSource;


    void Awake()
    {
        //シングルトン処理
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        //シーン遷移検知イベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;

        //初回起動時の自動再生
        PlaySceneBGM(SceneManager.GetActiveScene().name);
    }


    //シーン遷移を検知
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneBGM(scene.name);
    }


    //シーン名に応じてBGMを変更
    private void PlaySceneBGM(string sceneName)
    {
        AudioClip clip = null;

        if (sceneName == "TsurukiTitle")
        {
            clip = titleBGM;
        }
        else if (sceneName == "niikawa")
        {
            clip = resultBGM;
        }
        else
        {
            if (gameBGMs != null && gameBGMs.Length > 0)
            {
                int index = Random.Range(0, gameBGMs.Length);
                clip = gameBGMs[index];
            }
        }

        if (clip != null && audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}