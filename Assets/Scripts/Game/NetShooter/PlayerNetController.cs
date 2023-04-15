using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;
using Mirror.Examples.Tanks;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game.NetShooter
{
	public class PlayerNetController : NetworkBehaviour
	{
		[SerializeField]
		private PlayerInput _input;
	    
	    [Header("Components")]
        public Animator  animator;
        public TextMeshPro  healthBar;
        public Transform turret;


        public Rigidbody2D _body;

        [Header("Movement")]
        public float rotationSpeed = 100;
        public float moveSpeed = 1;

        [Header("Firing")]
        public KeyCode shootKey = KeyCode.Space;
        public GameObject projectilePrefab;
        public Transform  projectileMount;

        [Header("Stats")]
        [SyncVar] public int health = 4;

        void FixedUpdate()
        {
            // always update health bar.
            // (SyncVar hook would only update on clients, not on server)
            healthBar.text = health.ToString();
            
            // take input from focused window only
            if(!Application.isFocused) return; 

            // movement for local player
            if (isLocalPlayer)
            {
	            Vector2 inputVector = _input.actions["Move"].ReadValue<Vector2>();
	            if (inputVector == Vector2.zero)
	            {
		            animator.SetBool("Moving", false);
		            return;
	            }

	            Vector3 moveVector = new Vector3(inputVector.x, inputVector.y, 0.0f);

                Debug.Log($"Move -> {moveVector.x}:{moveVector.y}");

                // rotate
                //float horizontal = moveVector.x;
                //transform.Rotate(0, 0, horizontal * rotationSpeed * Time.deltaTime);

                var targetRotation = Quaternion.LookRotation(Vector3.forward, moveVector);
                transform.rotation =
	                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                //transform.Rotate();

                // move
                float vertical = moveVector.y;
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 velocity = forward * Mathf.Max(vertical, 0) * moveSpeed;

                //_body.velocity = velocity;

                //_body.velocity = moveVector * moveSpeed;

                //animator.SetBool("Moving", agent.velocity != Vector3.zero);

                //transform.position += velocity * Time.deltaTime;

                transform.position += moveVector * moveSpeed;

                animator.SetBool("Moving", moveVector != Vector3.zero);

	            /*
	            // rotate
                float horizontal = Input.GetAxis("Horizontal");
                transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
                // move
                float vertical = Input.GetAxis("Vertical");
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                agent.velocity = forward * Mathf.Max(vertical, 0) * agent.speed;
                animator.SetBool("Moving", agent.velocity != Vector3.zero);
                */
                
                // shoot
                /*if (Input.GetKeyDown(shootKey))
                {
                    CmdFire();
                }
                */

                RotateTurret();
            }
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
	        if (context.started)
	        {
		        CmdFire();
	        }
        }

        // this is called on the server
        [Command]
        void CmdFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);

            var bullet = projectile.GetComponent<BulletController>();
            if (bullet != null)
            {
	            bullet.direction = projectileMount.rotation * Vector3.right;
            }

            NetworkServer.Spawn(projectile);
            RpcOnFire();
        }

        // this is called on the tank that fired for all observers
        [ClientRpc]
        void RpcOnFire()
        {
            animator.SetTrigger("Shoot");
        }

        [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BulletController>() != null)
            {
                --health;
                if (health == 0)
                    NetworkServer.Destroy(gameObject);
            }
        }

        void RotateTurret()
        {
            /*
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                Debug.DrawLine(ray.origin, hit.point);
                Vector3 lookRotation = new Vector3(hit.point.x, turret.transform.position.y, hit.point.z);
                turret.transform.LookAt(lookRotation);
            }
            */
        }
    }
}
