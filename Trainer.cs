using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Trainer : MonoBehaviour
{

    public Trainer S;

    public string trainerName;
    public string speech;
    public Pokemon[] pokemonInBall = new Pokemon[6];

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public bool fighttime = true;

    public int currentPokemon;

 //   public bool moving;

    public SpriteRenderer sprend;

    void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start()
    {
        sprend = gameObject.GetComponent<SpriteRenderer>();
    }

    new public Rigidbody rigidbody
    {
        get { return gameObject.GetComponent<Rigidbody>(); }
    }

    public bool Fighttime
    {
        get { return fighttime; }
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public void PlayDialog()
    {
        print(speech);
        Dialog.S.gameObject.SetActive(true);
        Color noAlpha = GameObject.Find("DialogBackground").GetComponent<Image>().color;
        noAlpha.a = 255;
        GameObject.Find("DialogBackground").GetComponent<Image>().color = noAlpha;
        Dialog.S.ShowMessage(speech);
    }

    public void FacePlayer(Direction playerDir)
    {
        switch (playerDir)
        {
            case Direction.down:
                sprend.sprite = upSprite;
                break;
            case Direction.right:
                sprend.sprite = leftSprite;
                break;
            case Direction.left:
                sprend.sprite = rightSprite;
                break;
            case Direction.up:
                sprend.sprite = downSprite;
                break;
        }
    }

    public void Moving() {
        Vector3 temp = Player.S.pos;
        if (Player.S.sprend.sprite == downSprite) {
            temp.y = temp.y - 1;
            pos = temp;
        }
        else if (Player.S.sprend.sprite == upSprite) {
            temp.y = temp.y + 1;
            pos = temp;
        }
        else if (Player.S.sprend.sprite == rightSprite)
        {
            temp.x = temp.x + 1;
            pos = temp;
        }
        else if (Player.S.sprend.sprite == leftSprite)
        {
            temp.x = temp.x - 1;
            pos = temp;
        }
        fighttime = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

