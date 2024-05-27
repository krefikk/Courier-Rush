using UnityEngine;
using UnityEditor;

public class ChangeDropPointRigidbodies : MonoBehaviour
{
    [MenuItem("Tools/Change DropPoint Rigidbodies to Kinematic")]
    public static void ChangeRigidbodiesToKinematic()
    {
        // Find all game objects with the "DropPoint" tag
        GameObject[] dropPoints = GameObject.FindGameObjectsWithTag("DropPoint");

        foreach (GameObject dropPoint in dropPoints)
        {
            Rigidbody2D rb = dropPoint.GetComponent<Rigidbody2D>();
            BoxCollider2D collider = dropPoint.GetComponent <BoxCollider2D>();

            // If the game object has a Rigidbody2D, set it to Kinematic
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                Debug.Log("Changed Rigidbody2D to Kinematic for: " + dropPoint.name);
            }
            else
            {
                Debug.LogWarning("No Rigidbody2D found on: " + dropPoint.name);
            }

            if (collider != null) 
            {
                collider.isTrigger = true;
                Debug.Log("Changed Collider to isTrigger for: " + dropPoint.name);
            }
            else
            {
                Debug.LogWarning("No Collider found on: " + dropPoint.name);
            }
        }

        Debug.Log("All DropPoint Rigidbodies changed to Kinematic.");
    }
}

