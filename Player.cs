using UnityEngine;
using System.Collections;

public enum Direction {
    down, left, up, right
}

public class Player : MonoBehaviour {

	public static Player S;

    public float moveSpeed;
    public int tileSize;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    public SpriteRenderer sprend;

    public bool ___________;

    public RaycastHit hitInfo;
    public RaycastHit ledgehitInfo;

    public bool moving = false;
    public Vector3 targetPos;
    public Direction direction;
    public Vector3 moveVec;
    public bool onGrass = false;
    

    public Pokemon[] pokemonInBall = new Pokemon[6];
    public int numOfPokemonInBall;
    public int numOfPokemonFainted;
    public int currentPokemon;

	void Awake(){
		S = this;
	}

    void Start() {
        sprend = gameObject.GetComponent<SpriteRenderer>();
        targetPos = transform.position;
    }

    new public Rigidbody rigidbody {
        get { return gameObject.GetComponent<Rigidbody>(); }
    }

    public Vector3 pos {
        get { return transform.position; }
        set { transform.position = value; }
    }

	void FixedUpdate(){
        string playerCurPosStr = "0" + (int)targetPos.x + "x" + "0" + (int)targetPos.y;

        if (!moving && !Main.S.inDialog && !Main.S.paused)
        {
            if (Input.GetKeyDown(KeyCode.Z)) {
                CheckForAction();
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveVec = Vector3.right;
                direction = Direction.right;
                sprend.sprite = rightSprite;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveVec = Vector3.left;
                direction = Direction.left;
                sprend.sprite = leftSprite;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                moveVec = Vector3.up;
                direction = Direction.up;
                sprend.sprite = upSprite;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveVec = Vector3.down;
                direction = Direction.down;
                sprend.sprite = downSprite;
                moving = true;
            }
            else
            {
                moveVec = Vector3.zero;
                moving = false;
            }

            if (Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] { "Immovable", "NPC", "WaterTile" })))
            {
                moveVec = Vector3.zero;
                moving = false;
            }

            if (ShowMapOnCamera.S.mapAnchor.FindChild(playerCurPosStr).gameObject.layer == 9)
            {
                onGrass = true;
            }
            else onGrass = false;

           /* if (Physics.Raycast(GetRay(), out hitInfo, 0.5f, GetLayerMask(new string[] { "Grass" })))
            {
                onGrass = true;
            }
            else onGrass = false; */
            

            if (Physics.Raycast(LedgeGetRay(), out ledgehitInfo, 1f, GetLayerMask(new string[] { "Ledge" })))
            {
                moveVec = Vector3.zero;
                moving = false;
            }
            else if (Physics.Raycast(DownLedgeGetRay(), out ledgehitInfo, 1f, GetLayerMask(new string[] { "Ledge" })))
            {
                targetPos = pos + moveVec;
                targetPos.y -= 1;
                moving = true;

            }
            else
            {
                targetPos = pos + moveVec;
            }
        }
        else {
            if ( Physics.Raycast(GetRay(), out hitInfo, 10f, GetLayerMask(new string[] { "Trainer" })))
            {
                if (moving)
                    CheckForAction();
                moveVec = Vector3.zero;
                moving = false;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    moveSpeed = 10;
                else
                    moveSpeed = 4;
                if ((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime)
                {
                    pos = targetPos;
                    moving = false;
                }
                else
                {
                    pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
                }
                if (onGrass && Random.value < 0.01 && !Main.S.paused)
                {
                    Main.S.paused = true;
                    onGrass = false;
                    moving = false;
                    Main.S.inBattle = true;
                    BattleDecider.S.gameObject.SetActive(true);
                    Application.LoadLevelAdditive("_Scene_Battle");
                }
            }
        }
	}

    public void CheckForAction() {
        if (Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] { "Immovable", "NPC" })))
        {
            NPC npc = hitInfo.collider.gameObject.GetComponent<NPC>();
            npc.FacePlayer(direction);
            npc.PlayDialog();
        }
        if (Physics.Raycast(GetRay(), out hitInfo, 10f, GetLayerMask(new string[] { "Trainer" })))
        {
            Trainer trainer = hitInfo.collider.gameObject.GetComponent<Trainer>();
            if (trainer.fighttime)
            {
                trainer.FacePlayer(direction);
                trainer.PlayDialog();
                trainer.Moving();
                Application.LoadLevelAdditive("_Scene_Battle");
            }
        }
    }

    public Ray GetRay() {
        switch (direction) { 
            case Direction.down:
                return new Ray(pos, Vector3.down);
            case Direction.left:
                return new Ray(pos, Vector3.left);
            case Direction.right:
                return new Ray(pos, Vector3.right);
            case Direction.up:
                return new Ray(pos, Vector3.up);
            default:
                return new Ray();
        }
    }

    Ray LedgeGetRay()
    {
        switch (direction)
        {
            case Direction.left:
                return new Ray(pos, Vector3.left);
            case Direction.right:
                return new Ray(pos, Vector3.right);
            case Direction.up:
                return new Ray(pos, Vector3.up);
            default:
                return new Ray();
        }
    }

    Ray DownLedgeGetRay() {
        if (direction == Direction.down)
            return new Ray(pos, Vector3.down);
        return new Ray();
    }

    public int GetLayerMask(string[] layerNames) {
        int layerMask = 0;

        foreach(string layer in layerNames) {
            layerMask = layerMask | (1 << LayerMask.NameToLayer(layer));
        }

        return layerMask;
    }

    public void MoveThroughDoor(Vector3 doorLoc) {
        if (doorLoc.z <= 0)
            doorLoc.z = transform.position.z;
        moving = false;
        moveVec = Vector3.zero;
        transform.position = doorLoc;
    }

    public bool allFainted()
    {
        return numOfPokemonInBall == numOfPokemonFainted;
    }

    public void swapPokemon()
    {

    }
}
