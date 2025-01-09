using UnityEngine;
using TMPro;
using System.Collections;

public class TextEffects : MonoBehaviour
{
    public static TextEffects Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TypeSentenceWithDelay(TextMeshProUGUI dialogueText, string sentence, float typeSpeed)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / typeSpeed);
        }
        yield return null;
    }

    public IEnumerator TypeSentenceWithFade(TextMeshProUGUI dialogueText, string sentence, float typeSpeed)
    {
        dialogueText.text = "";
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0);

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
        }

        for (float alpha = 0; alpha <= 1; alpha += Time.deltaTime)
        {
            dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, alpha);
            yield return null;
        }
    }

    public IEnumerator TypeSentenceWithBounce(TextMeshProUGUI dialogueText, string sentence, float typeSpeed)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            StartCoroutine(BounceLetter(dialogueText, dialogueText.text.Length - 1));
            yield return new WaitForSeconds(1f / typeSpeed);
        }
    }

    private IEnumerator BounceLetter(TextMeshProUGUI dialogueText, int index)
    {
        float bounceDuration = 0.5f;
        AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        for (float t = 0; t < bounceDuration; t += Time.deltaTime)
        {
            float scale = bounceCurve.Evaluate(t / bounceDuration);
            dialogueText.text = dialogueText.text.Substring(0, index) + "<size=" + (1 + scale * 0.5f) + ">" + dialogueText.text[index] + "</size>" + dialogueText.text.Substring(index + 1);
            yield return null;
        }
    }

    public IEnumerator TypeSentenceWithWave(TextMeshProUGUI dialogueText, string sentence, float typeSpeed)
    {
        dialogueText.text = sentence; // Tampilkan teks lengkap terlebih dahulu
        TMP_TextInfo textInfo = dialogueText.textInfo;

        // Inisialisasi offset untuk masing-masing karakter
        Vector3[] initialPositions = new Vector3[textInfo.characterCount];

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            initialPositions[i] = textInfo.characterInfo[i].bottomLeft;
        }

        float waveFrequency = 2f;
        float waveAmplitude = 10f;

        while (true)
        {
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (textInfo.characterInfo[i].isVisible)
                {
                    Vector3 offset = new Vector3(0, Mathf.Sin(Time.time * waveFrequency + i) * waveAmplitude, 0);
                    textInfo.characterInfo[i].bottomLeft += offset;
                    textInfo.characterInfo[i].topLeft += offset;
                    textInfo.characterInfo[i].bottomRight += offset;
                    textInfo.characterInfo[i].topRight += offset;
                }
            }
            
            dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            yield return null;
        }
    }


    private IEnumerator WaveLetter(TextMeshProUGUI dialogueText, int index)
    {
        while (true)
        {
            float offset = Mathf.Sin(Time.time * 2f + index * 0.1f) * 10f;
            dialogueText.text = dialogueText.text.Substring(0, index) + "<voffset=" + offset + ">" + dialogueText.text[index] + "</voffset>" + dialogueText.text.Substring(index + 1);
            yield return null;
        }
    }

    public IEnumerator TypeSentenceWithRandomLetters(TextMeshProUGUI dialogueText, string sentence, float typeSpeed)
    {
        dialogueText.text = "";
        System.Random rand = new System.Random();

        foreach (char letter in sentence)
        {
            char randomChar = (char)rand.Next(33, 127); // ASCII range for printable characters
            dialogueText.text += randomChar;
            yield return new WaitForSeconds(0.05f);
            dialogueText.text = dialogueText.text.Remove(dialogueText.text.Length - 1) + letter;
            yield return new WaitForSeconds(1f / typeSpeed);
        }
    }
}
