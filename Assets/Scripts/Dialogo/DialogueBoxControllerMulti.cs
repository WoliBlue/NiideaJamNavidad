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
    
    // Para que no se pueda pulsar Z mientras eliges respuesta
    bool isWaitingForAnswer = false; 

    private Person currentPerson;
    private BranchPoint currentBranchPoint;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Update()
    {
        // 1. SI ESTAMOS ESPERANDO RESPUESTA (MODO ELECCIÓN)
        if (dialogueBox.activeSelf && isWaitingForAnswer)
        {
            // Tecla 1
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                TryPressAnswer(0);
            }
            // Tecla 2
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                TryPressAnswer(1);
            }
            // Tecla 3
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                TryPressAnswer(2);
            }
        }
        // 2. SI SOLO ESTAMOS LEYENDO TEXTO (MODO LECTURA)
        else if (dialogueBox.activeSelf && !isWaitingForAnswer)
        {
            // Detectar tecla Z, Espacio o Clic Izquierdo para avanzar
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                SkipLine();
            }
        }
    }

    public void StartDialogue(DialogueTree dialogueTree, int startSection, Person person)
    {
        ResetBox();
        currentPerson = person;
        
        // Poner nombre
        nameText.text = person.Name; 
        
        dialogueBox.SetActive(true);
        OnDialogueStarted?.Invoke();
        StartCoroutine(RunDialogue(dialogueTree, startSection));
    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        // ---------------------------------------------------------
        // 1. REPRODUCIR LAS FRASES NORMALES (Dialogue)
        // ---------------------------------------------------------
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {
            string rawText = dialogueTree.sections[section].dialogue[i];
            
            // Usamos el Parser (asegúrate de que DialogueParser existe y es estático)
            // Si da error, usa: dialogueText.text = rawText;
            dialogueText.text = DialogueParser.ParseText(rawText, currentPerson);

            // Esperar a que el jugador pulse Z
            skipLineTriggered = false;
            while (skipLineTriggered == false)
            {
                yield return null;
            }
        }

        // ---------------------------------------------------------
        // 2. COMPROBAR SI ES EL FINAL DEL DIÁLOGO (End After Dialogue)
        // ---------------------------------------------------------
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            CerrarDialogo();
            
            // Si se acaba el diálogo normal y NO se ha activado la compra, el cliente se va.
            // Si willBuy es true, el cliente se queda esperando la figura.
            if (!GameManager.instance.willBuy && currentPerson != null) 
            {
                currentPerson.EndConversation(); 
            }
            yield break;
        }

        // ---------------------------------------------------------
        // 3. GESTIÓN DE LA PREGUNTA O DECISIÓN FINAL (_decision)
        // ---------------------------------------------------------
        string rawQuestion = dialogueTree.sections[section].branchPoint.question;

        // CASO ESPECIAL: EL MOMENTO DE LA VERDAD
        if (rawQuestion.Trim() == "_decision")
        {
            // Miramos el balance de humor acumulado
            if (currentPerson.Personality.HumourBalance >= 1)
            {
                // --- GANA: COMPRA ---
                dialogueText.text = "¡Me has convencido! Me llevaré una figura.";
                GameManager.instance.willBuy = true; // ACTIVAMOS LA MECÁNICA DE ARRASTRAR
                
                // Esperamos una última Z para cerrar el texto
                skipLineTriggered = false;
                while (skipLineTriggered == false) yield return null;
                
                CerrarDialogo();
                // OJO: No llamamos a currentPerson.EndConversation() aquí
                // porque el cliente debe quedarse quieto esperando que le des la figura.
            }
            else
            {
                // --- PIERDE: SE VA ---
                dialogueText.text = "Bah, paso de esto. Me voy.";
                GameManager.instance.willBuy = false;

                skipLineTriggered = false;
                while (skipLineTriggered == false) yield return null;

                CerrarDialogo();
                if(currentPerson != null) currentPerson.EndConversation(); // Se va enfadado
            }
            yield break; // Salimos de la corutina
        }

        // ---------------------------------------------------------
        // 4. MOSTRAR PREGUNTA Y RESPUESTAS (SI NO ES _decision)
        // ---------------------------------------------------------
        
        // Mostrar la pregunta
        dialogueText.text = DialogueParser.ParseText(rawQuestion, currentPerson);

        // Mostrar botones
        ShowAnswers(dialogueTree.sections[section].branchPoint);
        isWaitingForAnswer = true; // Bloqueamos la tecla Z

        // Esperar a que se pulse un botón
        answerTriggered = false;
        while (answerTriggered == false)
        {
            yield return null;
        }
        
        // Ocultar botones y desbloquear Z
        answerBox.SetActive(false);
        isWaitingForAnswer = false; 

        // Saltar a la siguiente sección del árbol
        int nextSec = dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement;
        
        // Pequeña pausa para evitar saltos bruscos
        yield return null; 
        
        StartCoroutine(RunDialogue(dialogueTree, nextSec));
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        currentBranchPoint = branchPoint;
        answerBox.SetActive(true);
        
        for (int i = 0; i < answerObjects.Length; i++)
        {
            if (i < branchPoint.answers.Length)
            {
                string rawLabel = branchPoint.answers[i].answerLabel;
                // Parseamos también el texto de los botones
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
        // Aplicar puntos a la personalidad
        // Quitamos "currentBranchPoint != null" porque al ser struct no puede ser null
        if (currentPerson != null) 
        {
            // Asumimos que si se ha pulsado el botón, currentBranchPoint tiene datos validos
            int impact = currentBranchPoint.answers[index].scoreImpact;
            currentPerson.Personality.Add(impact);
        }

        answerIndex = index;
        answerTriggered = true;
    }

    void CerrarDialogo()
    {
        OnDialogueEnded?.Invoke();
        dialogueBox.SetActive(false);
        answerBox.SetActive(false);
    }

    // Método de seguridad para comprobar si el botón existe antes de pulsarlo
    void TryPressAnswer(int index)
    {
        // Verificamos que el índice esté dentro del array Y que el botón esté activo (visible)
        if (index >= 0 && index < answerObjects.Length)
        {
            if (answerObjects[index].gameObject.activeSelf)
            {
                // Simulamos que se ha pulsado el botón
                AnswerQuestion(index);
            }
        }
    }

    void ResetBox()
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        answerBox.SetActive(false);
        skipLineTriggered = false;
        answerTriggered = false;
        isWaitingForAnswer = false;
    }
}