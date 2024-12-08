using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wine : MonoBehaviour
{
    //Cinemachine 
    private CinemachineImpulseSource impluseSrouce;
    // Start is called before the first frame update
    void Start()
    {
        impluseSrouce = GetComponent<CinemachineImpulseSource>();
    }
    private void OnDestroy()
    {
        // Check if the AudioManager instance exists to prevent errors
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("GlassBreak");
            CameraShakeManager.instance.CameraShake(impluseSrouce);

        }
        else
        {
            Debug.LogWarning("AudioManager Instance not found. Cannot play SFX.");
        }
    }

}
