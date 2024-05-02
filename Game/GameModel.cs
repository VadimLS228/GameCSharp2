using Game;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

public class GameModel
{
    private int countCoins = 0;
    private bool lose = false;

    public GameModel()
    {
        GameForm gameForm = (GameForm)Application.OpenForms["GameForm"];

        CurrentPlayerImage = gameForm.shipStandart.Image;
    }
    public GameModel(GameForm gameForm)
    {
        CurrentPlayerImage = gameForm.shipStandart.Image;
    }

    public int CountCoins { get => countCoins; set => countCoins = value; }
    public bool Lose { get => lose; set => lose = value; }
    public bool IsInPortal { get; set; }

    public Image CurrentPlayerImage { get; set; }
    public Image CurrentBackgroundImage1 { get; set; }
    public Image CurrentBackgroundImage2 { get; set; }

    public void EnterPortal(Image playerImage, Image backgroundImage1, Image backgroundImage2)
    {
    IsInPortal = true;
        CurrentPlayerImage = playerImage;
        CurrentBackgroundImage1 = backgroundImage1;
        CurrentBackgroundImage2 = backgroundImage2;
    }

    public void CollectCoin()
    {
        countCoins++;
    }

    public void RestartGame()
    {
        Lose = false;
        CountCoins = 0; // Использование свойства CountCoins
        IsInPortal = false;
    }
}