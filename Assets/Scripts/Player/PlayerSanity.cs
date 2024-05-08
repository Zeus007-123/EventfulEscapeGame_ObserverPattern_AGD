using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    [SerializeField] private float sanityLevel = 100.0f;
    [SerializeField] private float sanityDropRate = 0.2f;
    [SerializeField] private float sanityDropAmountPerEvent = 10f;
    private float maxSanity;
    private PlayerController playerController;

    private void OnEnable()
    {
        EventService.Instance.OnRatRush.AddListener(onSupernaturalEvent);
        EventService.Instance.OnSkullDrop.AddListener(onSupernaturalEvent);
        EventService.Instance.OnPotionDrinkEvent.AddListener(onDrankPotion);
    }

    private void OnDisable()
    {
        EventService.Instance.OnRatRush.RemoveListener(onSupernaturalEvent);
        EventService.Instance.OnSkullDrop.RemoveListener(onSupernaturalEvent);
        EventService.Instance.OnPotionDrinkEvent.RemoveListener(onDrankPotion);
    }

    private void Start()
    {
        maxSanity = sanityLevel;
        playerController = GameService.Instance.GetPlayerController();
    }
    void Update()
    {
        if (playerController.PlayerState == PlayerState.Dead)
            return;

        float sanityDrop = updateSanity();

        increaseSanity(sanityDrop);
    }

    private float updateSanity()
    {
        float sanityDrop = sanityDropRate * Time.deltaTime;
        if (playerController.PlayerState == PlayerState.InDark)
        {
            sanityDrop *= 10f;
        }
        return sanityDrop;
    }

    private void increaseSanity(float amountToDecrease)
    {
        Mathf.Floor(sanityLevel -= amountToDecrease);
        if (sanityLevel <= 0)
        {
            sanityLevel = 0;
            GameService.Instance.GameOver();
        }
        GameService.Instance.GetGameUI().UpdateInsanity(1f - sanityLevel / maxSanity);
    }

    private void decreaseSanity(float amountToIncrease)
    {
        Mathf.Floor(sanityLevel += amountToIncrease);
        if (sanityLevel > 100)
        {
            sanityLevel = 100;
        }
        GameService.Instance.GetGameUI().UpdateInsanity(1f - sanityLevel / maxSanity);
    }
    private void onSupernaturalEvent()
    {
        increaseSanity(sanityDropAmountPerEvent);
    }

    private void onDrankPotion(int potionEffect)
    {
        decreaseSanity(potionEffect);
    }
}