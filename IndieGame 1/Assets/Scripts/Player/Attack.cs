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
    private float leftTriggerTimer = 0;
    private float rightTriggerTimer = 0;
    private float leftTriggerCD = 1;
    private float rightTriggerCD = 4;

    [Header("Samurai")]

    //Melee slash
    public ParticleSystem meleeAttack;
    public GameObject BoxCollider;
    private float deactivateTimer;
    private bool attacking;

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

    [Header("Geisha")]

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
    public ParticleSystem SoundBlastParticles;

    //Mana
    private float _mana;
    private float manaForAttackTwo = 60;
    private float manaForLeftTrigger = 60;
    private float manaForRightTrigger = 100;

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


    public System.Action<Attack> OnRBDown;
    public System.Action<Attack> OnRBUp;

    public System.Action<Attack> OnLBDown;
    public System.Action<Attack> OnLBUp;

    public System.Action<Attack> OnRTDown;
    public System.Action<Attack> OnRTUp;

    public System.Action<Attack> OnLTDown;
    public System.Action<Attack> OnLTUp;

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

        if (attacking) deactivateTimer += Time.deltaTime;
        if (deactivateTimer >= 0.2f)
        {
            deactivateTimer = 0;
            attacking = false;
        }
        if (attacking) BoxCollider.gameObject.SetActive(true);
        else BoxCollider.gameObject.SetActive(false);
        /*if (comboTimer > comboTimeLimit)
        {
            comboTimer = 0;
            comboCount = 0;
        }*/
        //print("Combo Count " + comboCount + " Old Combo Count " + oldComboCount);
        // print(oldComboCountTimer);

        if (character.CharacterClass == PlayerClass.Samurai) oldComboCountTimerCD = 0.35f;
        else if (character.CharacterClass == PlayerClass.Geisha) oldComboCountTimerCD = 0.5f;

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
            if (OnRBDown != null) OnRBDown.Invoke(this);
        }

        if (basicAttacksLeftToDo > 0) stillNeedsToAttackBasic = true;

        //print(basicAttacksLeftToDo);

        if (character.CharacterClass == PlayerClass.Geisha
            && stillNeedsToAttackBasic && attackTimer > waitBeforeNextAttack)
        {
            GameObject p = Instantiate(Projectile, ShootingPoint.transform.position, transform.rotation, null);
            p.transform.eulerAngles = new Vector3 (p.transform.eulerAngles.x, p.transform.eulerAngles.y, 90);
            mana += 5;
            attackTimer = 0;

            character.Heal(2);

            comboCount++;

            if (comboCount > 3)
            {
                comboCount = 0;
                GameObject pr = Instantiate(StrongProjectile, ShootingPoint.transform.position, transform.rotation, null);
                pr.transform.eulerAngles = new Vector3(pr.transform.eulerAngles.x, pr.transform.eulerAngles.y, 90);
                mana += 10;
            }

            basicAttacksLeftToDo--;
        }

        else if (GetComponent<CharacterStats>().CharacterClass == PlayerClass.Samurai
                 && stillNeedsToAttackBasic && attackTimer > waitBeforeNextAttack)
        {
            mana += 5;
            character.Heal(5);
            attackTimer = 0;

            attacking = true;
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

        if (character.CharacterClass == PlayerClass.Samurai) samuraiAnimator.SetFloat("AttackNumber", comboCount);
        else if (character.CharacterClass == PlayerClass.Geisha) geishaAnimator.SetFloat("AttackNumber", comboCount);
    }
    
    private void AttackOne()
    {
        if (Mathf.Round(Input.GetAxis("RBumperJ" + playerN)) > 0 && mana > manaForAttackTwo
            && attackTimer > waitBeforeNextAttack && dashTimer > dashCD)
        {
            if (character.CharacterClass == PlayerClass.Samurai)
            {
                GameObject ww = Instantiate(WhirwindBox, transform.position, transform.rotation, null);
                ww.GetComponent<AbilityDmg>().charStats = character;
                samuraiAnimator.SetTrigger("Heal");

                /*GameObject wp = */Instantiate(whirwind, transform.position, transform.rotation, null);

                if (OnLBDown != null) OnLBDown.Invoke(this);
            }
            else if (character.CharacterClass == PlayerClass.Geisha)
            {
                soundWaveIndicator.SetActive(true);
                jumpingBack = true;

                if (OnLBUp != null) OnLBUp.Invoke(this);
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

            /*GameObject sw = */Instantiate(JumpBackParticle, transform.position, transform.rotation, null);
            
           // sw.AddComponent<Rigidbody>();
            //sw.GetComponent<Rigidbody>().AddForce(transform.right);
            geishaAnimator.SetTrigger("JumpBack");

            jumpingBack = false;
        }

    }

    private void AttackLeftTrigger()
    {
        if (Mathf.Round(Input.GetAxis("TriggersJ" + playerN)) == 1 && attackTimer > waitBeforeNextAttack
            && mana >= manaForLeftTrigger && leftTriggerTimer > leftTriggerCD)
        {
            if (character.CharacterClass == PlayerClass.Samurai)
            {
                aimingTheDash = true;
                dashIndicator.SetActive(true);
            }
            else if (character.CharacterClass == PlayerClass.Geisha) teleporting = true;
        }

        if (teleporting && te == null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            rb.transform.rotation = Quaternion.Euler(Vector3.zero);
            te = Instantiate(Teleport, transform.position, Teleport.transform.rotation, null);
        }
        else if (te != null)
        {
            teleportDistance = transform.position - te.transform.position;
        }

        if (te != null && teleportDistance.magnitude <= teleportDistanceMax)
        {
            te.transform.Translate(Input.GetAxis("HorizontalCameraJ" + playerN) * 20 * -1 * Time.deltaTime,
                0, Input.GetAxis("VerticalCameraJ" + playerN) * 20 * Time.deltaTime, Space.World);
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

            if (OnLTUp != null) OnLTUp.Invoke(this);

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
            leftTriggerTimer = 0;
            mana -= manaForLeftTrigger;

            if (OnLTUp != null) OnLTUp.Invoke(this);

        }
    }

    private void AttackRightTrigger()
    {
        if (Mathf.Round(Input.GetAxis("TriggersJ" + playerN)) < 0 && attackTimer > waitBeforeNextAttack
            && mana >= manaForRightTrigger && rightTriggerTimer > rightTriggerCD)
        {
            if (character.CharacterClass == PlayerClass.Samurai)
            {
                aimingTheWindSlash = true;
                windSlashIndicator.SetActive(true);
            }
            else if (character.CharacterClass == PlayerClass.Geisha)
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

        if (chargeTimer > 0.5f && character.CharacterClass == PlayerClass.Samurai)
        {
            windSlashIndicator.SetActive(false);
            chargeTimer = 0;
            charching = false;
            GameObject ws = Instantiate(WindSlash, transform.position, transform.rotation, null);
            ws.GetComponent<AbilityDmg>().charStats = character;
            windSlashParticles.Play();

            if (OnRTDown != null) OnRTDown.Invoke(this);

        }
        else if (chargeTimer > 0.3f && character.CharacterClass == PlayerClass.Geisha)
        {
            chargeTimer = 0;
            charching = false;
            /*GameObject sw = */Instantiate(SoundBlast, transform.position, transform.rotation, null);
            //sw.GetComponent<AbilityDmg>().charStats = character;
            SoundBlastParticles.Play();

            if (OnRTDown != null) OnRTDown.Invoke(this);
        }
    }
}
