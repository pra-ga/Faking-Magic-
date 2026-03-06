using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Quest/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string npcName;
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public Sprite npcPortrait; // Keep this but we will null-check it
}