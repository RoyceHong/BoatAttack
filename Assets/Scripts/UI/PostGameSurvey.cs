using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

namespace BoatAttack.UI
{
    public class PostGameSurvey : MonoBehaviour
    {
        private String[] questions = {
            "How would you rate your satisfaction of the smoothness of the game? " +
                "[1 - Very Unsatisfied; 5 - Very Satisfied]",
            "How would you rate your satisfaction of the graphics/visual appeal of the game? " +
                "[1 - Very Unsatisfied; 5 - Very Satisfied]",
            "How would you rate your satisfaction of the responsiveness of the game? " +
                "[1 - Very Unsatisfied; 5 - Very Satisfied]",
            "How would you rate your satisfaction of the overall experience of the game? " +
                "[1 - Very Unsatisfied; 5 - Very Satisfied]"
        };
        int qi = 0;
        int prev_qi = -1;
        GameObject qtext, etext, ebtn;
        public bool showing = true;
        public bool doneSurvey = false;
        public bool notErrorYet = true;
        ToggleGroup toggle_group;
        GameObject panel;
        public String surveyData = "";
        Guid sessionUuid = System.Guid.NewGuid();
        public StreamWriter writer;
        UnityWebRequest wwwPostGame;

        // Use this for initialization
        void Start()
        {
            qtext = GameObject.Find("SurveyQText");
            etext = GameObject.Find("SurveyEText");
            ebtn = GameObject.Find("SurveyRetry");
            panel = GameObject.Find("Panel (Post-Game Survey)");
            //raceui = GameObject.Find("Race_Canvas").GetComponent<RaceUI>();
            //raceui.postGameSurvey = this;
            toggle_group = GameObject.Find("SurveyToggleGroup").GetComponent<ToggleGroup>();
            hide();
        }

        public void hide()
        {
            showing = false;
            panel.SetActive(false);
            clearError();
        }

        public void show()
        {
            Debug.Log("In the showing survey function!");
            showing = true;
            panel.SetActive(true);
            toggle_group.SetAllTogglesOff();
            clearError();
            prev_qi = -1;
            qi = 0;
            surveyData = "";
        }

        public void clearToggles()
        {
            toggle_group.SetAllTogglesOff();
        }

        static String getIdButtonName(String btnName)
        {
            int pos = btnName.IndexOf('(');
            if (pos < 0)
            {
                return "";
            }
            int pos1 = btnName.IndexOf(')', pos);
            if (pos1 < 0)
            {
                pos1 = btnName.Length;
            }
            return btnName.Substring(pos + 1, pos1 - (pos + 1));
        }

        public void setError(String error)
        {
            notErrorYet = false;
            etext.GetComponent<UnityEngine.UI.Text>().text = error;
            etext.SetActive(true);
            // ebtn.SetActive(true);
        }

        public void clearError()
        {
            notErrorYet = true;
            etext.SetActive(false);
            ebtn.SetActive(false);
        }

        public void nextAction()
        {
            if (!toggle_group.AnyTogglesOn())
            {
                return;
            }
            bool first = true;
            String arrRecord = "";
            foreach (Toggle t in toggle_group.ActiveToggles())
            {
                String id = getIdButtonName(t.gameObject.name);
                if (arrRecord.Length > 0)
                {
                    arrRecord += ", ";
                }
                arrRecord += "\"" + id.Replace("\"", "\\\"") + "\"";
            }
            String dataRecord = "\"Question_" + qi + "\": [" + arrRecord + "]";
            if (surveyData.Length > 0)
            {
                surveyData += ", ";
            }
            else
            {
                surveyData = "{";
            }
            surveyData += dataRecord;
            if (GameObject.Find("SurveyNext").GetComponentInChildren<Text>().text == "Finish")
            {
                finishAction();
            }
            else
            {
                ++qi;
                if (qi + 1 == questions.Length)
                {
                    GameObject.Find("SurveyNext").GetComponentInChildren<Text>().text = "Finish";
                }
                //if (qi >= questions.Length)
                //{
                //    GameObject.Find("SurveyNext").GetComponentInChildren<Text>().text = "Next";
                //    qi = 0;
                //    doneSurvey = true;
                //    surveyData += "}";
                //}
                toggle_group.SetAllTogglesOff();
            }
        }

        public void finishAction()
        {
            // Dummy values
            int resolutionMultiple = 80;
            int q_len = 1;
            int fps = 30;
            int fps_var = 0;
            int lapCount = 0;

            String postData = "{\"surveyData\": " + surveyData + ", \"parameters\": {" + string.Format("\"lapNumber\": {4}, \"fps\": {0}, \"resolutionMultiple\": {1}, \"q_len\": {2}, \"fps_var\": {3}", fps, resolutionMultiple, q_len, fps_var, lapCount + 1) + "},";
            postData += "\"systemInfo\": {" + string.Format("\"deviceType\": \"{0}\", \"deviceModel\": \"{1}\", \"deviceUniqueIdentifier\": \"{2}\", \"operatingSystem\": \"{3}\", \"processorType\": \"{4}\"", ("" + SystemInfo.deviceType).Replace("\"", "\\\""), ("" + SystemInfo.deviceModel).Replace("\"", "\\\""), ("" + SystemInfo.deviceUniqueIdentifier).Replace("\"", "\\\""), ("" + SystemInfo.operatingSystem).Replace("\"", "\\\""), ("" + SystemInfo.processorType).Replace("\"", "\\\"")) + "}, \"uuid\": \"" + sessionUuid + "\"}";
            initLog();
            writer.WriteLine(postData);
            writer.Flush();
            Debug.Log("ABOUT TO SEND SURVEY RESULTS TO SERVER");
            if (surveyData.Length > 0)
            {
                //UnityWebRequest www = UnityWebRequest.Put("https://seniordesign-295702.uc.r.appspot.com/raceGameResults", postData);
                //www.SetRequestHeader("Accept", "application/json");
                //www.SetRequestHeader("Content-Type", "application/json");
                //www.SetRequestHeader("Access-Control-Allow-Origin", "*");
                //www.Send();
                //wwwPostGame = www;
            }

            GameObject.Find("SurveyNext").GetComponentInChildren<Text>().text = "Next";
            qi = 0;
            doneSurvey = true;
            surveyData += "}";
            Debug.Log(surveyData);

            GameObject root = transform.root.gameObject;
            RaceUI raceui = (RaceUI)root.GetComponent(typeof(RaceUI));
            raceui.FinishMatch();
        }

        public void initLog()
        {
            if (writer == null)
            {
                try
                {
                    String settingsLogPath = Application.persistentDataPath + "/settingsLog.txt";
                    // Debug.Log("logging path: " + settingsLogPath);
                    writer = new StreamWriter(settingsLogPath, true);
                    writer.WriteLine("".PadLeft(80, '='));
                    writer.WriteLine("Starting a new Log");
                    writer.Flush();
                }
                catch (Exception exc)
                {
                    // Debug.Log(exc.StackTrace);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (prev_qi != qi)
            {
                if (qi < questions.Length && qi >= 0)
                {
                    qtext.GetComponent<UnityEngine.UI.Text>().text = questions[qi];
                }
                prev_qi = qi;
            }
        }
    }
}
