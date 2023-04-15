using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Game.NetShooter
{
	public class BulletController: NetworkBehaviour
	{
		public float destroyAfter = 2;
		public float speed = 10;

		[SyncVar]
		public Vector3 direction = Vector3.zero;

		public override void OnStartServer()
		{
			Invoke(nameof(DestroySelf), destroyAfter);
		}

		// set velocity for server and client. this way we don't have to sync the
		// position, because both the server and the client simulate it.
		void Start()
		{
			//rigidBody.AddForce(transform.forward * force);
		}

		void Update()
		{
			transform.position += direction * speed * Time.deltaTime;
		}

		// destroy for everyone on the server
		[Server]
		void DestroySelf()
		{
			NetworkServer.Destroy(gameObject);
		}

		// ServerCallback because we don't want a warning
		// if OnTriggerEnter is called on the client
		[ServerCallback]
		void OnTriggerEnter(Collider co) => DestroySelf();
	}
}
