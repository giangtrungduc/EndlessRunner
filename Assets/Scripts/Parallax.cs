using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float startX = 2f;
    [SerializeField] private float endX = -2f;
    public GameObject[] childsTop;
    public GameObject[] childsDown;

    private void Update()
    {
        MoveChild();
    }
    private void MoveChild()
    {
        float moveSpeed = 0f;
        if(GameManager.Instance != null)
        {
            moveSpeed = GameManager.Instance.GameSpeed * speedMultiplier;
        }
        foreach(GameObject child in childsTop)
        {
            child.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            if(child.transform.position.x <= endX)
            {
                child.transform.position = new Vector3(startX, child.transform.position.y, child.transform.position.z);
            }
        }
        foreach(GameObject child in childsDown)
        {
            child.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            if(child.transform.position.x <= endX)
            {
                child.transform.position = new Vector3(startX, child.transform.position.y, child.transform.position.z);
            }
        }
    }
}
