using UnityEngine;
using States;
using System.Collections.Generic;

namespace GameManager
{
    public class Map : MonoBehaviour
    {
        public static Map MyReference { get; private set; }

        public GameObject prefabCell;
        private List<GameObject> list;
        public Transform parent;
        int[,] map;
        public int gridSize = 10;
        int rows;
        int cols;
        [Range(0.0f, 1.0f)]
        public float resolution = 1;
        internal bool isSimulationRunning;

        private void Awake()
        {
            MyReference = this;
        }

        void Start()
        {
            isSimulationRunning = false;
            list = new List<GameObject>();
            Vector3 gridS = new Vector3(gridSize, gridSize, 0f);
            transform.localScale = gridS;
            rows = (int)(gridSize / resolution);
            cols = (int)(gridSize / resolution);
            map = new int[rows, cols];

            //initially every cell is dead
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    map[i, j] = (int)State.Dead;
                }
            }

            RenderMap();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSimulationRunning = !isSimulationRunning;
            }
            if (isSimulationRunning)
            {
                Debug.Log("simulation is running"); // just for debugging
                int[,] newGeneration = new int[rows, cols];

                for(int i = 0; i < rows; i++)
                {
                    for(int j = 0; j < cols; j++)
                    {
                        State state = (State)map[i, j];

                        ////edge calculation -- done in countLiveNeighbours -- considered imaginary neighbours for edge cells that are dead.
                        //if(i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
                        //{
                        //    newGeneration[i, j] = (int)state;
                        //    continue;
                        //}

                        //non-edge
                        int liveNeighbours = CountLiveNeighbours(i,j);

                        if(state == State.Dead && liveNeighbours == 3)
                        {
                            newGeneration[i, j] = (int)State.Alive;
                        }else if(state == State.Alive && (liveNeighbours<2 || liveNeighbours > 3))
                        {
                            newGeneration[i, j] = (int)State.Dead;
                        }
                        else
                        {
                            newGeneration[i, j] = (int)state;
                        }

                    }
                }

                map = newGeneration;
                UpdateMap();

            }
            else
            {
                Debug.Log("simulation is not running"); //just for debugging. otherwise the else block is not required.
            }
        }

        void UpdateMap()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    SetStateColor(list[cols * i + j], (State)map[i, j]);
                }
            }

        }

        void RenderMap()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    GameObject prefabInstance = Instantiate(prefabCell, new Vector3((resolution / 2) - (resolution * ((float)cols / 2 - j)), (resolution * ((float)rows / 2 - i)) - resolution / 2, 0f), Quaternion.identity, parent);
                    prefabInstance.transform.localScale = new Vector3(resolution, resolution, 0f);
                    list.Add(prefabInstance);
                    if (map[i, j] == (int)State.Dead)
                    {
                        SetStateColor(prefabInstance, State.Dead);
                    }
                    else
                    {
                        SetStateColor(prefabInstance, State.Alive);
                    }
                }
            }
        }

        int CountLiveNeighbours(int x, int y)
        {
            int sum = 0;
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if(x+i == -1 || x+i == rows || y+j == -1 || y+j == cols)
                    {
                        continue;
                    }
                    sum += map[x + i, y + j];
                }
            }
            sum -= map[x, y];
            return sum;
        }

        void SetStateColor(GameObject prefabinstance, State state)
        {
            Renderer rend = prefabinstance.GetComponent<Renderer>();
            if (state == State.Dead)
            {
                rend.material.color = Color.white;
            }
            else
            {
                rend.material.color = Color.black;
            }
        }

        public State GetState(float x, float y)
        {
            int X = (int)((float)rows / 2 - y - 0.5f);
            int Y = (int)((float)cols / 2 + x - 0.5f);
            return (State)map[X, Y];
        }

        public void SetState(float x, float y, State state)
        {
            int X = (int)((float)rows / 2 - y - 0.5f);
            int Y = (int)((float)cols / 2 + x - 0.5f);
            map[X, Y] = (int)state;
        }
    }
}
