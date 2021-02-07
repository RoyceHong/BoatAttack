using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace BoatAttack
{
    /// <summary>
    /// This sends input controls to the boat engine if 'Human'
    /// </summary>
    public class HumanController : BaseController
    {
        private InputControls _controls;

        private float _throttle;
        private float _steering;

        private bool _paused;

        private Queue<float> _throttle_queue = new Queue<float>();
        private Queue<float> _steering_queue = new Queue<float>();

        public static int q_len = 0;

        private void Awake()
        {
            _controls = new InputControls();
            
            _controls.BoatControls.Trottle.performed += context => _throttle = context.ReadValue<float>();
            _controls.BoatControls.Trottle.canceled += context => _throttle = 0f;
            
            _controls.BoatControls.Steering.performed += context => _steering = context.ReadValue<float>();
            _controls.BoatControls.Steering.canceled += context => _steering = 0f;

            _controls.BoatControls.Reset.performed += ResetBoat;
            _controls.BoatControls.Freeze.performed += FreezeBoat;

            _controls.BoatControls.Time.performed += SelectTime;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _controls.BoatControls.Enable();
        }

        private void OnDisable()
        {
            _controls.BoatControls.Disable();
        }

        public static void setLatency(int frames)
        {
            q_len = frames;
        }

        private void ResetBoat(InputAction.CallbackContext context)
        {
            controller.ResetPosition();
        }

        private void FreezeBoat(InputAction.CallbackContext context)
        {
            _paused = !_paused;
            if(_paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        private void SelectTime(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            Debug.Log($"changing day time, input:{value}");
            DayNightController.SelectPreset(value);
        }

        void FixedUpdate()
        {
            _throttle_queue.Enqueue(_throttle);
            if (_throttle_queue.Count >= q_len)
            {
                float late_throttle = _throttle_queue.Dequeue();
                engine.Accelerate(late_throttle);
            }

            _steering_queue.Enqueue(_steering);
            if (_steering_queue.Count >= q_len)
            {
                float late_steering = _steering_queue.Dequeue();
                engine.Turn(late_steering);
            }
           
        }
    }
}

