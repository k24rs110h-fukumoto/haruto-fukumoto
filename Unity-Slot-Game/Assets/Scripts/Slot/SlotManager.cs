using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotManager : MonoBehaviour
{
    public Button startButton;
    public GameObject pushButton;
    public GameObject blackoutPanel;

    public TextMeshProUGUI effectText;
    public TextMeshProUGUI foreNoticeText;
    public TextMeshProUGUI sevenChanceText;

    public Image flashImage;

    public Image reel1Image;
    public Image reel2Image;
    public Image reel3Image;

    public Sprite sevenSprite;
    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Sprite staffSprite;
    public Sprite jewelSprite;
    public Sprite barSprite;

    public Image cutInImage;
    public Sprite swordCutInSprite;
    public Sprite shieldCutInSprite;
    public Sprite mageCutInSprite;
    public Sprite allCutInSprite;

    bool isPlaying = false;
    bool foreNoticeActive = false;
    bool isSpinning = false;

    Sprite[] symbols;

    void Start()
    {
        symbols = new Sprite[]
        {
            sevenSprite,
            swordSprite,
            shieldSprite,
            staffSprite,
            jewelSprite,
            barSprite
        };

        pushButton.SetActive(false);
        blackoutPanel.SetActive(false);
        cutInImage.gameObject.SetActive(false);

        flashImage.gameObject.SetActive(true);
        flashImage.color = new Color(1, 1, 1, 0);

        foreNoticeText.text = "";
        sevenChanceText.text = "";
        effectText.text = "STARTを押してください";

        startButton.onClick.AddListener(StartSlot);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.slotBGM);
        }
    }

    public void StartSlot()
    {
        if (isPlaying) return;

        isPlaying = true;
        isSpinning = true;
        foreNoticeActive = false;

        GameData.Instance.expectation = 0;
        GameData.Instance.effectLogs.Clear();

        pushButton.SetActive(false);
        blackoutPanel.SetActive(false);
        cutInImage.gameObject.SetActive(false);

        foreNoticeText.text = "";
        sevenChanceText.text = "";
        effectText.text = "";

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.startSE);
        }

        StartCoroutine(ReelLoop());
        StartCoroutine(SlotSequence());
    }

    IEnumerator ReelLoop()
    {
        while (isSpinning)
        {
            reel1Image.sprite = GetRandomSymbol();
            reel2Image.sprite = GetRandomSymbol();
            reel3Image.sprite = GetRandomSymbol();

            yield return new WaitForSeconds(0.08f);
        }
    }

    IEnumerator SlotSequence()
    {
        effectText.text = "リール回転開始";
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(PlayForeNotice());

        int effectCount = Random.Range(3, 7);

        for (int i = 0; i < effectCount; i++)
        {
            int rand = Random.Range(0, 100);

            if (rand < 50)
            {
                yield return StartCoroutine(PlayFlash());
            }
            else if (rand < 70)
            {
                yield return StartCoroutine(PlayCutIn());
            }
            else if (rand < 85)
            {
                yield return StartCoroutine(PlaySevenChance());
            }
            else
            {
                yield return StartCoroutine(PlayBlackout());
            }
        }

        if (foreNoticeActive)
        {
            AddEffect("激アツ展開！！", 20);
            yield return new WaitForSeconds(1.2f);
        }

        effectText.text = "ラストチャンス！";
        yield return new WaitForSeconds(1f);

        effectText.text = "PUSH!!";
        pushButton.SetActive(true);

        pushButton.GetComponent<PushButtonController>().UpdatePushButton();

        isPlaying = false;
    }

    IEnumerator PlayForeNotice()
    {
        int rand = Random.Range(0, 100);

        if (rand < 25)
        {
            foreNoticeActive = true;
            foreNoticeText.text = "先バレ発生!!";

            GameData.Instance.AddExpectation(30);
            GameData.Instance.AddEffectLog("先バレ発生!!");

            yield return StartCoroutine(FlashScreen(Color.red, 0.6f));

            yield return new WaitForSeconds(1.0f);

            foreNoticeText.text = "";
        }
    }

    IEnumerator PlayFlash()
    {
        int rand = Random.Range(0, 100);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.flashSE);
        }

        if (rand < 50)
        {
            AddEffect("フラッシュ！", 10);
            yield return StartCoroutine(FlashScreen(Color.white, 0.6f));
        }
        else if (rand < 80)
        {
            AddEffect("強フラッシュ！", 30);
            yield return StartCoroutine(FlashScreen(Color.red, 0.7f));
        }
        else
        {
            AddEffect("激アツフラッシュ！！", 60);
            yield return StartCoroutine(FlashScreen(Color.yellow, 0.8f));
        }

        yield return new WaitForSeconds(0.7f);
    }

    IEnumerator PlayCutIn()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.cutInSE);
        }

        int rand = Random.Range(0, 100);

        cutInImage.gameObject.SetActive(true);

        if (rand < 25)
        {
            cutInImage.sprite = swordCutInSprite;
            AddEffect("剣士カットイン！", 30);
        }
        else if (rand < 50)
        {
            cutInImage.sprite = shieldCutInSprite;
            AddEffect("盾カットイン！", 30);
        }
        else if (rand < 80)
        {
            cutInImage.sprite = mageCutInSprite;
            AddEffect("魔導士カットイン！", 45);
        }
        else
        {
            cutInImage.sprite = allCutInSprite;
            AddEffect("全員集合カットイン！！", 100);
        }

        yield return new WaitForSeconds(1.8f);

        cutInImage.gameObject.SetActive(false);
    }

    IEnumerator PlaySevenChance()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.sevenChanceSE);
        }

        sevenChanceText.text = "7テンパイ!!";
        AddEffect("7テンパイ!!", 80);

        yield return StartCoroutine(FlashScreen(Color.yellow, 0.8f));

        yield return new WaitForSeconds(1.2f);

        sevenChanceText.text = "";
    }

    IEnumerator PlayBlackout()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.blackoutSE);
        }

        blackoutPanel.SetActive(true);
        effectText.text = "";

        yield return new WaitForSeconds(1.2f);

        blackoutPanel.SetActive(false);

        AddEffect("ブラックアウト！！", 75);

        yield return new WaitForSeconds(1.2f);
    }

    IEnumerator FlashScreen(Color color, float alpha)
    {
        flashImage.color = new Color(color.r, color.g, color.b, alpha);

        yield return new WaitForSeconds(0.12f);

        flashImage.color = new Color(color.r, color.g, color.b, 0f);
    }

    void AddEffect(string effectName, int expectationValue)
    {
        effectText.text = effectName;
        GameData.Instance.AddExpectation(expectationValue);
        GameData.Instance.AddEffectLog(effectName);
    }

    public void StopSlotReels()
    {
        isSpinning = false;
        StopReelsByExpectation();
    }

    void StopReelsByExpectation()
    {
        int expectation = GameData.Instance.expectation;

        if (expectation >= 180)
        {
            reel1Image.sprite = sevenSprite;
            reel2Image.sprite = sevenSprite;
            reel3Image.sprite = sevenSprite;
        }
        else if (expectation >= 120)
        {
            reel1Image.sprite = swordSprite;
            reel2Image.sprite = swordSprite;
            reel3Image.sprite = swordSprite;
        }
        else if (expectation >= 70)
        {
            reel1Image.sprite = shieldSprite;
            reel2Image.sprite = shieldSprite;
            reel3Image.sprite = shieldSprite;
        }
        else
        {
            reel1Image.sprite = barSprite;
            reel2Image.sprite = jewelSprite;
            reel3Image.sprite = barSprite;
        }
    }

    Sprite GetRandomSymbol()
    {
        int index = Random.Range(0, symbols.Length);
        return symbols[index];
    }
}