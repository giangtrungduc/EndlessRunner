using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    public GameObject[] childs;
    public float endX;
    public float speedMultiplier;
    private Queue<GameObject> queue = new Queue<GameObject>();
    private Vector3[] startPositions;

    private void Start()
    {
        foreach(GameObject child in childs)
        {
            child.GetComponent<Obstacles>().RandomObstacle();
            queue.Enqueue(child);
        }
        SaveStartPositions();
    }
    private void Update()
    {
        MoveObstacle();
    }
    private void SaveStartPositions()
    {
        startPositions = new Vector3[childs.Length];

        for(int i = 0; i < childs.Length; i++)
        {
            startPositions[i] = childs[i].transform.position;
        }
    }
    public void ResetSpawner()
    {
        queue.Clear();

        for(int i = 0; i < childs.Length; i++)
        {
            childs[i].SetActive(true);
            childs[i].transform.position = startPositions[i];

            childs[i].GetComponent<Obstacles>().RandomObstacle();

            queue.Enqueue(childs[i]);
        }
    }
    private void MoveObstacle()
    {
        float moveSpeed = 0f;
        if(GameManager.Instance != null)
        {
            moveSpeed = GameManager.Instance.GameSpeed * speedMultiplier;
        }
        foreach(GameObject child in childs)
        {
            child.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            if(child.transform.position.x < endX)
            {
                child.GetComponent<Obstacles>().RandomObstacle();
                SetPosObstacle();
            }
        }
    }
    private void SetPosObstacle()
    {
        GameObject firstChild = queue.Dequeue();

        GameObject secondChild = null;
        foreach(GameObject child in queue)
        {
            secondChild = child;
        }

        firstChild.transform.position = new Vector3(secondChild.transform.position.x + 15f, firstChild.transform.position.y, firstChild.transform.position.z);

        queue.Enqueue(firstChild);
    }
}
