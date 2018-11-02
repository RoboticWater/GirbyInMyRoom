using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public float transitionOut = 0.5f;
	public float transitionIn = 0.5f;
    private Image[] images;

    private Toggle menuToggle;
	private float timeScaleRef = 1f;
    private float volumeRef = 1f;
    private bool paused;

    private TextMeshProUGUI[] menuText;
    private List<Animator> pausedAnimators;

    void Awake()
    {
        menuToggle = GetComponent <Toggle> ();
        images = GetComponentsInChildren<Image>(true);
        menuText = GetComponentsInChildren<TextMeshProUGUI>(true);

        Physics.autoSimulation = true;
	}

    void Update()
	{
		if(Inputs.togglePause)
		{
		    menuToggle.isOn = !menuToggle.isOn;
            Cursor.visible = menuToggle.isOn;//force the cursor visible if anythign had hidden it
		}
	}

    private void MenuOn ()
    {
        //timeScaleRef = Time.timeScale;
        //Time.timeScale = 0f;

        volumeRef = AudioListener.volume;
        AudioListener.volume = 0f;

        paused = true;
    }


    public void MenuOff ()
    {
        //Time.timeScale = timeScaleRef;
        AudioListener.volume = volumeRef;
        paused = false;
    }


    public void OnMenuStatusChange ()
    {
        
        if (menuToggle.isOn && !paused)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(TransitionOut());
            PauseScene(true);
            MenuOn();
        }
        else if (!menuToggle.isOn && paused)
        {
            StartCoroutine(TransitionIn());
            
            MenuOff();
            PauseScene(false);
        }
    }

    public void PauseScene(bool pause) {

        Animator[] components = GameObject.FindObjectsOfType<Animator>();
        
        if (pause) {
            Physics.autoSimulation = false;
            pausedAnimators = new List<Animator>();
            
            foreach(Animator a in components) {
                if (a.enabled && a.transform.tag != "MenuUI") {
                    pausedAnimators.Add(a);
                    a.enabled = false;
                }
                
            }
            
        } else {     
            foreach(Animator a in pausedAnimators) {
                a.enabled = true;
            }
            Physics.autoSimulation = true;
        }
        
    }

	public IEnumerator TransitionOut() {
		float alpha = 0.0f;
		float timeElapsed = 0.0f;

		while (timeElapsed < transitionOut) {
			timeElapsed += Time.deltaTime;
			alpha = Mathf.Clamp01(timeElapsed/transitionOut);
            foreach(Image i in images) {
                if (i.transform.name != "LevelLoader") {
                    Color clr = i.color;
                    clr.a = alpha;
                    i.color = clr;
                }
            }

            foreach(TextMeshProUGUI t in menuText) {
                Color clr = t.color;
                clr.a = alpha;
                t.color = clr;
            }
			yield return null;
		}
	}

	IEnumerator TransitionIn() {
		float alpha = 1.0f;
		float timeRemaining = transitionIn;

		while (timeRemaining > 0 ) {
			timeRemaining -= Time.deltaTime;
			alpha = Mathf.Clamp01(timeRemaining/transitionIn);
			foreach(Image i in images) {
                if (i.transform.name != "LevelLoader") {
                    Color clr = i.color;
                    clr.a = alpha;
                    i.color = clr;
                }
            }
            foreach(TextMeshProUGUI t in menuText) {
                Color clr = t.color;
                clr.a = alpha;
                t.color = clr;
            }
			yield return null;
		}
        transform.GetChild(0).gameObject.SetActive(false);
	}
	

}
