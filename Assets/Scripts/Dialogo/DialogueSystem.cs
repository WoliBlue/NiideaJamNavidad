using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
[SerializeField] TextMeshProUGUI dialogueText;
[SerializeField] TextMeshProUGUI nameText;
[SerializeField] GameObject dialoguePanel;
public DialogueAsset dialogueAsset;
public float charactersPerSecond = 90;

public string charName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowDialogue("HOLA TENGO AHHHHHHHHHHHHHHHHHHH",charName);
        }
    }


IEnumerator TypeTextUncapped(string line)
{
    float timer = 0;
    float interval = 1 / charactersPerSecond;
    string textBuffer = null;
    char[] chars = line.ToCharArray();
    int i = 0;

    while (i < chars.Length)
    {
        if (timer < Time.deltaTime)
        {
            textBuffer += chars[i];
            dialogueText.text = textBuffer;
            timer += interval;
            i++;
        }
        else
        {
            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
public void ShowDialogue(string dialogue, string name)
{
    nameText.text = name + ":";
    StartCoroutine(TypeTextUncapped("Hola"));
    dialoguePanel.SetActive(true);
}

public void EndDialogue()
{
    nameText.text = null;
    dialogueText.text = null;;
    dialoguePanel.SetActive(false);
}
}
