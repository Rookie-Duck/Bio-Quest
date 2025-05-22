using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private List<string> organTags = new List<string>
    {
        "Brain", "NoseCavity", "OralCavity", "Trachea", "Lungs", "Heart",
        "Liver", "Stomach", "Gallbladder", "Pancreas", "Kidney", "LargeIntestine", "SmallIntestine"
    };

    private void Start()
    {
        if (currentUI != null) Destroy(currentUI);
    }

    public void OnPlayButtonPoke()
    {
        // Reset dan destroy UI sebelumnya
        if (currentUI != null) Destroy(currentUI);

        StopAllCoroutines();
        sfxSource.Stop();
        bgmSource.Stop();

        currentQuestionIndex = 0;
        correctCount = 0;
        currentTime = 0f;
        gameRunning = false;
        waitingNext = false;

        currentUI = Instantiate(uiStartPrefab, uiSpawnPoint.position, uiSpawnPoint.rotation);

        // Cek apakah child path benar
        Transform qTransform = currentUI.transform.Find("[Panel]_Background/[Text]_Question");
        Transform tTransform = currentUI.transform.Find("[Panel]_Background/[Text]_Time_Field");

        if (qTransform == null || tTransform == null)
        {
            Debug.LogError("UI path not found in uiStartPrefab! Periksa prefab path-nya.");
            return;
        }

        questionText = qTransform.GetComponent<TextMeshProUGUI>();
        timeText = tTransform.GetComponent<TextMeshProUGUI>();

        StartCoroutine(CountdownAndStart());
    }


    IEnumerator CountdownAndStart()
    {
        int count = 3;
        while (count > 0)
        {
            questionText.text = count.ToString();

            sfxSource.Stop();                    // matikan yang sedang bunyi
            sfxSource.clip = countdownClip;      // assign clip
            sfxSource.Play();                    // play manual (tanpa OneShot)

            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

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
        questionText.text = currentTag;
        questionText.color = Color.white;
    }

    public void HandleOrganPoke(string tag)
    {
        if (!gameRunning || waitingNext) return;

        string expectedTag = organTags[currentQuestionIndex];

        if (tag == expectedTag)
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
        resultText = currentUI.transform.Find("[Panel]_Background/[Text]_Result_Field").GetComponent<TextMeshProUGUI>();
        timeText = currentUI.transform.Find("[Panel]_Background/[Text]_Time_Field").GetComponent<TextMeshProUGUI>();

        float duration = Time.time - startTime;
        float accuracy = (correctCount / (float)organTags.Count) * 100f;

        resultText.text = $"{accuracy:F1}%";
        timeText.text = $"{duration:F1}s";

        bgmSource.Stop();
        bgmSource.loop = false;
        bgmSource.clip = bgmWin;
        bgmSource.Play();
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
}
