using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodPosition;
    private GameObject foodgameObject;
    private int width;
    private int height;
    private Snake snake;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake)
    {
        this.snake = snake;

        SpawnFood();
    }


    private void SpawnFood()
    {
        do
        {
            foodPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetPosition() == foodPosition);

        foodgameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodgameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodgameObject.transform.position = new Vector3(foodPosition.x, foodPosition.y);
    }

    public bool SnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodPosition)
        {
            Object.Destroy(foodgameObject);
            SpawnFood();
            GameHendler.AddScore();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2Int ValidatePosition(Vector2Int Position)
    {
        if (Position.x < 0)
        {
            Position.x = width - 1;
        }
        if (Position.x > width - 1)
        {
            Position.x = 0;
        }
        if (Position.y < 0)
        {
            Position.y = height - 1;
        }
        if (Position.y > height - 1)
        {
            Position.y = 0;
        }
        return Position;
    }
}