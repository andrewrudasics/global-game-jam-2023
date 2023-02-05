using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupParticleSystemOnStop : MonoBehaviour
{
    public GameObject ObjectToDestroy;
    public void OnParticleSystemStopped()
    {
        if (ObjectToDestroy) {
            Destroy(ObjectToDestroy);
        } else {
            Destroy(this);
        }
    }
}
