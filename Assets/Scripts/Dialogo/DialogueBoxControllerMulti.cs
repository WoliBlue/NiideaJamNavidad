using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DialogueBoxControllerMulti : MonoBehaviour
{
    public static DialogueBoxControllerMulti instance;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject answerBox;
    [SerializeField] Button[] answerObjects;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;

    bool skipLineTriggered;
    bool answerTriggered;
    int answerIndex;
    private Person currentPerson;
    private BranchPoint currentBranchPoint; // Movida aquí arriba por orden

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    // ÚNICA DEFINICIÓN DE StartDialogue
    public void StartDialogue(DialogueTree dialogueTree, int startSection, Person person)
    {
        ResetBox();
        currentPerson = person;
        
        // Nombre del cliente
        nameText.text = person.Name; 
        
        dialogueBox.SetActive(true);
        OnDialogueStarted?.Invoke();
        StartCoroutine(RunDialogue(dialogueTree, startSection));
    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        // 1. Ciclo de frases del diálogo
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {
            // USAMOS EL PARSER PARA SUSTITUIR _figura
            string rawText = dialogueTree.sections[section].dialogue[i];
            dialogueText.text = DialogueParser.ParseText(rawText, currentPerson);

            while (skipLineTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
        }

        // 2. ¿Es el final de la charla?
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke();
            dialogueBox.SetActive(false);

            if(currentPerson != null) currentPerson.EndConversation();
            yield break;
        }

        // 3. Si no es el final, mostramos la pregunta y las respuestas
        string rawQuestion = dialogueTree.sections[section].branchPoint.question;
        dialogueText.text = DialogueParser.ParseText(rawQuestion, currentPerson);

        ShowAnswers(dialogueTree.sections[section].branchPoint);

        while (answerTriggered == false)
        {
            yield return null;
        }
        
        answerBox.SetActive(false);
        answerTriggered = false;

        // Saltamos a la siguiente sección según la respuesta elegida
        int nextSec = dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement;
        StartCoroutine(RunDialogue(dialogueTree, nextSec));
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        currentBranchPoint = branchPoint; // Guardamos para saber el impacto de puntos luego
        answerBox.SetActive(true);
        
        for (int i = 0; i < answerObjects.Length; i++)
        {
            if (i < branchPoint.answers.Length)
            {
                // Filtramos también el texto de los botones por si usas etiquetas
                string rawLabel = branchPoint.answers[i].answerLabel;
                answerObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = DialogueParser.ParseText(rawLabel, currentPerson);
                
                answerObjects[i].gameObject.SetActive(true);
            }
            else
            {
                answerObjects[i].gameObject.SetActive(false);
            }
        }
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void AnswerQuestion(int index)
    {
        // Aplicar puntos a la personalidad antes de avanzar
        if (currentPerson != null)
        {
            int impact = currentBranchPoint.answers[index].scoreImpact;
            currentPerson.Personality.Add(impact);
            Debug.Log($"Puntos de humor: {impact}. Total: {currentPerson.Personality.HumourBalance}");
        }

        answerIndex = index;
        answerTriggered = true;
    }

    void ResetBox()
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        answerBox.SetActive(false);
        skipLineTriggered = false;
        answerTriggered = false;
    }
}