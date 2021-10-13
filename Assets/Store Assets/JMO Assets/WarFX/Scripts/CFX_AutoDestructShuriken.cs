using System.Collections;
using UnityEngine;

namespace Store_Assets.JMO_Assets.WarFX.Scripts
{
	[RequireComponent(typeof(ParticleSystem))]
	public class CfxAutoDestructShuriken : MonoBehaviour
	{
		public bool onlyDeactivate;
	
		void OnEnable()
		{
			StartCoroutine("CheckIfAlive");
		}
	
		IEnumerator CheckIfAlive ()
		{
			while(true)
			{
				yield return new WaitForSeconds(0.5f);
				if(!GetComponent<ParticleSystem>().IsAlive(true))
				{
					if(onlyDeactivate)
					{
						#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
						#else
						this.gameObject.SetActive(false);
						#endif
					}
					else
						GameObject.Destroy(this.gameObject);
					break;
				}
			}
		}
	}
}
