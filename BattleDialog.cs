using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleDialog : MonoBehaviour {

    public static BattleDialog S;
    public Slider playerSlider;
    public Slider enemySlider;

    string[] battleDialogs = {
                                 "What should your Pokemon do?",
                                 "Go, my Pokemon!", 
                                 "Pokemon1 used XX", 
                                 "Great effect!",
                                 "Successfully runaway!"
                             };
    int dialogNum = 0;
    bool firstTime;

    void Awake()
    {
        S = this;
    }
	// Use this for initialization
	void OnEnable () {
        string currentMessage;
        Text playerPokeName = transform.Find("PlayerPokemonName").GetComponent<Text>();
        playerPokeName.text = Player.S.pokemonInBall[0].pokeName + "\n" + ":L" + Player.S.pokemonInBall[0].level;
        Text enemyPokeName = transform.Find("EnemyPokemonName").GetComponent<Text>();
        enemyPokeName.text = BattleDecider.S.enemyPokemons[BattleDecider.S.currentPokemon].pokeName + "\n" + ":L" + BattleDecider.S.enemyPokemons[BattleDecider.S.currentPokemon].level;
        currentMessage = "Wild " + BattleDecider.S.enemyPokemons[BattleDecider.S.currentPokemon].pokeName + " appeared!";
        ShowMessage(currentMessage);
        firstTime = true;
        playerSlider = transform.Find("PlayerSlider").GetComponent<Slider>();
        enemySlider = transform.Find("EnemySlider").GetComponent<Slider>();
        playerSlider.maxValue = Player.S.pokemonInBall[0].maxHealth;
        enemySlider.maxValue = BattleDecider.S.enemyPokemons[BattleDecider.S.currentPokemon].maxHealth;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Text playerHP = transform.Find("PlayerPokemonHP").GetComponent<Text>();
        playerHP.text = Player.S.pokemonInBall[0].currentHealth + " / " + Player.S.pokemonInBall[0].maxHealth;
        playerSlider.value = Player.S.pokemonInBall[0].currentHealth;
        enemySlider.value = BattleDecider.S.enemyPokemons[BattleDecider.S.currentPokemon].currentHealth;

        /* if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowMessage(battleDialogs[dialogNum++]);
            if (firstTime)
            {
                BattleMenu.S.gameObject.SetActive(true);
            }
        } */

	}

    public void ShowMessage(string message)
    {
        GameObject dialogBox = transform.Find("Background").gameObject;
        Text goText = dialogBox.transform.Find("Text").GetComponent<Text>();
        goText.text = message;
    }
}
