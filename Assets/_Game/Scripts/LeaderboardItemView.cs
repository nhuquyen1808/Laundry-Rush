using TMPro;
using UnityEngine;

public class LeaderboardItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text scoreText;

    public void Setup(LeaderboardItem item)
    {
        if (item == null)
            return;

        rankText.text = item.rank.ToString();
        scoreText.text = item.score.ToString();

        nicknameText.text =
            item.user != null &&
            !string.IsNullOrWhiteSpace(item.user.nickname)
                ? item.user.nickname
                : "Unknown";

        // Có thể load avatar ở đây nếu item.user.avatar có URL.
        
    }
}
