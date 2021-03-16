using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using BoatAttack.UI;
using UnityEngine.Playables;
using Random = System.Random;

namespace BoatAttack
{
    public static class UpdateMetrics
    {
        private static Queue<int> queue;
        private static int surveyCode;
        private static Random rng = new Random();

        private static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while(n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static List<int> initQueue()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                list.Add(i);
            }
            list.Shuffle();

            return list;
        }

        public static int randFPS()
        {
            if (Application.targetFrameRate == _fps) {
                if (rng.Next(0, 20) < 2)
                {
                    return _fps - _fpsVar;
                } else {
                    return _fps;
                }
            } else {
                if (rng.Next(0,20) < 19)
                {
                    return _fps - _fpsVar;
                } else {
                    return _fps;
                }
            }
        }

        private static int _fps = 30;
        private static int _resScale = 80;
        private static int _latency = 0;
        private static int _fpsVar = 0;
        private static int _lapID = 0;

        public static void Init()
        {
            queue = new Queue<int>(initQueue());
            surveyCode = rng.Next();
            QualitySettings.vSyncCount = 0;
            SetDefault();
            Update();
        }

        private static void SetDefault()
        {
            _fps = 30;
            _resScale = 80;
            _latency = 0;
            _fpsVar = 0;
            _lapID = 0;
        }

        public static void ChangeAndUpdateMetrics()
        {
            Debug.Log("ChangeAndUpdateMetrics called");
            ChangeMetrics();
            Update();
        }

        private static void ChangeMetrics()
        {
            _lapID = (queue.Count > 0) ? queue.Dequeue() : 0;

            switch(_lapID)
            {
                // control
                case 0:
                    SetDefault();
                    break;
                
                case 1:
                    _fps = 30; _resScale = 80; _latency = 0; _fpsVar = 5; 
                    break;
                case 2:
                    _fps = 30; _resScale = 80; _latency = 0; _fpsVar = 15; 
                    break;
                case 3:
                    _fps = 30; _resScale = 80; _latency = 5; _fpsVar = 0; 
                    break;
                case 4:
                    _fps = 30; _resScale = 80; _latency = 5; _fpsVar = 5; 
                    break;
                case 5:
                    _fps = 30; _resScale = 80; _latency = 5; _fpsVar = 15; 
                    break;
                case 6:
                    _fps = 30; _resScale = 80; _latency = 15; _fpsVar = 0; 
                    break;
                case 7:
                    _fps = 30; _resScale = 80; _latency = 15; _fpsVar = 5; 
                    break;
                case 8:
                    _fps = 30; _resScale = 80; _latency = 15; _fpsVar = 15; 
                    break;
                case 9:
                    _fps = 60; _resScale = 80; _latency = 0; _fpsVar = 0; 
                    break;
                case 10:
                    _fps = 60; _resScale = 80; _latency = 0; _fpsVar = 5; 
                    break;
                case 11:
                    _fps = 60; _resScale = 80; _latency = 0; _fpsVar = 15; 
                    break;
                case 12:
                    _fps = 60; _resScale = 80; _latency = 5; _fpsVar = 0; 
                    break;
                case 13:
                    _fps = 60; _resScale = 80; _latency = 5; _fpsVar = 5; 
                    break;
                case 14:
                    _fps = 60; _resScale = 80; _latency = 5; _fpsVar = 15; 
                    break;
                case 15:
                    _fps = 60; _resScale = 80; _latency = 15; _fpsVar = 0; 
                    break;
                case 16:
                    _fps = 60; _resScale = 80; _latency = 15; _fpsVar = 5; 
                    break;
                case 17:
                    _fps = 60; _resScale = 80; _latency = 15; _fpsVar = 15; 
                    break;
                case 18:
                    _fps = 30; _resScale = 40; _latency = 0; _fpsVar = 0; 
                    break;
                case 19:
                    _fps = 30; _resScale = 120; _latency = 0; _fpsVar = 0; 
                    break;
                default:
                    SetDefault();
                    break;
            }
        }

        private static void Update()
        {
            Debug.Log("This lap's fps: " + _fps);
            Debug.Log("This lap's resScale: " + _resScale);
            Debug.Log("This lap's latency: " + _latency);
            Application.targetFrameRate = _fps;
            Screen.SetResolution(16 * _resScale, 9 * _resScale, true);
            HumanController.setLatency(_latency);

        }

        public static int getLapID()
        {
            return _lapID;
        }

        public static int getFPS()
        {
            return _fps;
        }

        public static int getResScale()
        {
            return _resScale;
        }

        public static int getLatency()
        {
            return _latency;
        }

        public static int getFPSVar()
        {
            return _fpsVar;
        }
        public static int getSurveyCode()
        {
            return surveyCode;
        }
        public static bool isQueueEmpty()
        {
            return queue.Count == 0;
        }
    }
}