using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioController : MonoBehaviour
{

    private int clickTracker = 1;

    private float channelD, delayT;
    private RadioManager controller;
    private bool cooldownComplete;

    [SerializeField] private GameObject inLandText;
    [SerializeField] private GameObject ADFMText;
    [SerializeField] private GameObject theWaveText;
    [SerializeField] private GameObject offText; 

    private void Start()
    {
        controller = RadioManager.instance;
        channelD = controller.channelDelay;
        delayT = controller.delayTime;
    }

    private void Update()
    {
        if (channelD < 0.01f)
        {
            cooldownComplete = true;
        }
        else
        {
            channelD -= Time.deltaTime;
        }
    }

    public void OnChannelFlip(InputAction.CallbackContext context)
    {
        if (context.started && cooldownComplete)
        {
            channelD = delayT;

            Debug.Log(clickTracker);

            RadioManager.instance.ChangeStation(clickTracker);

            clickTracker++;

            if (clickTracker == 5)
            {
                clickTracker = 1;
            }

            channelD = delayT;
            cooldownComplete = false;
        }
    }
}
