using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;
    public Image portraitImage; // Assign even if not used; we will null-check
    
    private int currentLineIndex;
    private DialogueData currentData;

    public void ShowDialogue(DialogueData data)
    {
        currentData = data;
        currentLineIndex = 0;
        
        nameText.text = data.npcName;
        
        // Null check for portrait as requested
        if (portraitImage != null)
        {
            if (data.npcPortrait != null)
            {
                portraitImage.sprite = data.npcPortrait;
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                portraitImage.gameObject.SetActive(false);
            }
        }
        
        gameObject.SetActive(true);
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentData == null) return;

        if (currentLineIndex < currentData.dialogueLines.Length)
        {
            messageText.text = currentData.dialogueLines[currentLineIndex];
            currentLineIndex++;
        }
        else
        {
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        gameObject.SetActive(false);
        currentData = null;
    }
}