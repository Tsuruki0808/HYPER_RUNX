using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager;
    [Header("TutorialManager‚Ì‰º‚Ì•û‚É‚ ‚é”Ô†‚Æ‚Ğ‚à‚Ã‚¢‚Ä‚¢‚Ü‚·")]
    public int tutorialIndex;

    private bool fastTime = false; 

    private void OnTriggerEnter(Collider c)
    {

        if (!fastTime)
        {
            fastTime = true;
            Debug.Log("G‚ê‚½I");
            tutorialManager.AdvanceTutorial(tutorialIndex);
        }
    }
}
