using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private Text coinText;
    private void Awake()
    {
        instance = this;
    }

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
    
}
