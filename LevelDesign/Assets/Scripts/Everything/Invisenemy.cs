using UnityEngine;

public class BooBehavior : MonoBehaviour
{
    public Transform player;          
    private Renderer invisRenderer;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        invisRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Optional: Keep Boo upright
        transform.rotation = Quaternion.LookRotation(directionToPlayer);

        
        Vector3 toBoo = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, toBoo);

        if (dotProduct > 0.7f) 
        {
            
            invisRenderer.enabled = true;
        }
        else
        {
            
            invisRenderer.enabled = false;
        }
    }
}