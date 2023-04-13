using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed =0.1f;
    GameObject currentFloor;
    [SerializeField] int Hp;
    [SerializeField] GameObject HpBar;

    [SerializeField] Text scoreText;

    int score;
    float scoreTime;
    Animator anim;
    SpriteRenderer SpriteRenderer;
    AudioSource deathSound;

    [SerializeField] GameObject replayButton;
    // Start is called before the first frame update
    void Start()
    {
        Hp = 10;
        score =0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
            transform.Translate(moveSpeed*Time.deltaTime,0,0);
            SpriteRenderer.flipX = false;
            anim.SetBool("run",true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow)){
            transform.Translate(-moveSpeed*Time.deltaTime,0,0);
            SpriteRenderer.flipX = true;
            anim.SetBool("run",true);
        }
        else{
            anim.SetBool("run",false);
        }
        UpdataScore();
            
    }
    void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "Normal"){
            
            if (other.contacts[0].normal == new Vector2(0f,1f)){
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
                
            }
            
        }
        else if(other.gameObject.tag == "Naits"){
            
             if (other.contacts[0].normal == new Vector2(0f,1f)){
                currentFloor = other.gameObject;
                ModifyHp(-1);
                other.gameObject.GetComponent<AudioSource>().Play();
                anim.SetTrigger("hurt");
                
            }
        }
        else if(other.gameObject.tag == "Ceiling"){
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3);
            other.gameObject.GetComponent<AudioSource>().Play();
            anim.SetTrigger("hurt");
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
         if(other.gameObject.tag == "DeadLine"){
            Die();
            
        }
    }
    void ModifyHp(int num){
        Hp +=num;
        if(Hp >10){
            Hp = 10;
        }
        else if(Hp <=0){
            Hp = 0;
            Die();
        }
        UpdateHpBar();
    }

    void UpdateHpBar(){
        for(int i = 0;i<HpBar.transform.childCount;i++){
            if(Hp>i){
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else{
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    void UpdataScore(){
        scoreTime += Time.deltaTime;
        if(scoreTime >=2){
            score++;
            scoreTime = 0;
            scoreText.text = "地下" +  score.ToString() + "層";
        }
    }
    void Die(){
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }
    public void Replay(){
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}
