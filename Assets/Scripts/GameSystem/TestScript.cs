using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoatAttack
{
    public class TestScript : MonoBehaviour
    {
        public RaceManager rm;
        // Start is called before the first frame update
        void Start()
        {
            rm = GetComponent<RaceManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("You pressed F");
            }
        }
    }
}
