using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction;
using System.Linq;

public class FindTheOrganGameManager : MonoBehaviour
{
    public Transform organParent;

    public GameObject uiStartPrefab;
    public GameObject uiResultPrefab;
    public Transform uiSpawnPoint;

    public AudioSource sfxSource;
    public AudioSource bgmSource;
    public AudioClip countdownClip;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip bgmMain;
    public AudioClip bgmWin;

    private GameObject currentUI;
    private TextMeshProUGUI questionText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI resultText;


    private float startTime;
    private float currentTime;
    private int currentQuestionIndex = 0;
    private int correctCount = 0;
    private bool gameRunning = false;
    private bool waitingNext = false;

    public enum Language
    {
        English,
        Indonesian
    }

    [SerializeField] private Language currentLanguage = Language.English;

    private List<string> organTags = new List<string>
    {
        "Brain", "NoseCavity", "OralCavity", "Trachea", "Lungs", "Heart",
        "Liver", "Stomach", "Gallbladder", "Pancreas", "Kidney", "LargeIntestine", "SmallIntestine"
    };

    private void Start()
    {
        if (currentUI != null) Destroy(currentUI);
    }

    public void RestartGame()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        yield return null; // Tunggu 1 frame untuk hindari race condition tombol

        if (currentUI != null) Destroy(currentUI);

        StopAllCoroutines();
        sfxSource.Stop();
        bgmSource.Stop();

        currentQuestionIndex = 0;
        correctCount = 0;
        currentTime = 0f;
        gameRunning = false;
        waitingNext = false;

        ShuffleOrganTags();

        currentUI = Instantiate(uiStartPrefab, uiSpawnPoint.position, uiSpawnPoint.rotation);

        questionText = currentUI.transform.Find("[Panel]_Background/[Text]_Question")?.GetComponent<TextMeshProUGUI>();
        timeText = currentUI.transform.Find("[Panel]_Background/[Text]_Time_Field")?.GetComponent<TextMeshProUGUI>();

        if (questionText == null || timeText == null)
        {
            Debug.LogError("UI path tidak lengkap!");
            yield break;
        }

