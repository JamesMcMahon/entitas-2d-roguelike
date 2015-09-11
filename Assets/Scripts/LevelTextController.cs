using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextController : MonoBehaviour
{
    // Delay time in seconds to display level text.
    public float displayDelay = 1f;
    // Time to wait before hiding level text, in seconds.
    public float hideDelay = 2f;
    public Text levelText;

    int currentLevel;
    Image levelImage;
    Pool pool;

    // Use awake to ensure that this fires before the systems boot
    // otherwise it misses the initial level set
    void Awake()
    {
        levelImage = GetComponent<Image>();

        pool = Pools.pool;
        pool.GetGroup(Matcher.Level).OnEntityAdded += (group, entity, index, component) =>
        {
            currentLevel = pool.level.level;
            pool.isLevelTransitionDelay = true;
            Invoke("ShowLevelImage", displayDelay);
        };
        pool.GetGroup(Matcher.GameOver).OnEntityAdded += (group, entity, index, component) =>
        {
            GameOver();
        };
    }

    void ShowLevelImage()
    {
        levelImage.enabled = true;
        levelText.text = "Day " + currentLevel;
        levelText.enabled = true;
        Invoke("HideLevelImage", hideDelay);
    }

    void HideLevelImage()
    {
        levelText.enabled = false;
        levelImage.enabled = false;
        pool.isLevelTransitionDelay = false;
    }

    void GameOver()
    {
        levelImage.enabled = true;
        levelText.text = "After " + currentLevel + " days, you starved.";
        levelText.enabled = true;
    }
}
