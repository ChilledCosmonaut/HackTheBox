using UnityEngine;

namespace TextMesh_Pro.AssaultCyborg.scripts
{
	public class Player : MonoBehaviour {
		public Animator anim;
		private float _inputH;
		private float _inputV;
		public Rigidbody rBody;
		private bool _run;
		private static readonly int InputH = Animator.StringToHash("inputH");
		private static readonly int InputV = Animator.StringToHash("inputV");
		private static readonly int Run = Animator.StringToHash("run");

		// Use this for initialization
		void Start () 
		{
			anim = GetComponent<Animator>();
			_run = false;
		}
	
		// Update is called once per frame
		void Update () 
		{
			if(Input.GetKeyDown("z"))
			{
				anim.Play("AimingCrouching", -1, 0f);
				rBody = GetComponent<Rigidbody>();

			}

			if (Input.GetKey(KeyCode.LeftShift))
			{
				_run = true;
			}
			else
			{
				_run = false;	
			}

			_inputH = Input.GetAxis ("Horizontal");
			_inputV = Input.GetAxis ("Vertical");

			anim.SetFloat(InputH, _inputH);
			anim.SetFloat(InputV, _inputV);
			anim.SetBool (Run,_run);

			float moveX = _inputH*20f*Time.deltaTime;
			float moveZ = _inputV*50f*Time.deltaTime;

			if (moveZ <= 0f)
			{
				moveX = 0f;
			}
			else if(_run)
			{
				moveX*=3f;
				moveZ*=3f;
			}

			rBody.velocity = new Vector3(moveX,0f,moveZ);
		}
	}
}
