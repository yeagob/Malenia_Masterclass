using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MaleniaAttacks : MonoBehaviour, INextMove
{
    [Header("Control")]
    public bool AI;
    public bool debug;

    [Header("References")]
    public GameObject impactPrefab;
    public Transform model;
    public Transform player;
    public BoxCollider leftFootCollider;
    private Animator playerAnim;
    public DamageDealer prosthesisBlade;
    public DamageDealer kickDamageDealer;
    public CameraShaker shaker;
    public GameManagerScript gameManager;
    public BossLifeBarScript bossLifeBar;

    [Header("Attacks")]
    public GameObject swordGlow;

    private Animator anim;

    [Header("Debug")]
    public GameObject brainIcon;
    public Image bossAttackingDebug;
    public Image bossMovingDebug;
    public Text walkTimeDebug;
    public Text distanceDebug;
    public Text brainDebug;
    public Text damageDebug;
    public Text speedText;
    public Color farColor;
    public Color middleColor;
    public Color nearColor;

    [Header("AI Manager")]
    public float nearValue;
    public float farValue;
    public float chillTime;
    private string action;
    private float lastActionTime;
    private float distance;
    private float chillDirection;
    private bool canBeginAI;
    private int lastAttack;

    [Header("Waterfowl Dance")]
    private bool waterfowlUsed;
    private float waterfowlHPThreshold = 0.75f;
    private float waterfowlMinDistance = 8f;
    private float waterfowlMaxDistance = 15f;

    private bool slowDown;
    private string actionAfterSlowDown;

    private void Start()
    {
        anim = model.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        
        SetupLifestealRatios();
    }

    private void SetupLifestealRatios()
    {
        if (prosthesisBlade != null)
        {
            prosthesisBlade.lifestealRatio = 0.016f;
            prosthesisBlade.maleniaAttacks = this;
        }
        
        if (kickDamageDealer != null)
        {
            kickDamageDealer.lifestealRatio = 0.016f;
            kickDamageDealer.maleniaAttacks = this;
        }
    }
 
    
    private void Update()
    {
        speedText.text = anim.GetFloat("Vertical").ToString("0.0");

        if (Input.GetKeyDown(KeyCode.Keypad0) && gameManager.master) AI = !AI;
        if (Input.GetKeyDown(KeyCode.Keypad1) && gameManager.master) debug = true;
        brainIcon.gameObject.SetActive(AI);

        distance = Vector3.Distance(model.transform.position, player.transform.position);

        this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (debug)
            DebugUI();

        if (distance < 20 && !anim.GetBool("Equipped"))
        {
            anim.SetTrigger("DrawSword");
            StartCoroutine(StartAI());
        }

        if (!anim.GetBool("Equipped")) return;

        if (!canBeginAI) return;

        if (AI && !playerAnim.GetBool("Dead"))
        {
            AI_Manager();
        }
        else if (gameManager.master)
        {
            DebugAttack();
        }
        else
        {
            anim.SetBool("GameEnd", true);
            anim.SetBool("CanRotate", false);
        }

        prosthesisBlade.damageOn = anim.GetBool("Attacking");
    }

    IEnumerator StartAI()
    {
        yield return new WaitForSeconds(4);
        canBeginAI = true;
    }

    private void DebugUI()
    {
        speedText.transform.parent.gameObject.SetActive(true);
        distanceDebug.transform.parent.gameObject.SetActive(true);
        brainDebug.transform.parent.gameObject.SetActive(true);
        damageDebug.text = prosthesisBlade.damageAmount.ToString();
        bossAttackingDebug.gameObject.SetActive(anim.GetBool("Attacking"));
        bossMovingDebug.gameObject.SetActive(action == "Move");
        distanceDebug.text = distance.ToString("0.0");
        if (distance <= nearValue) distanceDebug.color = nearColor;
        else if (distance >= farValue) distanceDebug.color = farColor;
        else distanceDebug.color = middleColor;
    }

    private void FarAttack()
    {
        brainDebug.text = "Far Attack";
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);

        if (CanUseWaterfowlDance())
        {
            anim.SetTrigger("WaterfowlDance");
            waterfowlUsed = true;
            action = "Wait";
            return;
        }

        int rand = 0;
        do
        {
            rand = Random.Range(0, 2);
        } while (rand == lastAttack);
        lastAttack = rand;

        switch (rand)
        {
            case 0:
                anim.SetTrigger("OverheadSlam");
                break;
            case 1:
                anim.SetTrigger("ChargeAndSpin");
                break;
            default:
                break;
        }

        action = "Wait";
    }

    private void NearAttack()
    {
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);

        int rand = 0;
        do
        {
            rand = Random.Range(0, 6);
        } while (rand == lastAttack);
        lastAttack = rand;

        switch (rand)
        {
            case 0:
                anim.SetTrigger("FiveHitCombo");
                brainDebug.text = "Five Hit Combo";
                break;
            case 1:
                anim.SetTrigger("DoubleThrust");
                brainDebug.text = "Double Thrust";
                break;
            case 2:
                anim.SetTrigger("Kick");
                brainDebug.text = "Kick";
                break;
            case 3:
                anim.SetTrigger("UppercutSlam");
                brainDebug.text = "Uppercut Slam";
                break;
            case 4:
                anim.SetTrigger("RunningThrust");
                brainDebug.text = "Running Thrust";
                break;
            case 5:
                anim.SetTrigger("Thrust");
                brainDebug.text = "Thrust";
                break;
            case 6:
                //Not implemented
                anim.SetTrigger("RapidSlices");
                brainDebug.text = "Rapid Slices";
                break;
            case 7:
                //Not implemented
                anim.SetTrigger("Grab");
                brainDebug.text = "Grab";
                break;
 
            default:
                break;
        }

        action = "Wait";
    }

    private bool CanUseWaterfowlDance()
    {
        if (waterfowlUsed) return false;
        
        float currentHPRatio = bossLifeBar.GetBossLifeAmount() / bossLifeBar.maxLife;
        bool hpCondition = currentHPRatio <= waterfowlHPThreshold;
        bool distanceCondition = distance >= waterfowlMinDistance && distance <= waterfowlMaxDistance;

        return hpCondition && distanceCondition;
    }

    private void SlowBossDown()
    {
        if (anim.GetFloat("Vertical") <= 0.4f)
        {
            slowDown = false;
            if (actionAfterSlowDown == "CallNextMove")
            {
                action = "Wait";
                anim.SetFloat("Vertical", 0);
                anim.SetFloat("Horizontal", 0);
                StartCoroutine(WaitAfterNearMove());
            }
            else if (actionAfterSlowDown == "FarAttack")
            {
                action = "FarAttack";
            }
            else
            {
                Debug.LogError("Not supposed to be here");
            }
        }
        else
        {
            brainDebug.text = "SlowDown";
            anim.SetFloat("Vertical", Mathf.Lerp(anim.GetFloat("Vertical"), 0, 1 * Time.deltaTime));
        }
    }

    IEnumerator WaitAfterNearMove()
    {
        brainDebug.text = "WaitRandomly";
        slowDown = false;
        action = "Wait";
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);
        float maxWaitTime = 6;
        float possibility = 2;
        float waitTime;
        float decision = Random.Range(0, possibility);
        if (decision == 0) waitTime = Random.Range(2.5f, maxWaitTime);
        else waitTime = 0;
        yield return new WaitForSeconds(waitTime);
        action = "NearAttack";
        CallNextMove();
    }

    private void MoveToPlayer()
    {
        brainDebug.text = "Move";

        anim.SetFloat("Horizontal", 0);

        float speedValue = distance / 15;
        if (speedValue > 1) speedValue = 1;

        walkTimeDebug.text = (Time.time - lastActionTime).ToString("0.0");

        if (slowDown)
        {
            SlowBossDown();
            return;
        }

        if (distance < nearValue)
        {
            actionAfterSlowDown = "CallNextMove";
            slowDown = true;
        }
        else if (Time.time - lastActionTime > chillTime)
        {
            actionAfterSlowDown = "FarAttack";
            slowDown = true;
        }
        else
        {
            anim.SetFloat("Vertical", speedValue);
        }
    }

    private void WaitForPlayer()
    {
        brainDebug.text = "Chill";

        anim.SetFloat("Horizontal", chillDirection);
        anim.SetFloat("Vertical", 0);

        if (distance <= nearValue && Time.time - lastActionTime > chillTime)
        {
            CallNextMove();
        }
        else if (distance > farValue && Time.time - lastActionTime > chillTime)
        {
            FarAttack();
        }
        else if (Time.time - lastActionTime > chillTime)
        {
            int rand = Random.Range(0, 3);

            if (rand % 2 == 0)
            {
                NearAttack();
            }
            else if (rand % 2 == 1)
            {
                FarAttack();
            }
        }
    }

    private void AI_Manager()
    {
        if (action == "Wait" || anim.GetBool("Dead") || anim.GetBool("Transposing")) return;

        if (action == "Move")
        {
            MoveToPlayer();
        }

        if (action == "WaitForPlayer")
        {
            WaitForPlayer();
        }

        if (action == "FarAttack")
        {
            if (!anim.GetBool("TakingDamage"))
                FarAttack();
        }

        if (action == "NearAttack")
        {
            if (!anim.GetBool("TakingDamage"))
            {
                NearAttack();
            }
        }
    }

    public void CallNextMove()
    {
        lastActionTime = Time.time;

        if (distance >= farValue && !anim.GetBool("Dead"))
        {
            action = "Move";
        }
        else if (distance > nearValue && distance < farValue && !anim.GetBool("Dead"))
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) chillDirection = -0.5f;
            if (rand == 1) chillDirection = 0.5f;
            action = "WaitForPlayer";
        }
        else if (distance <= nearValue && !anim.GetBool("Dead"))
        {
            action = "NearAttack";
        }
    }

    private bool IsBossTakingDamage()
    {
        return !anim.GetCurrentAnimatorStateInfo(2).IsName("none");
    }

    #region Debug

    private void DebugAttack()
    {
        anim.SetFloat("Vertical", 0);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("FiveHitCombo");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("DoubleThrust");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("Kick");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            anim.SetTrigger("Thrust");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            anim.SetTrigger("UppercutSlam");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            anim.SetTrigger("RunningThrust");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            anim.SetTrigger("WaterfowlDance");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            anim.SetTrigger("OverheadSlam");
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            anim.SetTrigger("ChargeAndSpin");
        }

    }

    #endregion

    #region Lifesteal System

    public void ApplyLifesteal(float healRatio)
    {
        if (bossLifeBar == null) return;

        float healAmount = bossLifeBar.maxLife * healRatio;
        bossLifeBar.UpdateLife(healAmount);
    }

    #endregion



    #region Kick

    public void TurnKickColliderOn()
    {
        leftFootCollider.enabled = true;
        leftFootCollider.GetComponent<DamageDealer>().damageOn = true;
    }

    public void TurnKickColliderOff()
    {
        leftFootCollider.enabled = false;
        leftFootCollider.GetComponent<DamageDealer>().damageOn = false;
    }

    #endregion

    private void SetNotAttackingFalse()
    {
        anim.SetBool("NotAttacking", false);
    }

    private void SetNotAttackingTrue()
    {
        anim.SetBool("NotAttacking", true);
    }

    private void SetTrackingPlayerTrue()
    {
        anim.SetBool("CanRotate", true);
    }

    private void SetTrackingPlayerFalse()
    {
        anim.SetBool("CanRotate", false);
    }
    private void Impact()
    {
        GameObject impactObj = Instantiate(impactPrefab, transform.position, Quaternion.identity);
        Destroy(impactObj, 1.5f);
        shaker.ShakeCamera(0.5f);
    }
    
    private void CastAura()
    {
        
    }

    private void GlowSwordTrue()
    {
        swordGlow.SetActive(true);
    }
    
    private void GlowSwordFalse()
    {
        swordGlow.SetActive(false);
    }
}

public interface INextMove
{
    public void CallNextMove();
}
