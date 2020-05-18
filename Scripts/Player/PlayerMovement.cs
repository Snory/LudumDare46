using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _body;

    [SerializeField]
    Animator _anim;
    float _speed = 1.5f;

    private void Awake()
    {
        _body = this.GetComponent<Rigidbody2D>();
        if(_body == null)
        {
            Debug.Log("[PlayerMovement]: missing body");
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * _speed;
        Vector3 tempScale = transform.localScale;

        if(velocity.sqrMagnitude > 0)
        {
            _anim.SetBool("walking", true);
        }else
        {
            _anim.SetBool("walking", false);
        }

        if (velocity.x < 0)
        {
            if (tempScale.x > 0)
            {
                tempScale.x *= -1;
            }
        }else if(velocity.x > 0){
            if (tempScale.x < 0)
            {
                tempScale.x *= -1;
            }
        }
        transform.localScale = tempScale;

        _body.velocity = velocity;
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }


}
