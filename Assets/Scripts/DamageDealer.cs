using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public bool damageOn;
    public float damageAmount;
    public float lifestealRatio;
    public MaleniaAttacks maleniaAttacks;
    public AudioClip[] impactSound;

    private float lastSoundTime = 0;

    public float GetDamage()
    {
        return damageAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageOn) return;

        if (other.gameObject.layer != 11 && other.gameObject.layer != 13) return;

        if (other.gameObject.name == "Girl")
        {
            if (other.GetComponent<Animator>().GetBool("Intangible")) return;
            other.transform.GetComponentInParent<PlayerScript>().RegisterDamage(damageAmount);
            
            if (maleniaAttacks != null && lifestealRatio > 0)
            {
                maleniaAttacks.ApplyLifesteal(lifestealRatio);
            }
        }

        if (SoundInterval() && impactSound.Length > 0)
        {
            SoundManager.CreateAndPlay(impactSound[Random.Range(0, impactSound.Length)], GameObject.FindGameObjectWithTag("SoundManager").gameObject, other.transform, 2);
            lastSoundTime = Time.time;
        }
    }

    public void GreatSwordFiller(GameObject other)
    {
        if (!damageOn) return;

        if (other.gameObject.layer != 11 && other.gameObject.layer != 13) return;

        if (other.gameObject.name == "Girl")
        {
            if (other.GetComponent<Animator>().GetBool("Intangible")) return;
            other.transform.GetComponentInParent<PlayerScript>().RegisterDamage(damageAmount);
            
            if (maleniaAttacks != null && lifestealRatio > 0)
            {
                maleniaAttacks.ApplyLifesteal(lifestealRatio);
            }
        }

        if (SoundInterval() && impactSound.Length > 0)
        {
            SoundManager.CreateAndPlay(impactSound[Random.Range(0, impactSound.Length)], GameObject.FindGameObjectWithTag("SoundManager").gameObject, other.transform, 2);
            lastSoundTime = Time.time;
        }
    }

    private bool SoundInterval()
    {
        return Time.time > lastSoundTime + 0.5f;
    }

}
