using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkillMenu : MonoBehaviour {
    public static SkillMenu S;
    public int activeItem;
    public List<GameObject> menuItems;
    Text typeText;
    Text ppText;
    // 0 = FIGHT
    // 1 = BAG
    // 2 = POKEMON
    // 3 = RUN


    void Awake()
    {
        S = this;
    }
    // Use this for initialization
    void Start()
    {
        bool first = true;
        activeItem = 0;
        GameObject background = transform.Find("Background").gameObject;
        Transform typePP = transform.Find("TypePP");
        foreach (Transform child in background.transform)
        {
            menuItems.Add(child.gameObject);
        }

        foreach (GameObject go in menuItems)
        {
            Text itemText = go.GetComponent<Text>();
            if (first) itemText.color = Color.red;
            first = false;
            print(itemText);
        }
        Text s1 = background.transform.Find("Skill1").GetComponent<Text>();
        Text s2 = background.transform.Find("Skill2").GetComponent<Text>();
        Text s3 = background.transform.Find("Skill3").GetComponent<Text>();
        Text s4 = background.transform.Find("Skill4").GetComponent<Text>();
        s1.text = Player.S.pokemonInBall[0].moveName1;
        s2.text = Player.S.pokemonInBall[0].moveName2;
        s3.text = Player.S.pokemonInBall[0].moveName3;
        s4.text = Player.S.pokemonInBall[0].moveName4;
        typeText = typePP.Find("Type").gameObject.GetComponent<Text>();
        ppText = typePP.Find("PP").gameObject.GetComponent<Text>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        typeText.text = Player.S.pokemonInBall[Player.S.currentPokemon].moves[activeItem].type.ToString();
        ppText.text = Player.S.pokemonInBall[Player.S.currentPokemon].moves[activeItem].currentPP
            + " / " + Player.S.pokemonInBall[Player.S.currentPokemon].moves[activeItem].maxPP;
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            print(menuItems[activeItem].GetComponent<Text>().text + " selected!");
            BattleDecider.S.playerNextMove = Player.S.pokemonInBall[Player.S.currentPokemon].moves[activeItem];
            BattleDecider.S.state = BattleDecider.BattleState.usingMove1;
            BattleMenu.S.inFight = false;
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            gameObject.SetActive(false);
            BattleMenu.S.gameObject.SetActive(true);
            BattleMenu.S.inFight = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUpMenu();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDownMenu();
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {

        }
    }

    public void MoveDownMenu()
    {
        menuItems[activeItem].GetComponent<Text>().color = Color.black;
        activeItem = activeItem == menuItems.Count - 1 ? menuItems.Count - 1 : ++activeItem;
        menuItems[activeItem].GetComponent<Text>().color = Color.red;
    }

    public void MoveUpMenu()
    {
        menuItems[activeItem].GetComponent<Text>().color = Color.black;
        activeItem = activeItem == 0 ? 0 : --activeItem;
        menuItems[activeItem].GetComponent<Text>().color = Color.red;
    }
}
