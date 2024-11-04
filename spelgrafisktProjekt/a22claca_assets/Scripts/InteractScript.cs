using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    [SerializeField] private Animator doorAnc = null;

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(gameObject.name);

            if (gameObject.name == "WateringPlanter")
            {
                other.GetComponent<PlayerCombatScript>().Watering(transform);
            }

            if (gameObject.name == "HarvestingPlanter")
            {
                other.GetComponent<PlayerCombatScript>().Harvesting(transform);
            }

            if (gameObject.name == "WaterTank")
            {
                other.GetComponent<PlayerCombatScript>().WaterTank(transform);
            }

            if (gameObject.name == "CraftingTable")
            {
                other.GetComponent<PlayerCombatScript>().Crafting(transform);
            }

            if (gameObject.name == "ScythePlanter")
            {
                other.GetComponent<PlayerCombatScript>().ScytheHarvesting(transform);
            }

            if (gameObject.name == "Door")
            {
                other.transform.LookAt(new Vector3(gameObject.transform.position.x, other.transform.position.y, gameObject.transform.position.z));
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        doorAnc.Play("DoorOpen_ani");
    }
}
