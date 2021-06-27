using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private enum State
    {
        Alive,
        Dead
    }
    private State state;
    private Direction MoveDirection; //kierunek
    private Vector2Int Position;
    private float MoveTimer;
    private float MoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize; //powiekszanie weza
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }


    private void Awake()
    {
        Position = new Vector2Int(10, 10); //pozycja startowa
        MoveTimerMax = .4f;
        MoveTimer = MoveTimerMax;
        MoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                KeyInput();
                Movement();
                break;
            case State.Dead:
                break;
        }
    }

    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (MoveDirection != Direction.Down)
            {
                MoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (MoveDirection != Direction.Up)
            {
                MoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (MoveDirection != Direction.Right)
            {
                MoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (MoveDirection != Direction.Left)
            {
                MoveDirection = Direction.Right;
            }
        }
    }

    private void Movement()
    {
        MoveTimer += Time.deltaTime * 3;
        if (MoveTimer >= MoveTimerMax)
        {
            MoveTimer -= MoveTimerMax;

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(Position, MoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int MoveDirectionVector;
            switch (MoveDirection)
            {
                default:
                case Direction.Right:   MoveDirectionVector = new Vector2Int(1, 0); break;
                case Direction.Left:    MoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up:      MoveDirectionVector = new Vector2Int(0, 1); break;
                case Direction.Down:    MoveDirectionVector = new Vector2Int(0, -1); break;
            }

            Position += MoveDirectionVector;

            Position = levelGrid.ValidatePosition(Position);

            bool snakeAteFood = levelGrid.SnakeEatFood(Position);
            if (snakeAteFood)
            {
                snakeBodySize++; //powiekszenie weza
                CreateSnakeBody();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartPosition = snakeBodyPart.GetSnakePosition();
                if (Position == snakeBodyPartPosition)
                {
                    state = State.Dead;             //KONIEC GRY
                    GameHendler.SnakeDied();
                }
            }


            transform.position = new Vector3(Position.x, Position.y);           //obracanie
            transform.eulerAngles = new Vector3(0, 0, AngelFromVector(MoveDirectionVector) - 90);



            UpdateSnakeBodyParts();
        }


    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetPosition(snakeMovePositionList[i]);
        }
    }

    private float AngelFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetPosition()
    {
        return Position;
    }

    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetPosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;

            transform.position = new Vector3(snakeMovePosition.GetSnakePosition().x, snakeMovePosition.GetSnakePosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    angle = 0;
                    break;
                case Direction.Down:
                    angle = 180;
                    break;
                case Direction.Left:
                    angle = -90;
                    break;
                case Direction.Right:
                    angle = 90;
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetSnakePosition()
        {
            return snakeMovePosition.GetSnakePosition();
        }
    }

    private class SnakeMovePosition
    {
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(Vector2Int Position, Direction direction)
        {
            this.gridPosition = Position;
            this.direction = direction;
        }
        public Vector2Int GetSnakePosition()
        {
            return gridPosition;
        }
        public Direction GetDirection()
        {
            return direction;
        }
    }
    
}
