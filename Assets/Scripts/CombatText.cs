using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] private Text hpText;
    public void OnInit(float damage)
    {
        print(transform.position);
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
