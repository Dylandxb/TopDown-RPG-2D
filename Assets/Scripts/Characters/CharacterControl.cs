using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    
    Rigidbody2D rb;
    Animator anim;
    public Transform inventoryUI;
    private static CharacterControl instance;
    public static CharacterControl MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterControl>();
            }
            return instance;
        }
    }

    [SerializeField] public Stat Hunger;

    public float maxHunger = 100;

    [SerializeField] private Stat Mana;

    private float maxMana = 75;

    [SerializeField] private Stat Health;

    private float maxHealth = 100;

    private float hungerFallRate = 0.5f; //Rate of change for hunger proportional to time
    private float healthFallRate = 1f; // Rate of change for health proportional to time


    void Start()
    {
        Hunger.Initialize(maxHunger, maxHunger); //At the beginning of the game, each variable is set to its max, in this case Mana has a different value than hunger & health
        Mana.Initialize(maxMana, maxMana);
        Health.Initialize(maxHealth, maxHealth);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }
    

    public void FixedUpdate()
    {
        
        float speed = 4.0f;
        if (Hunger.MyCurrentValue <= 100 && Hunger.MyCurrentValue > 75) //Decrease movement speed proportionate to hunger level
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        }
        else if (Hunger.MyCurrentValue <= 75 && Hunger.MyCurrentValue > 50) 
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 3.0f, Input.GetAxis("Vertical") * 3.0f);
        }
        else if (Hunger.MyCurrentValue <= 50 && Hunger.MyCurrentValue > 25)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 2.0f, Input.GetAxis("Vertical") * 2.0f);
        }
        else
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 1.0f, Input.GetAxis("Vertical") * 1.0f); //Else when hunger is anything other than 25+ then set movement speed to 1
        }
        if (Health.MyCurrentValue == 0)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 0.0f, Input.GetAxis("Vertical") * 0.0f); //At the point when health reaches 0, character can no longer move in any direction, it falls over
        }

    }

    
    public void Update()
    {
        anim.SetFloat("xspeed", rb.velocity.x);
        anim.SetFloat("yspeed", rb.velocity.y);
        if (rb.velocity.magnitude < 0.01)
            anim.speed = 0.0f;
        else
            anim.speed = 1.0f;


        //Decrease hunger with time
        Hunger.MyCurrentValue -= hungerFallRate * Time.deltaTime;
        
        if (Hunger.MyCurrentValue == 0) //Once hunger is 0, begin removing health
        {
            Health.MyCurrentValue -= healthFallRate * Time.deltaTime;
        }
      



    }
    
}
