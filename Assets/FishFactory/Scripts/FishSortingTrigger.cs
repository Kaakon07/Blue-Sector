using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSortingTrigger : MonoBehaviour
{
    /// <summary>
    /// When the fish enters the trigger, check if the fish has been sorted and if it has the correct tier
    /// </summary>
    /// <param name="collisionObject">The fish collider</param>
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Bone")
        {
            return;
        }
        
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        List<GameObject> sortedFish = FishSortingButton.fishSortingButton.SortedFish;

        if (sortedFish.Contains(fish.gameObject))
        {
            return;
        }
        sortedFish.Add(fish.gameObject);

        string fishState = fish.GetComponent<FactoryFishState>().CurrentState.ToString();
        if (fishState == FishSortingButton.fishSortingButton.CurrentTier.ToString())
        {
            GameManager.Instance.PlaySound("correct");
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }

        OpenDoor(gameObject.name[gameObject.name.Length - 1]);
    }
    
    /// <summary>
    /// Open the door with the given name and close it after a delay
    /// </summary>
    /// <param name="doorName">The door number</param>
    private void OpenDoor(string doorName)
    {
        GameObject door = GameObject.Find("SlidingDoor" + doorName);
        StartCoroutine(CloseDoorAfterDelay(door));
    }

    /// <summary>
    /// Close the door after a delay
    /// </summary>
    /// <param name="door">The door to close</param>
    private IEnumerator CloseDoorAfterDelay(GameObject door)
    {
        yield return new WaitForSeconds(1);
        door.transform.Translate(Vector3.back * 0.177f);
        yield return new WaitForSeconds(2);
        door.transform.Translate(Vector3.forward * 0.177f);
    }
}
