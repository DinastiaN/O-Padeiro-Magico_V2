using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    AI_Movement aiMovement;

    private void Start()
    {
        aiMovement = GetComponent<AI_Movement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            aiMovement.StartFollowing();
        }
    }
}
