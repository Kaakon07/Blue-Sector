using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuttingFishSorting : MonoBehaviour
{
    [SerializeField]
    public FactoryFishState.State _successOnGuttingSuccess;
    private List<int> _sortedFish = new List<int>();
    public int SortedFishCount
    { 
        get { return _sortedFish.Count;}
    }
    // ------------------ Unity Functions ------------------

    /// <summary>
    /// When the fish collides with the fish sorting trigger, check if the fish is in the correct state.
    /// </summary>
    private void OnTriggerEnter(Collider collisionObject)
    {
        // Get the main fish object
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        if (fish.tag != "Fish")
        {
            return;
        }

        if (!_sortedFish.Contains(fish.gameObject.GetInstanceID()))
        {
            _sortedFish.Add(fish.gameObject.GetInstanceID());
        }
        if (GameManager.Instance != null)
            HandleAudioFeedback(fish.GetComponent<FactoryFishState>());
    }

     private void HandleAudioFeedback(FactoryFishState fishState)
    {
        if (checkFishState(fishState.gameObject))
        {
            GameManager.Instance.PlaySound("correct");
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }
    }


    // ---------------- Private Functions ------------------

    /// <summary>
    /// Check if the state of the fish is the same as the success condition.
    /// Play a sound based on the result.
    /// </summary>
    public bool checkFishState(GameObject fish)
    {
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (fishState.CurrentState == _successOnGuttingSuccess)
        {
        return true;
        }
        else
        {
        return false;
        }
    }
}
