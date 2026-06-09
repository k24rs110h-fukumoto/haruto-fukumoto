using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public static ElementManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public float GetElementMultiplier(MagicElement attack, MagicElement target)
    {
        if (attack == MagicElement.Neutral ||
            target == MagicElement.Neutral)
        {
            return 1f;
        }

        switch (attack)
        {
            case MagicElement.Fire:

                if (target == MagicElement.Wind)
                    return 1.5f;

                if (target == MagicElement.Water)
                    return 0.5f;

                break;

            case MagicElement.Water:

                if (target == MagicElement.Fire)
                    return 1.5f;

                if (target == MagicElement.Earth)
                    return 0.5f;

                break;

            case MagicElement.Wind:

                if (target == MagicElement.Earth)
                    return 1.5f;

                if (target == MagicElement.Fire)
                    return 0.5f;

                break;

            case MagicElement.Earth:

                if (target == MagicElement.Water)
                    return 1.5f;

                if (target == MagicElement.Wind)
                    return 0.5f;

                break;

            case MagicElement.Light:

                if (target == MagicElement.Dark)
                    return 1.5f;

                break;

            case MagicElement.Dark:

                if (target == MagicElement.Light)
                    return 1.5f;

                break;
        }

        return 1f;
    }
}
