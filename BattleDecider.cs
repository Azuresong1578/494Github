using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleDecider : MonoBehaviour {
    public enum BattleState
    {
        Start,
        Restart,
        TrainerStart,
        PlayerChoice,
        usingMove1,
        usingMove2,
        WinOne,
        WinAll,
        LoseOne,
        LoseAll,
        Out,
        Inactive
    }

    public enum OutMethod
    {
        Winout,
        Loseout,
        Runaway
    }

    public static BattleDecider S;
    public Pokemon[] enemyPokemons = new Pokemon[6];
    public int currentPokemon;
    public bool trainerStart;
    public BattleState state;
    public Move playerNextMove;
    public OutMethod outMethod;
    public int trainerPokemonCount;
    bool firstHand;
    float timer = 3f;
    float rng;
    string message;
    Trainer trainer;
    int escapeTries;

    void Awake()
    {
        S = this;
    }

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
    void OnEnable()
    {
        state = BattleState.Start;
        rng = Random.value;
        if (rng < 0.33)
        {
            currentPokemon = 0;
        }
        else if (rng < 0.66)
        {
            currentPokemon = 1;
        }
        else
        {
            currentPokemon = 1; // need to change
        }
        if (Random.value < 0.5) currentPokemon = 0;
        else currentPokemon = 1;
        enemyPokemons[currentPokemon].currentHealth = enemyPokemons[currentPokemon].maxHealth;
        escapeTries = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        print(state.ToString());
        firstHand = Player.S.pokemonInBall[Player.S.currentPokemon].speed >= enemyPokemons[currentPokemon].speed;

        if (state == BattleState.Start)
        {
            if (trainerStart == true)
            {
                state = BattleState.TrainerStart;
            }
            else
            {
                message = "Wild " + enemyPokemons[currentPokemon].pokeName + " appeared!";
                BattleDialog.S.ShowMessage(message);
                if (timer > 0) timer -= Time.fixedDeltaTime;
                else
                {
                    message = "GO! " + Player.S.pokemonInBall[Player.S.currentPokemon].pokeName + "!";
                    BattleDialog.S.ShowMessage(message);
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        state = BattleState.PlayerChoice;
                        timer = 1f;
                    }
                }
            }
        }
        else if (state == BattleState.Restart) 
        {
            message = "Get'em! " + Player.S.pokemonInBall[Player.S.currentPokemon].pokeName + "!";
            BattleDialog.S.ShowMessage(message);
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else 
            {
                state = BattleState.PlayerChoice;
                timer = 3f;
            }
        }
        else if (state == BattleState.TrainerStart)
        {
            message = trainer.trainerName + " wants to fight!";
            BattleDialog.S.ShowMessage(message);
            trainerStart = false;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                message = trainer.trainerName + " sent out " + trainer.pokemonInBall[trainer.currentPokemon];
                BattleDialog.S.ShowMessage(message);
                state = BattleState.PlayerChoice;
            }
        }
        else if (state == BattleState.PlayerChoice)
        {
            timer = 3f;
            if (!BattleMenu.S.gameObject.activeSelf && !BattleMenu.S.inFight)
            {
                BattleMenu.S.gameObject.SetActive(true);
            }
        }
        else if (state == BattleState.usingMove1)
        {
                Decide(ref Player.S.pokemonInBall[Player.S.currentPokemon], ref enemyPokemons[currentPokemon], firstHand);
                state = BattleState.usingMove2;
                timer = 1f;
        }
        else if (state == BattleState.usingMove2)
        {
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else
            {
                if (firstHand == true)
                {
                    if (enemyPokemons[currentPokemon].currentHealth <= 0)
                    {
                        message = enemyPokemons[currentPokemon].pokeName + " fainted!";
                        BattleDialog.S.ShowMessage(message);
                        state = BattleState.WinOne;
                        timer = 3f;
                    }
                    else
                    {
                        Decide(ref Player.S.pokemonInBall[Player.S.currentPokemon], ref enemyPokemons[currentPokemon], !firstHand);
                        state = BattleState.PlayerChoice;
                        timer = 3f;
                    }
                }
                else
                {
                    if (Player.S.pokemonInBall[Player.S.currentPokemon].currentHealth <= 0)
                    {
                        if (Player.S.allFainted())
                        {
                            state = BattleState.LoseAll;
                            timer = 3f;
                        }
                        else state = BattleState.LoseOne;
                        timer = 3f;
                    }
                    else
                    {
                        Decide(ref Player.S.pokemonInBall[Player.S.currentPokemon], ref enemyPokemons[currentPokemon], !firstHand);
                        state = BattleState.PlayerChoice;
                        timer = 3f;
                    }
                }
            }
        }
        else if (state == BattleState.WinOne)
        {
            if (!trainerStart)
            {
                state = BattleState.WinAll;
            }
        }
        else if (state == BattleState.WinAll)
        {
            BattleDialog.S.ShowMessage("You win! Gain 150 EXP, 100 Gold, 1 x Health Potion!");
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else
            {
                if (trainerStart)
                {
                    trainerStart = false;
                    message = trainer.trainerName + " says: OK you won. But, do you have a girlfriend?";
                    BattleDialog.S.ShowMessage(message);
                }
                outMethod = OutMethod.Winout;
                state = BattleState.Out;
            }
        }

        else if (state == BattleState.LoseOne)
        {
            message = Player.S.pokemonInBall[currentPokemon].pokeName + " fainted!";
            BattleDialog.S.ShowMessage(message);
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else
            {
                Player.S.swapPokemon();
                state = BattleState.Restart;
                timer = 3f;
            }
        }
        else if (state == BattleState.LoseAll)
        {
            message = "You are not prepared! --  Illidan Stormrage";
            BattleDialog.S.ShowMessage(message);
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else
            {
                outMethod = OutMethod.Loseout;
                state = BattleState.Out;
                timer = 3f;
            }
        }
        else if (state == BattleState.Out)
        {
            if (outMethod != OutMethod.Runaway)
            {
                Application.UnloadLevel("_Scene_Battle");
                gameObject.SetActive(false);
            }
            else
            {
                if (trainerStart)
                {
                    message = "Can't run away from trainer!";
                    BattleDialog.S.ShowMessage(message);
                    state = BattleState.PlayerChoice;
                }
                else
                {
                    int escapeChance = Player.S.pokemonInBall[Player.S.currentPokemon].speed * 32 / ((enemyPokemons[currentPokemon].speed / 4) % 256) + 30 * escapeTries;
                    print(escapeChance);
                    if (escapeChance > 70)
                    {
                        message = "Got away safely! Coward level +1!";
                        BattleDialog.S.ShowMessage(message);
                        if (timer > 0) timer -= Time.fixedDeltaTime;
                        else
                        {
                            timer = 3f;
                            state = BattleState.Inactive;
                            Main.S.inBattle = false;
                            Main.S.paused = false;
                            BattleMenu.S.gameObject.SetActive(false);
                            gameObject.SetActive(false);
                            Application.UnloadLevel("_Scene_Battle");
                        }
                    }
                    else
                    {
                        BattleDialog.S.ShowMessage("Run away unsuccessful. LOL, sonuvabitch.");
                        state = BattleState.PlayerChoice;
                    }
                }
            }
        }
        
	}

    public void Decide(ref Pokemon player, ref Pokemon enemy, bool whosTurn)
    {
        if (whosTurn == true)
        {
            // player's turn
            enemy.takeMove(player, playerNextMove);
        }
        else
        {
            // enemy's turn
            int randomMove = 0;
            if (Random.value <= 0.5) randomMove = 1;
            player.takeMove(enemy, enemy.moves[randomMove]);
        }
    }

    public void Decide1(Pokemon player, Pokemon enemy)
    {
        
        if (player.currentHealth <= 0) 
        {
            Player.S.swapPokemon();
            return;
        }
        if (enemy.currentHealth <= 0) 
        {
            BattleDialog.S.ShowMessage("You win! Gain 10 XP. Press Enter to exit.");
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Application.UnloadLevel("_Scene_Battle");
                BattleMenu.S.inFight = false;
                SkillMenu.S.gameObject.SetActive(false);
                Main.S.inBattle = false;
                Main.S.paused = false;
                return;
            }
        }
        if (player.speed > enemy.speed) 
        {
            enemy.takeMove(player, player.useMove(SkillMenu.S.activeItem));
            if (enemy.currentHealth > 0) 
            {
                while (timer > 0) timer -= Time.fixedDeltaTime;
                player.takeMove(enemy, enemy.useMove(0));
                timer = 3f;
            }
            else 
            {
                BattleDialog.S.ShowMessage("You win! Gain 10 XP. Press Enter to exit.");
                return;
            }
        }
        else
        {
            player.takeMove(enemy, enemy.useMove(0));
            while (timer > 0) 
            {
                timer -= Time.deltaTime;
                print(timer);
            }
            timer = 5f;
            if (player.currentHealth > 0)
            {
                enemy.takeMove(player, player.useMove(SkillMenu.S.activeItem));
            }
            else
            {
                BattleDialog.S.ShowMessage("Your Pokemon fainted");
                Player.S.swapPokemon();
            }
        }
    }

    public void StartedByTrainer(Trainer trainerIn)
    {
        trainerStart = true;
        trainer = trainerIn;
    }
}

