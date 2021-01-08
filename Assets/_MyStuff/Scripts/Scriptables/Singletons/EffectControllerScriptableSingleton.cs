using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Singleton/EffectController")]
    public class EffectControllerScriptableSingleton : ScriptableSingleton<EffectControllerScriptableSingleton>
    {

        public SmokeObject smokePuff;

        public GameObject bloodPrefab;

        public AudioClip[] clips;
        public AudioClip[] pcshshClips;
        public AudioClip[] wooshclips;
        public AudioClip[] crunchClips;


        public AudioClip[] pickupClip;
        //
        public float minVolume = 0.2f;
        public float maxVolume = 0.3f;
        //
        public float minVelocity = 1;
        public float maxVelocity = 10;
        //
        protected float lastImpactTime = 0.5f;
        public float minImpactDelay = 0.15f;
        //
        public float pitch = 1;

        // 
        public AudioMixerGroup mixerGroup;
        public Camera camTransform;

        public MultiFighterCamera mfCam;
        public Fighter3DCamera f3dCam;
        // How long the object should shake for.
        //public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        //


        public float Magnitude = 2f;
        public float Roughness = 10f;
        public float FadeOutTime = 5f;

        public void CreateSmokePuffs(int count, Vector3 position, float force, Vector3 baseVelocity, float startOffset)
        {
            if (Instance != null)
            {
                float startAngle = Random.value * 3;
                for (int i = 0; i < count; i++)
                {
                    Vector2 direction = MathFL.Point2OnCircle(startAngle + Mathf.PI * 2 * (i / (float)count), i % 2 == 0 ? force * (0.8f + Random.value * 0.2f) : force * 0.99f * (0.9f + Random.value * 0.1f));
                    SmokeObject smoke = Instantiate(smokePuff, position + new Vector3(direction.x * startOffset, 0, direction.y * startOffset), Quaternion.identity) as SmokeObject;
                    smoke.deathOnGroundCollision = false;
                    Vector3 rVelocity = Random.onUnitSphere * 0.2f;
                    if (rVelocity.y < 0) rVelocity *= -1;
                    rVelocity.y += force * 0.0f;
                    smoke.Setup(new Vector3(direction.x, 0, direction.y) + rVelocity);
                }
            }
            else
            {
                Debug.LogError("No Effects Controller Instance in scene.");
            }
        }

        public void CreateBloodEffect(Vector3 position)
        {
            GameObject blood = Instantiate(bloodPrefab, position, Quaternion.identity);
        }

        public void PlayImpactSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
        {
            if (relativeVolocityMagnitude > minVelocity)
            {
                //
                float m = Mathf.Clamp01((relativeVolocityMagnitude - minVelocity) / (maxVelocity - minVelocity));
                float volumeM = minVolume + (maxVolume - minVolume) * m;
                //
                GameObject gO = new GameObject("OneShotAudio");
                gO.transform.position = collisionPoint;
                AudioSource source = gO.AddComponent<AudioSource>();
                source.loop = false;
                source.dopplerLevel = 0.1f;
                source.volume = volumeM;
                source.pitch = pitch;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.minDistance = 2000;
                source.maxDistance = 2010;
                source.spatialBlend = 0.5f;
                source.outputAudioMixerGroup = mixerGroup;
                int clipIndex = Mathf.FloorToInt(m * 0.999f * clips.Length);
                if (clipIndex < clips.Length && clipIndex >= 0)
                {
                    source.clip = clips[Mathf.FloorToInt(m * 0.999f * clips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
                    Debug.Log("clipIndex " + clipIndex + "  clips.Length " + clips.Length);
                }
                else
                {
                    Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + clips.Length);
                }
                source.Play();
                //
                Destroy(gO, source.clip.length + 0.1f);
                //
            }
        }

        public void PlayBreakSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
        {
            if (relativeVolocityMagnitude > minVelocity)
            {
                //
                float m = Mathf.Clamp01((relativeVolocityMagnitude - minVelocity) / (maxVelocity - minVelocity));
                float volumeM = minVolume + (maxVolume - minVolume) * m;
                //
                GameObject gO = new GameObject("OneShotAudio");
                gO.transform.position = collisionPoint;
                AudioSource source = gO.AddComponent<AudioSource>();
                source.loop = false;
                source.dopplerLevel = 0.1f;
                source.volume = volumeM;
                source.pitch = pitch;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.minDistance = 2000;
                source.maxDistance = 2010;
                source.spatialBlend = 0.5f;
                source.outputAudioMixerGroup = mixerGroup;
                int clipIndex = Mathf.FloorToInt(m * 0.999f * crunchClips.Length);
                if (clipIndex < crunchClips.Length && clipIndex >= 0)
                {
                    source.clip = crunchClips[Mathf.FloorToInt(m * 0.999f * crunchClips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
                    Debug.Log("clipIndex " + clipIndex + "  clips.Length " + crunchClips.Length);
                }
                else
                {
                    Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + crunchClips.Length);
                }
                source.Play();
                //
                Destroy(gO, source.clip.length + 0.1f);
                //
            }
        }

        /*public void Sleep(float x)
        {
            // sleep 
            Instance.StartCoroutine(Instance.SleepTime(x));
        }*/

        /*public IEnumerator SleepTime(float x)
        {
            //####MAKE DEAD coroutine
            Time.timeScale = .0000001f;
            //print ("before pause : "+x+" : "+Time.time);
            yield return new WaitForSeconds(x * Time.timeScale);
            //print ("after pause : "+x+" : "+Time.time);
            Time.timeScale = 1;
            
        }*/

        public void PlayBloodSpurtSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
        {
            if (relativeVolocityMagnitude > minVelocity)
            {
                //
                float m = Mathf.Clamp01((relativeVolocityMagnitude - minVelocity) / (maxVelocity - minVelocity));
                float volumeM = minVolume + (maxVolume - minVolume) * m;
                //
                GameObject gO = new GameObject("OneShotAudio");
                gO.transform.position = collisionPoint;
                AudioSource source = gO.AddComponent<AudioSource>();
                source.loop = false;
                source.dopplerLevel = 0.1f;
                source.volume = volumeM;
                source.pitch = pitch;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.minDistance = 2000;
                source.maxDistance = 2010;
                source.spatialBlend = 0.5f;
                source.outputAudioMixerGroup = mixerGroup;
                int clipIndex = Mathf.FloorToInt(m * 0.999f * pcshshClips.Length);
                if (clipIndex < pcshshClips.Length && clipIndex >= 0)
                {
                    source.clip = pcshshClips[Mathf.FloorToInt(m * 0.999f * pcshshClips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
                    Debug.Log("clipIndex " + clipIndex + "  clips.Length " + pcshshClips.Length);
                }
                else
                {
                    Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + pcshshClips.Length);
                }
                source.Play();
                //
                Destroy(gO, source.clip.length + 0.1f);
                //
            }
        }

        public void PlayWooshSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
        {
            if (relativeVolocityMagnitude > minVelocity)
            {
                //
                float m = Mathf.Clamp01((relativeVolocityMagnitude - minVelocity) / (maxVelocity - minVelocity));
                float volumeM = minVolume + (maxVolume - minVolume) * m;
                //
                GameObject gO = new GameObject("OneShotAudio");
                gO.transform.position = collisionPoint;
                AudioSource source = gO.AddComponent<AudioSource>();
                source.loop = false;
                source.dopplerLevel = 0.1f;
                source.volume = volumeM;
                source.pitch = pitch;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.minDistance = 2000;
                source.maxDistance = 2010;
                source.spatialBlend = 0.5f;
                source.outputAudioMixerGroup = mixerGroup;
                int clipIndex = Mathf.FloorToInt(m * 0.999f * wooshclips.Length);
                if (clipIndex < wooshclips.Length && clipIndex >= 0)
                {
                    source.clip = wooshclips[Mathf.FloorToInt(m * 0.999f * wooshclips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
                }
                else
                {
                    Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + wooshclips.Length);
                }
                source.Play();
                //
                Destroy(gO, source.clip.length + 0.1f);
                //
            }
        }
    }
}

