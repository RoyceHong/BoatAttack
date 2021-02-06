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

        private static void Shuffle<T>(this IList<T> list, Random rng)
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
            Random rng = new Random();

            for (int i = 0; i < 17; i++)
            {
                list.Add(i);
            }
            list.Shuffle(rng);

            return list;
        }

        private static int _fps = 30;
        private static int _resScale = 80;

        public static void Init()
        {
            queue = new Queue<int>(initQueue());
            SetDefault();
            Update();
        }

        private static void SetDefault()
        {
            _fps = 30;
            _resScale = 80;
        }

        public static void ChangeAndUpdateMetrics()
        {
            Debug.Log("ChangeAndUpdateMetrics called");
            ChangeMetrics();
            Update();
        }

        private static void ChangeMetrics()
        {
            int caseSwitch = queue.Dequeue();

            switch(caseSwitch)
            {
                // control
                case 0:
                    SetDefault();
                    break;
                
                // fps
                case 1:
                    _fps = 15; _resScale = 80;
                    break;
                case 2:
                    _fps = 20; _resScale = 80;
                    break;
                case 3:
                    _fps = 60; _resScale = 80;
                    break;
                case 4:
                    _fps = 90; _resScale = 80;
                    break;

                // resolution
                case 5:
                    _fps = 30; _resScale = 40;
                    break;
                case 6:
                    _fps = 30; _resScale = 50;
                    break;
                case 7:
                    _fps = 30; _resScale = 60;
                    break;
                case 8:
                    _fps = 30; _resScale = 120;
                    break;

                // TODO: latency
                case 9:
                    SetDefault();
                    break;
                case 10:
                    SetDefault();
                    break;
                case 11:
                    SetDefault();
                    break;
                case 12:
                    SetDefault();
                    break;

                // TODO: stability
                case 13:
                    SetDefault();
                    break;
                case 14:
                    SetDefault();
                    break;
                case 15:
                    SetDefault();
                    break;
                case 16:
                    SetDefault();
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
            Application.targetFrameRate = _fps;
            Screen.SetResolution(16 * _resScale, 9 * _resScale, true);
        }

        public static int getFPS()
        {
            return _fps;
        }

        public static int getResScale()
        {
            return _resScale;
        }
    }
}