using System.Collections;
using Units;
using UnityEditor.Experimental;
using UnityEngine.UI;
using UnityEngine;

namespace Store_Assets.Scifi.Scripts
{
    public class DoorFunctions : MonoBehaviour {

        static Animator _doorAnim;
    
        public GameObject locker;
        public GameObject sound;
        private CameraBehaviour _cameraBehaviour;
        private IEnumerator _timmer;

        private void Awake()
        {
            _doorAnim = GetComponent<Animator>();
        }

        public void Open(CameraBehaviour cameraBehaviour, GameObject hacker)
        {
            cameraBehaviour.CameraSwitch(gameObject);
            _cameraBehaviour = cameraBehaviour;;
            _doorAnim.SetBool("Door_Open",true);
            _timmer = Timmer(hacker);
            StartCoroutine(_timmer);
            ///locker.SetActive(true);
            // plays sound when door opens.
            //sound.SetActive(true);
            
        }

        private IEnumerator Timmer(GameObject hacker)
        {
            yield return new WaitForSeconds(2f);
            hacker.GetComponent<UnitStat>().actions -= 2;
            _doorAnim.SetBool("Door_Open", false);
            StopCoroutine(_timmer);
            //locker.SetActive(false);
            // plays sound when door closes
            //sound.SetActive(false);
        }
    }
}
	
	