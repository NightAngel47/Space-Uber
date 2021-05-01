using NaughtyAttributes;
using UnityEngine;

public class MoraleEndingBehaviour : MoneyEndingBehaviour
{
    [SerializeField, Foldout("Threshold Stats")] private CharacterSignatureBehaviour[] characterSignatures = new CharacterSignatureBehaviour[5];

    protected override void SetUIInfo(EventCanvas eventCanvas, bool meetsThreshold)
    {
        base.SetUIInfo(eventCanvas, meetsThreshold);
        
        // display character signatures if meets threshold
        if (!meetsThreshold) return;
        eventCanvas.textBox.text += "\t\t\t\t\t\t";
        CharacterStats characterStats = FindObjectOfType<CharacterStats>();
        foreach (CharacterSignatureBehaviour characterSignature in characterSignatures)
        {
            // display that character's signature if meets their approval threshold
            if (characterStats.GetCharacterApproval(characterSignature.character) >= characterSignature.threshold)
            {
                eventCanvas.textBox.text += characterSignature.character + "    ";
            }
        }
    }
}