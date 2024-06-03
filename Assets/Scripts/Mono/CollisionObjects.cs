using UnityEngine;

public class CollisionObjects : MonoBehaviour
{
    
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("Hit");
        }
    }
}