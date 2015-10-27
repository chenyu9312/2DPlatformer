using UnityEngine;
using System.Collections;
[System.Serializable]
public class VelocityRange {
    //class available in inspector
    public float vMin, vMax;
    //constructor
    public VelocityRange(float vMin, float vMax)
    {
        this.vMax = vMax;
        this.vMin = vMin;

    }
}
//playercontroll class
public class PlayerController : MonoBehaviour {
    //public instance variables
    public float speed = 5f;
    public float jump = 2f;


    public VelocityRange velocityRange = new VelocityRange(3f,10f);

    //private instance variabls
    private Rigidbody2D _rigidBody2D;
    private Transform _transform;
    private Animator _animator;
    private AudioSource[] _audioSources;
    private AudioSource _coinSound;
    private AudioSource _jumpSound;

    private float _movingValue = 0;
    private bool _isFacingRight = true;
    private bool _isGrounded = true;
	// Use this for initialization
	void Start () {
        this._rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        this._transform = gameObject.GetComponent<Transform>();
        this._animator = gameObject.GetComponent<Animator>();
        this._audioSources = gameObject.GetComponents<AudioSource>();
        this._coinSound = this._audioSources[0];
        this._jumpSound = this._audioSources[1];

    }
	
	// Update is called once per frame
	
    void FixedUpdate() {
        float forceX = 0f;
        float forceY = 0f;

        float absVelX = Mathf.Abs(this._rigidBody2D.velocity.x);
        float absVelY = Mathf.Abs(this._rigidBody2D.velocity.y);

         // value is between 1 and -1
        this._movingValue = Input.GetAxis("Horizontal");
        if (this._movingValue != 0)
        {
            //we are moving
            //check if player is grounded
            if (this._isGrounded)
            {
                this._animator.SetInteger("AnimState",1);
                if (this._movingValue > 0)//moving right
                {
                    if (absVelX < this.velocityRange.vMax)
                    {
                        forceX = this.speed;
                        this._isFacingRight = true;
                        this._flip();
                    }
                }
                else if (this._movingValue < 0)
                {
                    //moving left
                    if (absVelX < this.velocityRange.vMax)
                    {
                        forceX = -this.speed;
                        this._isFacingRight = false;
                        this._flip();
                    }

                }
            }
            
           
        }
        else 
        {
            //idle state
            this._animator.SetInteger("AnimState", 0);
        }
        //check if the player is jumping
        if ((Input.GetKey("up") || Input.GetKey(KeyCode.W)))
        {
            // chec if player is grounded
            if (this._isGrounded)
            {
                this._animator.SetInteger("AnimState", 2);
                if (absVelY < this.velocityRange.vMax)
                {
                    forceY = this.jump;
                    this._jumpSound.Play();
                    this._isGrounded = false;
                }
            }
        }

        // add force to the player to push him
        this._rigidBody2D.AddForce(new Vector2(forceX, forceY));

    }
    void OnCollisionEnter2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Coin"))
        {
            this._coinSound.Play();
        }
    }
    void OnCollisionStay2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Platform"))
        {
            this._isGrounded = true;
        }
    }

    // PRIVATE METHODS
    private void _flip()
    {
        if (this._isFacingRight)
        {
            this._transform.localScale = new Vector3(1f, 1f, 1f); // reset to normal scale
        }
        else
        {
            this._transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
