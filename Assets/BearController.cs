using UnityEngine;

public class BearController : MonoBehaviour
{
    public GameTimer gameTimer;

    private Animator animator;
    private bool triggered20 = false;
    private bool triggered10 = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameTimer == null) return;

        if (!triggered20 && gameTimer.timeRemaining <= 20f)
        {
            triggered20 = true;
            animator.SetTrigger("getUp");
        }
        else if (!triggered10 && gameTimer.timeRemaining <= 10f)
        {
            triggered10 = true;
            animator.SetTrigger("stand");
        }
    }
}