        StartCoroutine(CountdownAndStart());
    }

    private void ShuffleOrganTags()
    {
        for (int i = 0; i < organTags.Count; i++)
        {
            int randomIndex = Random.Range(i, organTags.Count);
            var temp = organTags[i];
            organTags[i] = organTags[randomIndex];
            organTags[randomIndex] = temp;
        }
    }

    IEnumerator CountdownAndStart()
    {
        timeText.text = "3";

        sfxSource.Stop();
        sfxSource.clip = countdownClip;
        sfxSource.loop = false;
        sfxSource.Play();

        yield return new WaitForSecondsRealtime(1f);
        timeText.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        timeText.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        timeText.text = "0";
        yield return new WaitForSecondsRealtime(1f);

        startTime = Time.time;
        gameRunning = true;

        bgmSource.clip = bgmMain;
        bgmSource.loop = true;
        bgmSource.Play();

        SetNextQuestion();
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (gameRunning)
        {
            currentTime = Time.time - startTime;
            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(currentTime / 60f);
                int seconds = Mathf.FloorToInt(currentTime % 60f);
                timeText.text = $"{minutes:00}:{seconds:00}";
            }
            yield return null;
        }
    }

    private void SetNextQuestion()
    {
        if (currentQuestionIndex >= organTags.Count)
        {
            ShowResult();
            return;
        }

        string currentTag = organTags[currentQuestionIndex];
        string displayedAlias = GetRandomAlias(currentTag);
        questionText.text = displayedAlias;
        questionText.color = Color.white;
    }

    public void HandleOrganPoke(string tag)
    {
        if (!gameRunning || waitingNext) return;

        string expectedTag = organTags[currentQuestionIndex];

        if (IsTagCorrect(tag, expectedTag))
        {
            questionText.color = Color.green;
            correctCount++;
            sfxSource.PlayOneShot(correctClip);
        }
        else
        {
            questionText.color = Color.red;
            sfxSource.PlayOneShot(wrongClip);
        }

        waitingNext = true;
        StartCoroutine(WaitAndNext());
    }

    IEnumerator WaitAndNext()
    {
        yield return new WaitForSeconds(2f);
        currentQuestionIndex++;
        waitingNext = false;
        SetNextQuestion();
    }

    private void ShowResult()
    {
        gameRunning = false;

        if (currentUI != null) Destroy(currentUI);

        currentUI = Instantiate(uiResultPrefab, uiSpawnPoint.position, uiSpawnPoint.rotation);
        resultText = currentUI.transform.Find("[Panel]_Background/[Text]_Result_Field")?.GetComponent<TextMeshProUGUI>();
        timeText = currentUI.transform.Find("[Panel]_Background/[Text]_Time_Field")?.GetComponent<TextMeshProUGUI>();

        float duration = Time.time - startTime;
        float accuracy = (correctCount / (float)organTags.Count) * 100f;

        resultText.text = $"{accuracy:F1}%";

        int minutes = Mathf.FloorToInt(duration / 60f);
        int seconds = Mathf.FloorToInt(duration % 60f);
        timeText.text = $"{minutes:00}:{seconds:00}";

        bgmSource.Stop();
        bgmSource.loop = false;
        bgmSource.clip = bgmWin;
        bgmSource.Play();

        SetupPlayAgainButton();
    }

    private void SetupPlayAgainButton()
    {
        var playAgainBtn = currentUI.transform.Find("[Panel]_Background/[Panel]_Play_Again/ISDK_PokeInteraction");
        if (playAgainBtn != null)
        {
            var wrapper = playAgainBtn.GetComponent<InteractableUnityEventWrapper>();
            if (wrapper != null)
            {
                wrapper.WhenSelect.RemoveAllListeners();
                wrapper.WhenSelect.AddListener(() =>
                {
                    var btnMgr = FindObjectOfType<ButtonManager>();
                    if (btnMgr != null)
                        btnMgr.StartFindTheOrganGame();
                });
            }
        }
    }

    public void OnOrganPoked(Transform root)
    {
        if (!gameRunning || waitingNext) return;

        string tag = GetTagFromHierarchy(root);
        if (!string.IsNullOrEmpty(tag))
        {
            HandleOrganPoke(tag);
        }
    }

    private string GetTagFromHierarchy(Transform root)
    {
        if (organTags.Contains(root.tag)) return root.tag;

        foreach (Transform child in root)
        {
            string result = GetTagFromHierarchy(child);
            if (!string.IsNullOrEmpty(result)) return result;
        }

        return null;
    }

    private bool IsTagCorrect(string actualTag, string expectedTag)
    {
        return string.Equals(actualTag, expectedTag, System.StringComparison.OrdinalIgnoreCase);
    }

    private string GetRandomAlias(string tag)
    {
        if (!organAliasMap.ContainsKey(tag)) return tag;

        var list = currentLanguage == Language.English
            ? organAliasMap[tag].english
            : organAliasMap[tag].indonesian;

        if (list.Count > 0)
            return list[Random.Range(0, list.Count)];

        return tag;
    }

    private Dictionary<string, (List<string> english, List<string> indonesian)> organAliasMap = new Dictionary<string, (List<string>, List<string>)>()
    {
        { "Brain", (new List<string>{ "Brain" }, new List<string>{ "Otak" }) },
        { "NoseCavity", (new List<string>{ "Nose Cavity" }, new List<string>{ "Hidung", "Rongga Hidung" }) },
        { "OralCavity", (new List<string>{ "Oral Cavity" }, new List<string>{ "Mulut", "Rongga Mulut" }) },
        { "Trachea", (new List<string>{ "Trachea" }, new List<string>{ "Tenggorokan" }) },
        { "Lungs", (new List<string>{ "Lungs" }, new List<string>{ "Paru-Paru" }) },
        { "Heart", (new List<string>{ "Heart" }, new List<string>{ "Jantung" }) },
        { "Liver", (new List<string>{ "Liver" }, new List<string>{ "Hati" }) },
        { "Stomach", (new List<string>{ "Stomach" }, new List<string>{ "Lambung" }) },
        { "Gallbladder", (new List<string>{ "Gallbladder" }, new List<string>{ "Kantung Empedu" }) },
        { "Pancreas", (new List<string>{ "Pancreas" }, new List<string>{ "Pankreas" }) },
        { "Kidney", (new List<string>{ "Kidney" }, new List<string>{ "Ginjal" }) },
        { "LargeIntestine", (new List<string>{ "Large Intestine" }, new List<string>{ "Usus Besar" }) },
        { "SmallIntestine", (new List<string>{ "Small Intestine" }, new List<string>{ "Usus Kecil" }) }
    };

    public void SetLanguage(string lang)
    {
        if (lang.ToLower() == "english")
            currentLanguage = Language.English;
        else if (lang.ToLower() == "indonesian")
            currentLanguage = Language.Indonesian;
    }
}
