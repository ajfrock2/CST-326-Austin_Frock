using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float impulseStrength = 1f;
    public AudioClip trampolineClip;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{this.name} collided with the {collision.gameObject.name}");
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * impulseStrength, ForceMode.Impulse);
        
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = trampolineClip;
        audio.Play();
    }
}
