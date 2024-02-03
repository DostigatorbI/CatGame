using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerMovement _player;
    void Awake()
    {
        _player = GetComponentInParent<PlayerMovement>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.gameObject.layer == 6)
        {
            _player.IsGrounded = true;
            Debug.Log("Set " + _player.IsGrounded);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _player.IsGrounded = false;
            Debug.Log("Set " + _player.IsGrounded);
        }
    }
}
