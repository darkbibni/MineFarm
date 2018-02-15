using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour {
    
    public Image rock;

    private void Reset()
    {
        rock = transform.GetChild(0).GetComponent<Image>();
    }

    public void ShakeRock()
    {
        rock.transform.DOShakePosition(0.75f, 100, 50).OnComplete(ResetRockPosition);
    }

    private void ResetRockPosition() 
    {
        rock.transform.localPosition = Vector3.zero;
    }

    public void FeedbackCantMine()
    {
        rock.DOColor(Color.red, 0.25f).OnComplete(ResetRockColor);
    }

    private void ResetRockColor()
    {
        rock.color = Color.white;
    }
}
