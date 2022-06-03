using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgent2D : MonoBehaviour
{
    
    private Rigidbody2D rb;
    NavMeshPath path;
    [HideInInspector]//常にUnityエディタから非表示
    private Vector2 trace_area = Vector2.zero;

    private void Awake()
    {
        path = new NavMeshPath();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    

    public void Trace(Vector2 current, Vector2 target,float chaseSpeed)
    {
        

        // NavMesh に応じて経路を求める
        
        NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);
        Vector2 corner = target;
        if (path.corners.Length != 0)
        {
            corner = path.corners[0];
            if (Vector2.Distance(current, corner) <= 0.05f)
            {
                corner = path.corners[1];
            }
        }


        //transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);
        rb.velocity = (corner-current).normalized*chaseSpeed;
    }
}