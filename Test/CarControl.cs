using UnityEngine;
using System.Collections;

public class CarControl : MonoBehaviour
{
    public GameObject mapSphere;
    public float radius;

    void Start()
    {
        radius = Mathf.Sqrt(mapSphere.transform.lossyScale.sqrMagnitude/3);
    }

    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");
        Move(xSpeed, ySpeed);
    }

    private void Move(float xSpeed, float ySpeed)
    {
        
    }
}
