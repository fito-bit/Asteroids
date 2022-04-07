using Supyrb;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private int scorePerDestroy;
    [SerializeField] private Image laserChargeImg;
    [SerializeField] private Text laserChargesText;
    [SerializeField] private Text laserChargeTime;
    [SerializeField] private Text shipPos;
    [SerializeField] private Text shipRotation;
    [SerializeField] private Text shipInstantSpeed;
    private int score = 0;

    private void Awake()
    {
        Signals.Get<AddScoreSignal>().AddListener(UpdateScore);
        Signals.Get<LoseSignal>().AddListener(ShowLosePanel);
        Signals.Get<UpdateLaserBarSignal>().AddListener(SetLaserBar);
        Signals.Get<SetShipDataSignal>().AddListener(SetShipData);
    }

    void SetLaserBar(float currentTime,float startTime,int charges)
    {
        laserChargeTime.text = "Laser charge time: " + startTime;
        laserChargesText.text = "Laser charges: " + charges;
        laserChargeImg.fillAmount = 1-currentTime / startTime;
    }

    void SetShipData(Vector3 position,Vector3 rotation,Vector3 velocity)
    {
        shipPos.text = "Ship position: " + position;
        shipRotation.text = "Ship rotation: " + rotation;
        shipInstantSpeed.text = "Ship instant speed: " + velocity;
    }

    void UpdateScore()
    {
        score += scorePerDestroy;
        scoreText.text = "Score: " + score;
    }

    void ShowLosePanel()
    {
        finalScoreText.text = "Score: " + score;
        losePanel.SetActive(true);
    }
    
    public void Restart()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        Signals.Get<RestartSignal>().Dispatch();
        losePanel.SetActive(false);
    }
}
