using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class HeroesManager : MonoBehaviour {

    public class Hero
    {
        public string name;
        public int damage;
        public int lvl;
        public int cost;
        public Hero(int damage, int lvl, string name, int cost)
        {
            this.damage = damage;
            this.lvl = lvl;
            this.name = name;
            this.cost = cost;
        }
        public Hero() { }
        public void Atack() { }
        public void Spells() { }
        public void LvlUp() { this.lvl++; }
    }
    [Header("Heroes Stats")]
    static public int GreenCost=60;
    static public int GreenDMG=5;
    static public int GreenLvl=0;
    static public int BlackCost=130;
    static public int BlackDMG=10;
    static public int BlackLvl=0;
    static public int AssasinCost=250;
    static public int AssasinDMG=15;
    static public int AssasinLvl=0;


    [Header("Texts")]
    public Text GreenHeroTxt;
    public Text BlackHeroTxt;
    public Text AssasinHeroTxt;
    public Text GreenDmg;
    public Text BlackDmg;
    public Text AssasinDmg;
    public Text GreenLVL;
    public Text BlackLVL;
    public Text AssasinLVL;

    [Header("Heroes")]
    public GameObject GreenHeroObject;
    public GameObject BlackHeroObject;
    public GameObject AssasinHeroObject;

    Color green = new Color(0, 0.6415094f, 0.2539307f);
    Color red = new Color(0.8301887f, 0.1755913f, 0.08223567f);

    Color col = new Color(205, 0, 0);
    float greenAtkValue, blackAtkValue, assasinsAtkValue;

    //Zmienne globalne.
    public static Hero GreenHero = new Hero(GreenDMG, GreenLvl, "GreenHero", GreenCost);
    public static Hero BlackHero = new Hero(BlackDMG, BlackLvl, "BlackHero", BlackCost);
    public static Hero AssasinHero = new Hero(AssasinDMG, AssasinLvl, "AssasinHero", AssasinCost);
    public static float AverageDMGperSecond = 0;

    public void GreenLvlUP()
    {
        if (GameManager.money >= GreenHero.cost)
        {
            GreenHero.LvlUp();
            GameManager.money -= GreenHero.cost;
        }
        if (GreenHero.lvl > 0)
            GreenHeroObject.SetActive(true);
    }
    public void BlackHeroLvlUP()
    {
        if (GameManager.money >= BlackHero.cost)
        {
            BlackHero.LvlUp();
            GameManager.money -= BlackHero.cost;
            if (BlackHero.lvl > 0)
                BlackHeroObject.SetActive(true);
        }
        
    }
    public void AssasinHeroLvlUP()
    {
        if (GameManager.money >= AssasinHero.cost)
        {
            AssasinHero.LvlUp();
            GameManager.money -= AssasinHero.cost;
        }
        if (AssasinHero.lvl > 0)
            AssasinHeroObject.SetActive(true);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.money >= GreenHero.cost)
        {
            GreenHeroTxt.color = green;
        }
        else
            GreenHeroTxt.color = red;

        if (GameManager.money >= BlackHero.cost)
        {
            BlackHeroTxt.color = green;
        }
        else
            BlackHeroTxt.color = red;

        if (GameManager.money >= AssasinHero.cost)
        {
            AssasinHeroTxt.color = green;
        }
        else
            AssasinHeroTxt.color = red;

        greenAtkValue = GreenHero.lvl * ((GreenHero.lvl * GreenHero.damage * 0.01f) + GreenHero.damage);
        blackAtkValue = BlackHero.lvl * ((BlackHero.lvl * BlackHero.damage * 0.01f) + BlackHero.damage);
        assasinsAtkValue = AssasinHero.lvl * ((AssasinHero.lvl * AssasinHero.damage * 0.01f) + AssasinHero.damage);
        AverageDMGperSecond = greenAtkValue + blackAtkValue + assasinsAtkValue; 
        GreenDmg.text = greenAtkValue.ToString();
        BlackDmg.text = blackAtkValue.ToString();
        AssasinDmg.text = assasinsAtkValue.ToString();
        GreenLVL.text = GreenHero.lvl.ToString();
        BlackLVL.text = BlackHero.lvl.ToString();
        AssasinLVL.text = AssasinHero.lvl.ToString();
    }
}
