using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText CombatTextPrefab;
    
    private float hp;
    private string currentAnimation;
    public bool isDead => hp <= 0;

    // Virtual Keyword is used to modify a method
    // and allow for it to be overridden in a 
    // derived class

    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
        
        healthBar.OnInit(100, transform);
    }

    public virtual void OnDespawn()
    {

    }

    public void OnHit(float damage)
    {
        hp -= damage;
        if (isDead)
        {
            hp = 0;
            Die();
        }

        healthBar.setNewHp(hp);
        print("Show combat text");
 
        print("Character Position: " + transform.position.ToString());
        Instantiate(CombatTextPrefab, transform.position, Quaternion.identity).OnInit(damage);
    }

    protected virtual void Die()
    {
        ChangeAnimation("die");
        Invoke(nameof(OnDespawn), 1f);
    }
    
    // protected keyword: Can make derived class access
    // to the method, but cannot override the method
    protected void ChangeAnimation(string animName)
    {
        if (currentAnimation != animName)
        {
            _animator.ResetTrigger(animName);
            currentAnimation = animName;
            _animator.SetTrigger(currentAnimation);
        }
    }

}
