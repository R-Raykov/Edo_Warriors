using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [Tooltip("Projectile to be shot")]
    public GameObject Projectile;

    public GameObject StrongProjectile;

    [Tooltip("Where the projectile comes from")]
    public GameObject ShootingPoint;

    public GameObject WhirwindBox;
    public GameObject WindSlash;

    //Combo
    private int comboCount;
    private float comboTimer;
    private float comboTimeLimit = 3;

    //Saving the attacks (half of this is for animation as well, sorry i fucked up)
    private float attackTimer;
    private float waitBeforeNextAttack = 0.4f;
    private float saveCharacterAttacks;
    private float discardCA = 1f;
    private int basicAttacksLeftToDo;
    private bool stillNeedsToAttackBasic;
    private float oldComboCountTimer;
    private float oldComboCount;
    private bool isAttacking;
    private float oldComboCountTimerCD;

    //Trigger timers
    private float leftTriggerTimer = 5;
    private float rightTriggerTimer = 5;
    private float leftTriggerCD = 2;
    private float rightTriggerCD = 4;

    //Melee slash
    public ParticleSystem meleeAttack;

    //Dash
    private float dashTimer = 5;
    private float dashCD = 1.5f;
    public GameObject DashCollider;
    public ParticleSystem dashParticles;
    private bool aimingTheDash;

    //Whirwind effect
    public GameObject whirwind;

    //WindSlash effect
    public ParticleSystem windSlashParticles;
    private bool charching;
    private float chargeTimer;
    private bool aimingTheWindSlash;

    //Cone
    public GameObject DmgCone;
    public GameObject JumpBackParticle;
    private GameObject sw;
    private bool jumpingBack;

    //Teleport
    public GameObject Teleport;
    public GameObject TeleportCollider;
    private bool teleporting;
    private ParticleSystem teleportEffect;
    private GameObject te = null; //teleport, bad practice, should fix this later
    private Vector3 teleportDistance;

    //Sound blast
    public GameObject SoundBlast;

    //Mana
    private float _mana;
    private float manaForAttackTwo = 20;
    private float manaForLeftTrigger = 40;
    private float manaForRightTrigger = 60;

    private Rigidbody rb;
    private float speed;
    private CharacterStats character;

    private int playerN;

    private Animator samuraiAnimator;
    private Animator geishaAnimator;

    [SerializeField] private float teleportDistanceMax;
    [SerializeField] private float dashDistanceMax;

    [SerializeField] private GameObject dashIndicator;
    [SerializeField] private GameObject windSlashIndicator;
    [SerializeField] private GameObject soundWaveIndicator;


    public UnityEvent OnRBDown;
    public UnityEvent OnRBUp;

    public UnityEvent OnLBDown;
    public UnityEvent OnLBUp;

    public UnityEvent OnRTDown;
    public UnityEvent OnRTUp;

    public UnityEvent OnLTDown;
    public UnityEvent OnLTUp;

    // TEMPORARY FIX FOR NAME
    private float mana
    {
        get { return character.Mana; }
        set { character.Mana = value; }
    }

    private void Awake()
    {
        playerN = GetComponent<CharacterStats>().PlayerNumber;
        rb = GetComponent<Rigidbody>();
        speed = GetComponent<BasicMovement>().Speed;
        character = GetComponent<CharacterStats>();
        mana = character.Mana;

        dashIndicator.SetActive(false);                                                     // indicator for the dash
       // dashParticles = GameObject.Find("DashParticle").GetComponent<ParticleSystem>();     // particle for the dash

       // windSlashParticles = GameObject.Find("Wind Slash").GetComponent<ParticleSystem>();  // particle for the wind slash
        windSlashIndicator.SetActive(false);

        teleportEffect = GameObject.Find("TeleportEffect").GetComponent<ParticleSystem>();
        soundWaveIndicator.SetActive(false);

        samuraiAnimator = transform.Find("Samurai_Animated").GetComponent<Animator>();
        geishaAnimator = transform.Find("Geish3.0_Animated").GetComponent<Animator>();
    }

    private void Update()
    {
        if (mana > 200) mana = 200;

        if (basicAttacksLeftToDo < 0) basicAttacksLeftToDo = 0;

        ManageTimer();
        BasicAttack();
        AttackOne();
        AttackLeftTrigger();
        AttackRightTrigger();
    }

    private void ManageTimer()
    {
        comboTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        saveCharacterAttacks += Time.deltaTime;
        leftTriggerTimer += Time.deltaTime;
        rightTriggerTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;

        if (charching) chargeTimer += Time.deltaTime;
        if (isAttacking) oldComboCountTimer += Time.deltaTime;

        if (saveCharacterAttacks > discardCA) saveCharacterAttacks = 0;

        if (saveCharacterAttacks == 0)
        {
            basicAttacksLeftToDo = 0;
        }

        /*if (comboTimer > comboTimeLimit)
        {
            comboTimer = 0;
            comboCount = 0;
        }*/
        //print("Combo Count " + comboCount + " Old Combo Count " + oldComboCount);
        // print(oldComboCountTimer);

        if (character.CharacterClass == CharacterStats.CharType.Samurai) oldComboCountTimerCD = 0.7f;
        else if (character.CharacterClass == CharacterStats.CharType.Geisha) oldComboCountTimerCD = 0.5f;

        if (comboCount != oldComboCount) oldComboCountTimer = 0;
        else if (comboCount == oldComboCount && oldComboCountTimer > oldComboCountTimerCD && comboCount > 0)
        {
            oldComboCountTimer = 0;
            comboCount = 0;
            //oldComboCount = 0;
        }
        else if (comboCount <= 0) isAttacking = false;
    }

    private void BasicAttack()
    {
        oldComboCount = comboCount;

        if (Input.GetAxis("LBumperJ" + playerN) > 0 && attackTimer > waitBeforeNextAttack)
        {
            basicAttacksLeftToDo++;
            isAttacking = true;
        }

        if (basicAttacksLeftToDo > 0) stillNeedsToAttackBasic = true;

        //print(basicAttacksLeftToDo);

        if (character.CharacterClass == CharacterStats.CharType.Geisha
            && stillNeedsToAttackBasic && attackTimer > waitBeforeNextAttack)
        {
            GameObject p = Instantiate(Projectile, ShootingPoint.transform.position, transform.rotation, null);
            mana += 5;
            attackTimer = 0;

            comboCount++;

            if (comboCount > 3)
            {
                comboCount = 0;
                GameObject pr = Instantiate(StrongProjectile, ShootingPoint.transform.position, transform.rotation, null);
                mana += 10;
            }

            basicAttacksLeftToDo--;
        }

        else if (GetComponent<CharacterStats>().CharacterClass == CharacterStats.CharType.Samurai
                 && stillNeedsToAttackBasic && attackTimer > waitBeforeNextAttack)
        {
            mana += 5;
            character.Heal(2);
            attackTimer = 0;

            meleeAttack.Play();     //playing the slash particle

            comboCount++;

            if (comboCount > 3)
            {
                comboCount = 0;
                //GameObject pr = Instantiate(Projectile, ShootingPoint.transform.position, transform.rotation, null);
                // pr.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                mana += 10;
            }

            basicAttacksLeftToDo--;

        }

        if (basicAttacksLeftToDo <= 0) stillNeedsToAttackBasic = false;

        if (character.CharacterClass == CharacterStats.CharType.Samurai) samuraiAnimator.SetFloat("AttackNumber", comboCount);
        else if (character.CharacterClass == CharacterStats.CharType.Geisha) geishaAnimator.SetFloat("AttackNumber", comboCount);
    }
    
    private void AttackOne()
    {
        if (Mathf.Round(Input.GetAxis("RBumperJ" + playerN)) > 0 && mana > manaForAttackTwo
            && attackTimer > waitBeforeNextAttack && dashTimer > dashCD)
        {
            if (character.CharacterClass == CharacterStats.CharType.Samurai)
            {
                GameObject ww = Instantiate(WhirwindBox, transform.position, transform.rotation, null);
                ww.GetComponent<AbilityDmg>().charStats = character;
                samuraiAnimator.SetTrigger("Heal");

                GameObject wp = Instantiate(whirwind, transform.position, transform.rotation, null);
            }
            else if (character.CharacterClass == CharacterStats.CharType.Geisha)
            {
                soundWaveIndicator.SetActive(true);
                jumpingBack = true;
            }

            mana -= manaForAttackTwo;
            attackTimer = 0;
            dashTimer = 0;
        }

        if (jumpingBack && Mathf.Round(Input.GetAxis("RBumperJ" + playerN)) < 1)
        {
            soundWaveIndicator.SetActive(false);
            rb.velocity = transform.forward * 8 * -1;
            GameObject p = Instantiate(DmgCone, transform.position, transform.rotation, null);
            p.transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
            p.GetComponent<AbilityDmg>().charStats = character;

            GameObject sw = Instantiate(JumpBackParticle, transform.position, transform.rotation, null);
            
           // sw.AddComponent<Rigidbody>();
            //sw.GetComponent<Rigidbody>().AddForce(transform.right);
            geishaAnimator.SetTrigger("JumpBack");

            jumpingBack = false;
        }

    }

    private void AttackLeftTrigger()
    {
        if (Mathf.Round(Input.GetAxis("TriggersJ" + playerN)) == 1 /*&& attackTimer > waitBeforeNextAttack*/
            && mana >= manaForLeftTrigger && leftTriggerTimer > leftTriggerCD)
        {
            if (character.CharacterClass == CharacterStats.CharType.Samurai)
            {
                aimingTheDash = true;
                dashIndicator.SetActive(true);
            }
            else if (character.CharacterClass == CharacterStats.CharType.Geisha) teleporting = true;
        }

        if (teleporting && te == null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.transform.rotation = Quaternion.Euler(Vector3.zero);
            te = Instantiate(Teleport, transform.position, Teleport.transform.rotation, null);
        }
        else if (te != null)
        {
            teleportDistance = transform.position - te.transform.position;
        }

        if (te != null && teleportDistance.magnitude <= teleportDistanceMax)
        {
            te.transform.Translate(Input.GetAxis("HorizontalCameraJ" + playerN) * 10 * -1 * Time.deltaTime,
                0, Input.GetAxis("VerticalCameraJ" + playerN) * 10 * Time.deltaTime, Space.World);
        }
        else if (te != null && teleportDistance.magnitude > teleportDistanceMax)
        {
            Vector3 v = te.transform.position - transform.position;
            v *= teleportDistanceMax / teleportDistance.magnitude;
            te.transform.position = transform.position + v;
        }


        if (teleporting && Mathf.Round(Input.GetAxis("TriggersJ" + playerN)) == 0)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            //rb.MovePosition(Teleport.transform.position);
            transform.position = te.transform.position;
            teleporting = false;
            GameObject t = Instantiate(TeleportCollider, transform.position, transform.rotation, null);
            t.GetComponent<AbilityDmg>().charStats = character;
            if(teleportEffect != null) teleportEffect.Play();
            geishaAnimator.SetTrigger("Teleport");
            Destroy(te.gameObject);
            mana -= manaForLeftTrigger;
            //attackTimer = 0;
            leftTriggerTimer = 0;
            character.Heal(5);
        }


        if (Mathf.Round(Input.GetAxis("TriggersJ" + playerN)) < 1 && aimingTheDash)
        {
            dashIndicator.SetActive(false);
            if(dashParticles != null) dashParticles.Play();       //play the dash particle

            GameObject d = Instantiate(DashCollider, transform.position, transform.rotation, this.gameObject.transform);
            rb.velocity = transform.forward * dashDistanceMax;
            d.GetComponent<AbilityDmg>().charStats = character;
            samuraiAnimator.SetTrigger("Dash");
            aimingTheDash = false;
        }
    }

    private void AttackRightTrigger()
    {
        if (Mathf.Round(Input.GetAxis("TriggersJ" + playerN)) < 0 && attackTimer > waitBeforeNextAttack
            && mana >= manaForRightTrigger && rightTriggerTimer > rightTriggerCD)
        {
            if (character.CharacterClass == CharacterStats.CharType.Samurai)
            {
                aimingTheWindSlash = true;
                windSlashIndicator.SetActive(true);
            }
            else if (character.CharacterClass == CharacterStats.CharType.Geisha)
            {
                charching = true;
                geishaAnimator.SetTrigger("SoundBlast");
            }

            mana -= manaForRightTrigger;
            attackTimer = 0;
            rightTriggerTimer = 0;
        }

        if (Mathf.Round(Input.GetAxis("TriggersJ" + playerN)) > -1 && aimingTheWindSlash)
        {
            charching = true;
            samuraiAnimator.SetTrigger("Slash");
            aimingTheWindSlash = false;
        }

        if (chargeTimer > 0.5f && character.CharacterClass == CharacterStats.CharType.Samurai)
        {
            windSlashIndicator.SetActive(false);
            chargeTimer = 0;
            charching = false;
            GameObject ws = Instantiate(WindSlash, transform.position, transform.rotation, null);
            ws.GetComponent<AbilityDmg>().charStats = character;
            windSlashParticles.Play();
        }
        else if (chargeTimer > 0.3f && character.CharacterClass == CharacterStats.CharType.Geisha)
        {
            chargeTimer = 0;
            charching = false;
            GameObject sw = Instantiate(SoundBlast, transform.position, transform.rotation, null);
            //sw.GetComponent<AbilityDmg>().charStats = character;
        }
    }
}
