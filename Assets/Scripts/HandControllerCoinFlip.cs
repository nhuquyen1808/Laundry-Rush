using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class HandControlledCoinFlip : MonoBehaviour
{
    public Rigidbody coinRigidbody; // The coin's Rigidbody
    public float flipStrength = 10f; // Strength of the coin flip
    private Vector3 startPosition;
    private bool isFlipping = false;
    void Start()
    {
        // Optionally, set initial position of the coin
        startPosition = transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFlipping)
        {
            StartCoinFlip();
        }
    }
    void StartCoinFlip()
    {
        isFlipping = true;
        Vector3 movementDirection = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0).normalized;
        coinRigidbody.AddForce(movementDirection * flipStrength, ForceMode.Impulse);
        coinRigidbody.AddTorque(new Vector3(Random.Range(5f, 15f), Random.Range(5f, 15f), Random.Range(5f, 15f)), ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isFlipping && this.gameObject.GetComponent<Rigidbody>().velocity.y < 1f)
        {
            isFlipping = false;
            float rotationY = transform.rotation.eulerAngles.z;
            string result = rotationY > 180f ? "Heads" : "Tails"; 
            Debug.Log("Coin result: " + result);
           // StartCoroutine(ResetCoin());
        }
    }
    IEnumerator ResetCoin()
    {
        yield return new WaitForSeconds(2);
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        coinRigidbody.velocity = Vector3.zero;
        coinRigidbody.angularVelocity = Vector3.zero;
        isFlipping = false;
    }
}