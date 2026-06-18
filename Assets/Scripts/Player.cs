using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 targetPos;
    private Vector3 currentTargetPos;

    public GameObject vfxParticle;
    public GameObject vfxParticle1;

    [Header("Player Input")]
    public float minSwipeDistance = 80f;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float defaultDuration = 0.08f;
    [SerializeField] private float defaultMagnitude = 0.06f;

    [Header("Sound")]
    public AudioSource aus;
    public AudioClip clip;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if(mainCamera != null)
        {
            originalPosition = mainCamera.transform.localPosition;
        }
    }
    private void Start()
    {
        currentTargetPos = transform.position;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position;
        }
        if(touch.phase == TouchPhase.Ended)
        {
            endTouchPosition = touch.position;

            Vector2 swipeDelta = endTouchPosition - startTouchPosition;

            if(Mathf.Abs(swipeDelta.y) < minSwipeDistance) return;

            if(Mathf.Abs(swipeDelta.y) < Mathf.Abs(swipeDelta.x)) return;

            if(swipeDelta.y > 0 && transform.position != targetPos)
            {
                PlayVFX();
                MovePlayer(1);
                Shake(defaultDuration, defaultMagnitude);
            }
            else if(swipeDelta.y < 0 && transform.position != targetPos * -1f)
            {
                PlayVFX();
                MovePlayer(-1);
                Shake(defaultDuration, defaultMagnitude);
            }
        }
    }

    private void MovePlayer(int direction)
    {
        if(transform.position == Vector3.zero)
        {
            currentTargetPos = targetPos * direction;
        }
        else
        {
            if(transform.position.y > 0f && direction < 0)
            {
                currentTargetPos = Vector3.zero;
            }
            if(transform.position.y < 0f && direction > 0f)
            {
                currentTargetPos = Vector3.zero;
            }
        }

        transform.position = currentTargetPos;
    }

    public void Shake(float duration, float magnitude)
    {
        if(shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }
    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            if(mainCamera != null)
            {
                mainCamera.transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if(mainCamera != null)
        {
            mainCamera.transform.localPosition = originalPosition;
        }
        shakeCoroutine = null;
    }
    private void PlayVFX()
    {
        if(vfxParticle != null)
        {
            GameObject vfx = Instantiate(vfxParticle, transform.position, Quaternion.identity);

            Destroy(vfx, 2f);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if(GameManager.Instance != null)
            {
                Shake(defaultDuration, defaultMagnitude + 0.2f);
                PlayClip();
                if(vfxParticle1 != null)
                {
                    GameObject vfx = Instantiate(vfxParticle1, collision.transform.position, Quaternion.identity);
                    collision.gameObject.SetActive(false);
                    Destroy(vfx, 2f);
                }
                if(vfxParticle != null)
                {
                    GameObject vfx = Instantiate(vfxParticle, transform.position, Quaternion.identity);

                    Destroy(vfx, 2f);
                }
                GameManager.Instance.GameOver();
            }
        }
    }
    private void PlayClip()
    {
        if(aus && clip)
        {
            aus.PlayOneShot(clip);
        }
    }
}