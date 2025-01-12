using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterFish : MonoBehaviour
{
    public ResultLogger resultLogger;
    public float fishWeight;
    public float fishLength;
    public float conditionRight;
    public GameObject fishObject;
    public Weight weight;
    private DialogueBoxController dialogueBoxController;
    public NpcTriggerDialogue NpcTriggerDialogue;

    private void Start()
    {
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.GetComponent<Weight>() && collisionObject.gameObject.tag == "Bone")
        {
            if (dialogueBoxController.dialogueTreeRestart != null && dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue")
            {
                if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[7].dialogue[1])
                {
                    dialogueBoxController.SkipLine();
                }
            }
            weight = collisionObject.GetComponent<Weight>();
            fishObject = weight.fish;

            resultLogger.activeFishText.SetText("Fish "+ resultLogger.getFishNumber(fishObject).ToString());
            resultLogger.setActiveFish(resultLogger.getFishNumber(fishObject));
            
            // Get weight
            fishWeight = weight.ObjectWeight;
            fishWeight = (float)System.Math.Round(fishWeight, 1);

            // Get length
            float lengt = weight.fish.transform.localScale.x;
            fishLength = (float)(3388341 + (-5.016348 - 3388341) / (1 + Math.Pow(lengt / 282116.9, 0.882021)));
            fishLength = (float)Math.Round(fishLength * 2, MidpointRounding.AwayFromZero) / 2;
            
            // Calculate condition factor
            conditionRight = ((fishWeight/fishLength)/fishLength)/fishLength;
            conditionRight *= 100;
            conditionRight = (float)Math.Truncate(conditionRight * 100) / 100;
            
        }
        else if (collisionObject.gameObject.name == "basket_plastic")
        {
            NpcTriggerDialogue.FindDialogueSection();
            collisionObject.transform.position = ObjectPositions.Instance._basketPosition;
            collisionObject.transform.rotation = ObjectPositions.Instance._basketRotation;
            NpcTriggerDialogue.Error5();
        }
        else if (collisionObject.gameObject.name == "counter_handheld")
        {
            NpcTriggerDialogue.FindDialogueSection();
            collisionObject.transform.position = ObjectPositions.Instance._handheldCounterPosition;
            collisionObject.transform.rotation = ObjectPositions.Instance._handheldCounterRotation;
            NpcTriggerDialogue.Error5();
        }
        else if (collisionObject.gameObject.name == "MicroscopeSlideModel")
        {
            NpcTriggerDialogue.FindDialogueSection();
            collisionObject.transform.position = ObjectPositions.Instance._microscopeSlidePosition;
            collisionObject.transform.rotation = ObjectPositions.Instance._microscopeSlideRotation;
            NpcTriggerDialogue.Error5();
        }
        
    }

    public void activeFishChanged(GameObject fish)
    {
        fishObject = fish;
        weight = fish.GetComponentInChildren<Weight>();
        
        // Get weight
        fishWeight = weight.ObjectWeight;
        fishWeight = (float)System.Math.Round(fishWeight, 1);

        // Get length
        float lengt = weight.fish.transform.localScale.x;
        fishLength = (float)(3388341 + (-5.016348 - 3388341) / (1 + Math.Pow(lengt / 282116.9, 0.882021)));
        fishLength = (float)Math.Round(fishLength * 2, MidpointRounding.AwayFromZero) / 2;
        
        // Calculate condition factor
        conditionRight = ((fishWeight/fishLength)/fishLength)/fishLength;
        conditionRight *= 100;
        conditionRight = (float)Math.Round(conditionRight, 2);
    }

    // private void OnTriggerExit(Collider collisionObject)
    // {
    //     if (collisionObject.GetComponent<Weight>())
    //     {
    //         fishWeight = 0;
    //         fishLength = 0;
    //         conditionRight = 0;
    //         fishObject = null;
    //     }
    // }
}