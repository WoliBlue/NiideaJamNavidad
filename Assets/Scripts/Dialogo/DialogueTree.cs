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
    public string[] dialogue;
    public bool endAfterDialogue;
    public BranchPoint branchPoint;
}
[System.Serializable]
public struct BranchPoint
{
    [TextArea]
    public string question;
    public Answer[] answers;
}
[System.Serializable]
public struct Answer
{
    public string answerLabel;
    public int nextElement;
}