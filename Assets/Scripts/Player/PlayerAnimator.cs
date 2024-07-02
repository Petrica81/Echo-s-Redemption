using UnityEngine;

public class PlayerAnimator : BaseRecognizer
{
    private PlayerController _controller;
    private Animator _headAnim;
    private Animator _chestAnim;
    private Animator _footAnim;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _headAnim = transform.GetChild(0).GetComponent<Animator>();
        _chestAnim = transform.GetChild(1).GetComponent<Animator>();
        _footAnim = transform.GetChild(2).GetComponent<Animator>();
        if (_headAnim == null || _chestAnim == null || _footAnim == null)
            Debug.Log("Eroare , nu gasesc componenta de animator pe unul dintre copiii playerului");
        UpdateDirection(0, -1);
        base.actions.Add("look up",() => { if (_controller.currentState == PlayerState.idle) UpdateDirection(0, 1); });
        base.actions.Add("look down", () => { if (_controller.currentState == PlayerState.idle) UpdateDirection(0, -1); });
        base.actions.Add("look right", () => { if (_controller.currentState == PlayerState.idle) UpdateDirection(1, 0); });
        base.actions.Add("look left", () => { if (_controller.currentState == PlayerState.idle) UpdateDirection(-1, 0); });

    }

    /// <summary>
    /// Updates the direction parameters for all animators.
    /// </summary>
    /// <param name="_x">Horizontal direction.</param>
    /// <param name="_y">Vertical direction.</param>
    public void UpdateDirection(float x, float y)
    {
        _headAnim.SetFloat("X", x);
        _headAnim.SetFloat("Y", y);

        _chestAnim.SetFloat("X", x);
        _chestAnim.SetFloat("Y", y);

        _footAnim.SetFloat("X", x);
        _footAnim.SetFloat("Y", y);
    }
    public Vector2Int GetDirection()
    {
        int x = (int)_headAnim.GetFloat("X");
        int y = (int)_headAnim.GetFloat("Y");
        return new Vector2Int(x, y);
    }

    public void PlayIdleAnim(string helmetName, string chestplateName, string pantsName)
    {
        _headAnim.Play(helmetName + "IdleGloves");
        _chestAnim.Play(chestplateName + "IdleGloves");
        _footAnim.Play(pantsName + "IdleGloves");
    }
    public void PlayAttackAnim()
    {
        _headAnim.SetBool("Attack", true);
        _chestAnim.SetBool("Attack", true);
        _footAnim.SetBool("Attack", true);
    }
    public void StopAttackAnim()
    {
        _headAnim.SetBool("Attack", false);
        _chestAnim.SetBool("Attack", false);
        _footAnim.SetBool("Attack", false);
    }
    public float GetCurrentAnimationDuration()
    {
        return _chestAnim.GetCurrentAnimatorStateInfo(0).length;
    }

}
