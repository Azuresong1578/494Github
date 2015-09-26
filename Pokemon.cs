using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Move
    {
    public enum PokeType
    {
        NORMAL,
        FLYING,
        POISON,
        BUG,
        FIRE,
        GRASS,
        WATER
    }
        public string moveName;
        public int moveType; 
        public PokeType type;
        public int power;
        public int accuracy;
        public int currentPP;
        public int maxPP;

        public Move(string _moveName, int _atktype, PokeType _type, int _power, int _accuracy, int _cpp, int _mpp)
        {
            moveName = _moveName;
            moveType = _atktype;
            type = _type;
            power = _power;
            accuracy = _accuracy;
            currentPP = _cpp;
            maxPP = _mpp;
        }
    }

public class Pokemon : MonoBehaviour {
    

    public string pokeName = "Pokemon";
    public int level = 5;
    public int maxHealth = 100;
    public int currentHealth;
    public int dph = 10;
    public int armor = 1;
    public int speed = 1;
    public int exp = 0;
    public int expOffered = 50;
    public bool displayFront = true;

    public string moveName1;
    public string moveName2;
    public string moveName3;
    public string moveName4;

    public Move[] moves = new Move[4];

    void Awake()
    {
        currentHealth = maxHealth;
    }

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        moves[0] = new Move(moveName1, 0, Move.PokeType.NORMAL, 50, 100, 35, 35);
        moves[1] = new Move(moveName2, 1, Move.PokeType.WATER, 6, 100, 30, 30);

	}
	
	// Update is called once per frame

    public Move useMove(int moveNum)
    {
        string message = this.pokeName + " used " + this.moves[moveNum].moveName;
        BattleDialog.S.ShowMessage(message);
        print(message);
        return moves[moveNum];
    }

    public void takeMove(Pokemon p, Move m)
    {
        if (m.moveType == 0)
        {
            currentHealth -= ((2 * p.level + 10) / 250) * (p.dph / armor) * m.power + 2;
        }
        if (m.moveType == 1)
        {
            armor -= m.power;
            if (armor < 1) armor = 1;
        }
    }

    public void heal()
    {
        currentHealth = maxHealth;
    }

    public bool ppEmpty(int moveNum)
    {
        return moves[moveNum].currentPP == 0;
    }
}
