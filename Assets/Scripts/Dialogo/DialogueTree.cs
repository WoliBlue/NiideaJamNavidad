using UnityEngine;
using UnityEngine.Scripting;

[CreateAssetMenu(fileName = "New Dialogue Tree", menuName = "Dialogue/Dialogue Tree")]
public class DialogueTree : ScriptableObject
{
    public DialogueSection[] sections;
}

[System.Serializable]
public struct DialogueSection
{
    [TextArea]
    public string[] dialogue; // Lo que dice el NPC antes de preguntar
    public bool endAfterDialogue; // Si marcas esto, se acaba la charla tras leer
    public BranchPoint branchPoint; // La pregunta y sus respuestas
}

[System.Serializable]
public struct BranchPoint
{
    [TextArea]
    public string question; // (Opcional) Texto extra de pregunta
    public Answer[] answers; // Botones de respuesta
}

[System.Serializable]
public struct Answer
{
    public string answerLabel; // Texto del botón (ej: "¡Claro que sí!")
    public int nextElement;    // A qué número de 'Section' salta (índice del array)
    public int scoreImpact;    // NUEVO: 1 (bien), -1 (mal), 0 (neutro)
}