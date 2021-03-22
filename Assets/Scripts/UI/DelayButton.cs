using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayButton : MonoBehaviour
{
    GameObject survey_button;
    // Start is called before the first frame update
    void Start()
    {
        survey_button = GameObject.Find("Survey_Button");
        survey_button.SetActive(false);
        Invoke("Delay", 15);
        //StartCoroutine(WaitToDisplay(5.0f));

    }

    public void Delay()
    {
        Debug.Log("Activate Survey Button : " + Time.time);
        survey_button.SetActive(true);
    }

    IEnumerator WaitToDisplay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        survey_button.SetActive(true);
    }
}
