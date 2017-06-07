#pragma warning disable 0067, 0649, 0169, 0414

// I have no idea what the deal with this is.
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TileSlider : MonoBehaviour {
    enum Difficulty { CASUAL, CHALLENGE };
    private Difficulty mode;

    private Transform keys;
    private GameObject currentSelection,
                       preview;

    private Ray ray;
    private RaycastHit hit;

    private Vector3 init_preview_pos;
    private Vector3 init_preview_local_scale;


    private List<Vector3> positions;
    private List<Bounds> bounds;
    private List<int> scrambled_slots;
    private List<Tile> tiles;

    private float selectionZpos,
                  timer;

    private bool playing = false,
                 gameWon = false,
                 scrambled = false,
                 resetting = false;

    private int currentID,
                score = 0,
                empty_slot,
                screenWidth,
                screenHeight;


    public Texture2D[] textures,
                       previews;

    public int numTiles;

    // Use this for initialization
    void Start() {
        positions = new List<Vector3>();
        bounds = new List<Bounds>();
        tiles = new List<Tile>();
        empty_slot = numTiles;
        preview = GameObject.Find("Preview");

        init();
        new_game();
        //solve();
    }

    // Update is called once per frame
    void Update() {
        if (playing) {
            check_input();
        } else {
            tiles.ForEach(delegate (Tile cur_tile) {
                cur_tile.hide();
            });

            preview.transform.position = init_preview_pos;
            preview.transform.localScale = init_preview_local_scale;

            if (gameWon) {
                score += 5;
                GameContext.Instance.Player.Points += score; // Add points to main Game Play
            }
        }
        GameObject pointsObj = GameObject.Find("Points");
        Text pointsText = pointsObj.GetComponentInChildren<Text>();
        pointsText.text = score.ToString();
    }

    void init() {
        GameObject currentTile;

        init_preview_pos = preview.transform.position;
        init_preview_local_scale = preview.transform.localScale;

        for (int i = 1; i <= numTiles; i++) {
            //if index is at empty slot, find gameObject called "Blank"
            if (i == empty_slot) {
                currentTile = GameObject.Find("PuzzlePiece9");
            } else {
                //set the position to the world position of the corresponding tile
                currentTile = GameObject.Find("PuzzlePiece" + i);

                tiles.Add(new Tile(currentTile, i));
            }

            positions.Add(currentTile.transform.position);
            bounds.Add(currentTile.GetComponent<MeshFilter>().mesh.bounds);

        }
    }

    public void start_game() {
        playing = true;
        StartCoroutine(showTiles());
    }

    public void gamePlay() // Used in START Button in game.
    {
        GameObject NewGameObj = GameObject.Find("NewGameButton");
        Text newgameButtonText = NewGameObj.GetComponentInChildren<Text>();
        if (playing) {
            newgameButtonText.text = "Start";
            new_game();
        } else {
            newgameButtonText.text = "New Game";
            start_game();
        }
    }

    public void new_game() {
        playing = false;

        set_textures();

        do {
            scramble();
        }
        while (!solvable());

    }

    public int get_score() {
        return score;
    }

    public void reset() {
        tiles.ForEach(delegate (Tile cur_tile) {
            int slot = scrambled_slots[cur_tile.get_init_slot() - 1];

            cur_tile.set_cur_slot(slot);
            cur_tile.move(positions[slot - 1]);
        });
    }

    void set_textures() {
        int rand = UnityEngine.Random.Range(0, textures.Length);

        preview.GetComponent<Renderer>().material.SetTexture("_MainTex", previews[rand]);

        tiles.ForEach(delegate (Tile cur_tile) {
            cur_tile.set_texture(textures[rand]);
        });
    }

    IEnumerator showTiles() {
        preview.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(preview.GetComponent<Animation>().clip.length);
        tiles.ForEach(delegate (Tile cur_tile) {
            cur_tile.show();
        });
    }

    void scramble() {
        int rand;
        scrambled_slots = new List<int>();

        tiles.ForEach(delegate (Tile cur_tile) {
            //generate a random number as long as the list of used_positions contains the number
            do {
                rand = UnityEngine.Random.Range(1, numTiles + 1);
            }
            while (scrambled_slots.Contains(rand));

            scrambled_slots.Add(rand);

            //Assign new position's slot number to tile
            cur_tile.set_cur_slot(rand);
            //Move tile to new position
            cur_tile.move(positions[rand - 1]);
        });

        //Add a position for the empty slot tile
        for (int i = 1; i <= numTiles; i++) {
            if (scrambled_slots.Contains(i))
                continue;

            scrambled_slots.Add(i);
        }
    }

    bool solvable() {
        int inversions = count_inversions();  //An inversion is a pair of tiles (a,b) such that a appears before b, but a>b;
        int dimension = (int)Math.Sqrt(numTiles);
        int empty_slot_row_from_bottom = dimension - (int)(scrambled_slots.FindIndex(s => s == empty_slot) / dimension);

        //String temp = "Row from bottom: " + empty_slot_row_from_bottom.ToString() + "; Inversions: " + inversions + "; ";
        //scrambled_slots.ForEach(delegate(int slot) { temp += slot.ToString() + ", "; });
        //Debug.Log(temp);

        /**(1)If grid width is odd, game is solvable if inversions are even number.
         * If grid width is even, game is solvable if
         * (2)empty slot is on an even row from bottom  and inversions are odd number, or
         * (3) empty slot is on an odd row from bottom  and inversions are even number.*/

        if (dimension % 2 != 0) {
            if (inversions % 2 == 0) {
                return true;
            }
        } else {
            if (inversions % 2 == 0 && empty_slot_row_from_bottom % 2 != 0)
                return true;

            if (inversions % 2 != 0 && empty_slot_row_from_bottom % 2 == 0)
                return true;
        }

        return false;
    }

    int count_inversions() {
        int inversions = 0;

        for (int i = 0; i < numTiles - 1; i++) {
            int cur_slot = scrambled_slots[i];

            if (cur_slot != empty_slot) {
                for (int j = i + 1; j < numTiles; j++) {
                    int slot = scrambled_slots[j];

                    if (cur_slot > slot && slot != empty_slot)
                        inversions++;
                }
            }
        }

        return inversions;
    }

    void check_input() {
        if (Input.GetButtonDown("Primary")) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                foreach (Tile tile in tiles) {
                    if (hit.collider.transform.name == tile.get_name()) {
                        //Get all the slots that surround the selected tile.
                        List<int> neighbors = tile.get_neighbors(numTiles);

                        foreach (int neighbor in neighbors) {
                            //If this slot isn't occupied, move the selected tile to it.
                            if (!tiles.Exists(t => t.slot_matches(neighbor))) {
                                tile.move(positions[neighbor - 1]);
                                tile.set_cur_slot(neighbor);

                                //Check if all tiles are at correct position
                                check_tiles();
                                /*if (correct_tiles() == tiles.Count)
                                {
                                    gameWon = true;
                                    playing = false;
                                }*/

                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    int check_tiles()//correct_tiles()//
    {
        int correct_tiles = 0;

        tiles.ForEach(delegate (Tile cur_tile) {
            if (cur_tile.get_init_slot() == cur_tile.get_cur_slot())
                correct_tiles++;
        });

        if (correct_tiles == tiles.Count) {
            gameWon = true;
            playing = false;
        }
        return correct_tiles;
    }

    void solve() {
        /*AStar solution = new AStar();

        List<OTNode> start_neighbors;
        List<OTNode> goal_neighbors;
        List<OTNode> start_nodes = new List<OTNode>();
        List<OTNode> goal_nodes = new List<OTNode>();
                
        scrambled_slots.ForEach(delegate(int s)
        {
            start_nodes.Add(new OTNode(tiles.Find(t => t.get_cur_slot() == s).get_bounds()));
        });

        bounds.ForEach(delegate(Bounds b)
        {
            goal_nodes.Add(b);
        });

        for (int i = 0; i < numTiles; i++)
        {
            start_neighbors = new List<OTNode>();
            goal_neighbors = new List<OTNode>();

            //start
            foreach (int slot in tiles[i].get_neighbors(numTiles))
            {
                OTNode node = start_nodes;
                start_neighbors.Add(node);
            }

            //goal
            foreach (int slot in tiles[i].get_neighbors(numTiles, true))
            {
                goal_neighbors.Add(goal_nodes[slot - 1]);
            }

            start_nodes[i].otneighbors = start_neighbors;
            goal_nodes[i].otneighbors = goal_neighbors;
        }
       
        solution.SetStart(start_nodes[0]);
        solution.SetGoal(goal_nodes[0]);

        print(solution.AStarSearch());*/


        // A* Algorithm
        // ---------------------------------------------------------------------------
        // -initialize the open list
        //List<int> open = new List<int>;

        // -initialize the closed list
        //List<int> closed = new List<int> {};

        // put the starting node on the open list (you can leave its f at zero)
        //
        // while the open list is not empty
        //    find the node with the least f on the open list, call it "q"
        //    pop q off the open list
        //    generate q's 8 successors and set their parents to q
        //    for each successor
        //        if successor is the goal, stop the search
        //        successor.g = q.g + distance between successor and q
        //        successor.h = distance from goal to successor
        //        successor.f = successor.g + successor.h
        //
        //        if a node with the same position as successor is in the OPEN list
        //            which has a lower f than successor, skip this successor
        //        if a node with the same position as successor is in the CLOSED list 
        //            which has a lower f than successor, skip this successor
        //        otherwise, add the node to the open list
        //    end
        //    push q on the closed list
        // end
        // 
    }


    /// <summary>
    /// Tile object
    /// </summary>
    class Tile {
        private GameObject tile;
        private int init_slot,
                    cur_slot;

        public Tile(GameObject t, int i_slot) {
            tile = t;
            init_slot = i_slot;
            cur_slot = i_slot;
        }

        public String get_name() {
            return tile.name;
        }

        public Bounds get_bounds() {
            return tile.GetComponent<MeshFilter>().mesh.bounds;
        }

        public int get_init_slot() {
            return init_slot;
        }

        public int get_cur_slot() {
            return cur_slot;
        }

        public void set_cur_slot(int slot) {
            cur_slot = slot;
        }

        public bool slot_matches(int neighbor) {
            if (cur_slot == neighbor)
                return true;

            return false;
        }

        public void show() {
            tile.GetComponent<MeshRenderer>().enabled = true;
        }

        public void hide() {
            tile.GetComponent<Renderer>().enabled = false;
        }

        public void set_texture(Texture2D texture) {
            tile.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
        }

        public void move(Vector3 target) {
            Vector3 translation = target - tile.transform.position;

            tile.transform.Translate(translation, Space.World);
        }

        public List<int> get_neighbors(int numTiles) {
            return get_neighbors(numTiles, false);
        }

        public List<int> get_neighbors(int numTiles, bool init_state) {
            List<int> neighbors = new List<int>();
            int dimension = (int)Math.Sqrt(numTiles);
            int slot = cur_slot;

            if (init_state)
                slot = init_slot;

            //Loop through columns (col 1:{1,4,7}, col 2:{2,5,8}, col 3{3,6,9});
            for (int i = 1; i <= dimension; i++) {
                //Loop through rows (row 0:{1,2,3}, row 1:{4,5,6}, row 2{7,8,9})
                for (int j = 0; j <= dimension - 1; j++) {
                    if (slot == i + (dimension * j)) {
                        //Each tile has a max of 4 neighbors                        
                        if (i == 1) {
                            neighbors.Add((i + 1) + (dimension * j));
                        } else if (i == dimension) {
                            neighbors.Add((i - 1) + (dimension * j));
                        } else {
                            neighbors.Add((i - 1) + (dimension * j));
                            neighbors.Add((i + 1) + (dimension * j));
                        }

                        if (j == 0) {
                            neighbors.Add(i + (dimension * (j + 1)));
                        } else if (j == dimension - 1) {
                            neighbors.Add(i + (dimension * (j - 1)));
                        } else {
                            neighbors.Add(i + (dimension * (j - 1)));
                            neighbors.Add(i + (dimension * (j + 1)));
                        }

                        goto done;
                    }
                }
            }

        done: return neighbors;
        }
    }
}
#pragma warning restore 0067, 0649, 0414