using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelProxy : MonoBehaviour
{
    public RectTransform[] DifficultySkulls;
    public RectTransform[] RewardPictures;
    public Text DifficultyText;
    public Text RewardText;
    public RawImage InfoImage;
    public Button YesButton;
    public Button NoButton;

    public void SetDifficulty(int difficultyMeasure)
    {
        for (int i = 0; i < DifficultySkulls.Length; i++)
        {
            DifficultySkulls[i].gameObject.SetActive(difficultyMeasure > i);
        }
    }

    public void SetInfoImage(Texture newInfoImage)
    {
        InfoImage.texture = newInfoImage;
    }

    public void SetReward(int rewardMeasure)
    {
        for (int i = 0; i < RewardPictures.Length; i++)
        {
            RewardPictures[i].gameObject.SetActive(rewardMeasure > i);
        }
    }
}
