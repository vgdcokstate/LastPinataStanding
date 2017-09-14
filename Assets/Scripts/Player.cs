using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float moveSpeed;
    private bool onGround;
    private float jumpPressure;
    private float minJump;
    private float maxJumpP;
    private Rigidbody rigBody;
    
    
    // Use this for initialization
	void Start () {
        //movement

        moveSpeed = 10f;

        //jump stuff

        onGround = true;
        jumpPressure = 0f;
        minJump = 2f;
        maxJumpP = 5f;
        rigBody = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {

        //Translate(x, y, z)
        //f is for float.
        //x: + for right, - for left
        //y: + for up, - for down

        Vector3 ang = transform.eulerAngles;

        transform.Translate(Time.deltaTime*Mathf.Cos((ang.x+90)*Mathf.Deg2Rad) * moveSpeed * Input.GetAxis("Vertical"), 0f, Time.deltaTime*Mathf.Sin((ang.x+90)*Mathf.Deg2Rad) * moveSpeed * Input.GetAxis("Vertical"));
        //transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed)
        //  transform.Rotate(0f, Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0f);
        // transform.Rotate(Vector3.up, Input.GetAxis("Horizontal")*Time.deltaTime);
        transform.Rotate(0f , Input.GetAxis("Horizontal") * Time.deltaTime * 50f, 0f);

        //big jump

        if (onGround)
        {
            //holding space bar for jump
            if (Input.GetButton("Jump"))
            {
                if(jumpPressure < maxJumpP)
                {
                    jumpPressure += Time.deltaTime * 5f;
                }
                else
                {
                    jumpPressure = maxJumpP;
                }
            }
            //not holding space bar
            else
            {
                if (jumpPressure > 0f)
                {
                    jumpPressure = jumpPressure + minJump;
                    rigBody.velocity = new Vector3(0f, jumpPressure, 0f);
                    jumpPressure = 0f;
                    onGround = false;
                }

            }
        }





	}
    //resets onGround
    void OnCollisionEnter(Collision other)
    {
       bool test = other.gameObject.CompareTag("Ground");
        if (test)
        {
            onGround = true;
           // Debug.Log("onGround = true");
        }
    }
}
