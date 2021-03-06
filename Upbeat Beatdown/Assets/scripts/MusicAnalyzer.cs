﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class MusicAnalyzer : MonoBehaviour {

    public double bpm = 140.0F;
    public float gain = 0.5F;
    public int maxLoopBeats = 4;
    public const float BEATOFFSET = 20000.0f;
    public int signatureLo = 4;
    public double nextTick = 0.0F;
    private float amp = 0.0F;
    private float phase = 0.0F;
    private double sampleRate = 0.0F;
    private double sample;
    private int beatCount;
    private int dataCount;
    private bool running = false;
    public float hitOff;

    void Start()
    {
        beatCount = maxLoopBeats;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;
        
        //spawn.AddListener(SpawnGround);
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
        sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;
        int dataCount = 0;
        while (dataCount < dataLen)
        {
            float x = gain * amp * Mathf.Sin(phase);
            int i = 0;
            while (i < channels)
            {
                data[dataCount * channels + i] += x;
                i++;
            }
            //Debug.Log(sample+n);
            
            // on beat
            while (sample + dataCount >= nextTick)
            {
                nextTick += samplesPerTick;
                //Debug.Log(samplesPerTick);
                amp = 1.0F;
                if (++beatCount > maxLoopBeats)
                {
                    beatCount = 1;
                    amp *= 2.0F;
                }
                // event for boss to spawn attack
                Debug.Log("on beat");
                
                // add offset travel time
                //Debug.Log("Tick: " + beatCount + "/" + maxLoopBeats);
            }
            //canSpawn = false;
            
            /*phase += amp * 0.3F;
            amp *= 0.993F;*/
            dataCount++;
        }
        // num ticks from beat hit
        hitOff = (float)(nextTick - (sample + dataCount));
    }

    void Update()
    {
    }

    public bool CheckBeat()
    {
        Debug.Log(hitOff);
        // checks if hti between beat offset
        if (hitOff < BEATOFFSET || nextTick - sample + dataCount > 75000)
        {
            // send event for window of player to hit beat
            return true;
        }
        return false;
    }
}
