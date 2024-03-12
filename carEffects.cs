using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carEffects : MonoBehaviourPun
{
    public Material brakeLights;
    public AudioSource skidClip;
    public AudioSource crashClip;
    public AudioSource scratchClip;
    public TrailRenderer[] tireMarks;
    public ParticleSystem[] smoke;
    
    
    private controller controller;
    private inputManager IM;
    private bool smokeFlag  = false , lightsFlag = false , tireMarksFlag;
    private bool scratching = false;

    
    private void Start() {      
        controller = GetComponent<controller>();
        IM = GetComponent<inputManager>();
        skidClip = gameObject.transform.Find("CarEffectAudioSource").transform.Find("skid").gameObject.GetComponent<AudioSource>();
        crashClip = gameObject.transform.Find("CarEffectAudioSource").transform.Find("collide").gameObject.GetComponent<AudioSource>();
        scratchClip = gameObject.transform.Find("CarEffectAudioSource").transform.Find("friction").gameObject.GetComponent<AudioSource>();
        brakeLights.DisableKeyword("_EMISSION");
        brakeLights.EnableKeyword("_EMISSION");

    }

    private void FixedUpdate() {
        chectDrift();
        activateSmoke();
        activateLights();
    }

    private void activateSmoke(){
        if (controller.playPauseSmoke) startSmoke();
        else stopSmoke();

        if (smokeFlag)
        {
            for (int i = 0; i < smoke.Length; i++)
            {
                var emission = smoke[i].emission;
                emission.rateOverTime = ((int)controller.KPH * 10 <= 2000) ? (int)controller.KPH * 10 : 2000;
            }
        }
    }

    public void startSmoke(){
        if(smokeFlag)return;
        for (int i = 0; i < smoke.Length; i++){
            var emission = smoke[i].emission;
            emission.rateOverTime = ((int) controller.KPH *2 >= 2000) ? (int) controller.KPH * 2 : 2000;
            smoke[i].Play();
        }
        skidClip.Play();
        smokeFlag = true;

    }

    public void stopSmoke(){
        if(!smokeFlag) return;
        for (int i = 0; i < smoke.Length; i++){
            smoke[i].Stop();
        }
        skidClip.Stop();
        smokeFlag = false;
    }
   

    private void activateLights() {
        if (IM.vertical < 0 || controller.KPH <= 1) turnLightsOn();
        else turnLightsOff();
    }

    private void turnLightsOn(){
        if (lightsFlag) return;
        brakeLights.SetColor("_EmissionColor", Color.red *5);
        lightsFlag = true;
       
    }    
    
    private void turnLightsOff(){
        if (!lightsFlag) return;
        brakeLights.SetColor("_EmissionColor", Color.black);
        lightsFlag = false;
       
    }

    private void chectDrift() {
        if (IM.handbrake) startEmitter();
        else stopEmitter();

    }

    private void startEmitter() {
        if (tireMarksFlag) return;
        foreach (TrailRenderer T in tireMarks) {
            T.emitting = true;
        }
        skidClip.Play();
        tireMarksFlag = true;
    }   
    private void stopEmitter() {
        if (!tireMarksFlag) return;
        foreach (TrailRenderer T in tireMarks)
        {
            T.emitting = false;
        }
        skidClip.Stop();
        tireMarksFlag = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("Track"))
            {
                //Debug.Log(controller.GetComponent<PhotonView>().Owner);
                crashClip.Play();
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("Track"))
            {

                if (!scratching)
                {
                    scratching = true;
                    scratchClip.Play();
                }

            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("Track"))
            {

                scratching = false;
                scratchClip.Stop();
            }
        }
    }

}
