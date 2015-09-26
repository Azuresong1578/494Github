using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour {

    public static BattleMenu S;
    public int activeItem;
    public bool selected;
    public List<GameObject> menuItems;

    public bool inFight;
    // 0 = FIGHT
    // 1 = BAG
    // 2 = POKEMON
    // 3 = RUN

    void Awake()
    {
        S = this;
    }
	// Use this for initialization
	void Start () {
        print("BattleMenu Started!");
        bool first = true;
        activeItem = 0;
        selected = false;
        inFight = false;
        GameObject background = transform.Find("Background").gameObject;
        foreach (Transform child in background.transform)
        {
            menuItems.Add(child.gameObject);
        }
 //       menuItems = menuItems.OrderByDescending(m => m.transform.position.y).ToList();

        foreach (GameObject go in menuItems)
        {
            Text itemText = go.GetComponent<Text>();
            if (first) itemText.color = Color.red;
            first = false;
            print(itemText);
        }


         gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            print(menuItems[activeItem].GetComponent<Text>().text + " selected!");
            selected = true;
            switch (activeItem) 
            {
                case 0:
                    SkillMenu.S.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                    inFight = true;
                    break;
                case 2:
                    gameObject.SetActive(false);
                    BattleDialog.S.gameObject.SetActive(false);
                    Application.LoadLevelAdditive("_Scene_1");
                    Menu_pokemon.S.enterFrom = 1;
                    break;
                case 3:
                    BattleDecider.S.state = BattleDecider.BattleState.Out;
                    BattleDecider.S.outMethod = BattleDecider.OutMethod.Runaway;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (activeItem == 2 || activeItem == 3)
            {
                menuItems[activeItem--].GetComponent<Text>().color = Color.black;
                menuItems[--activeItem].GetComponent<Text>().color = Color.red;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (activeItem == 0 || activeItem == 1)
            {
                menuItems[activeItem++].GetComponent<Text>().color = Color.black;
                menuItems[++activeItem].GetComponent<Text>().color = Color.red;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (activeItem == 1 || activeItem == 3)
            {
                menuItems[activeItem].GetComponent<Text>().color = Color.black;
                menuItems[--activeItem].GetComponent<Text>().color = Color.red;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (activeItem == 0 || activeItem == 2)
            {
                menuItems[activeItem].GetComponent<Text>().color = Color.black;
                menuItems[++activeItem].GetComponent<Text>().color = Color.red;
            }
        }
	}
}
