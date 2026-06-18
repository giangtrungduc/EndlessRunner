using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject[] childs;

    public void RandomObstacle()
    {
        int first = Random.Range(0, childs.Length);
        int second = Random.Range(0, childs.Length - 1);

        if(second >= first)
        {
            second++;
        }

        for(int i = 0; i < childs.Length; i++)
        {
            if(i == first || i == second)
            {
                childs[i].gameObject.SetActive(true);
            }
            else
            {
                childs[i].gameObject.SetActive(false);
            }
        }
    }
}
