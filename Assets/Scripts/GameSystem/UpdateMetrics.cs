using System;
using System.Collections;

namespace BoatAttack
{
    public class UpdateMetrics : MonoBehaviour
    {
        private static Queue<int> queue = new Queue<int>(initQueue());

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
            List<int> list = new List<int>(17);
            Random rng = new Random();

            for (int i = 0; i < 17; i++)
            {
                list[i] = i;
            }
            list.Shuffle(rng);

            return list;
        }

        private static int _fps = 60;
        private static int _resScale = 80;

        public static void Init()
        {
            SetDefault();
            Update();
        }

        private static void SetDefault()
        {
            _fps = 30;
            _resScale = 80;
        }

        public static void UpdateMetrics()
        {
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