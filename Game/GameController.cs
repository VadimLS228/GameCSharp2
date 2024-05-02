using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public class GameController
    {
        private GameModel model;
        private Timer timer;
        private Label loseLabel;
        private GameForm form;

        public GameController(GameModel model, Timer timer, Label loseLabel, GameForm form)
        {
            this.model = model;
            this.timer = timer;
            this.loseLabel = loseLabel;
            this.form = form;
        }

        public void CheckCollisions(PictureBox player, PictureBox coin, PictureBox stone1,
        PictureBox stone2, PictureBox metall, PictureBox portalRed, PictureBox portalBlue,
        PictureBox portalGreen, PictureBox shipBlue, PictureBox shipGreen,
        PictureBox shipPurple, PictureBox portalBgRed1, PictureBox portalBgRed2,
        PictureBox portalBgBlue1, PictureBox portalBgBlue2, PictureBox portalBgGreen1, PictureBox portalBgGreen2)
        {

 
            if (player.Bounds.IntersectsWith(stone1.Bounds) ||
                player.Bounds.IntersectsWith(stone2.Bounds) ||
                player.Bounds.IntersectsWith(metall.Bounds))
            {
                model.Lose = true;
                StopTimerAndShowLoseMessage();
            }
            if (player.Bounds.IntersectsWith(portalRed.Bounds))
            {
                model.EnterPortal(shipBlue.Image, portalBgRed1.Image, portalBgRed2.Image);
            }
            if (player.Bounds.IntersectsWith(portalBlue.Bounds))
            {
                model.EnterPortal(shipGreen.Image, portalBgBlue1.Image, portalBgBlue2.Image);
            }
            if (player.Bounds.IntersectsWith(portalGreen.Bounds))
            {
                model.EnterPortal(shipPurple.Image, portalBgGreen1.Image, portalBgGreen2.Image);
            }
            

        }

        private void StopTimerAndShowLoseMessage()
        {
            timer.Stop();
            loseLabel.Visible = true;
            GameForm form = (GameForm)Application.OpenForms["GameForm"];
            form.ShowRestartButton();
            form.backgroundMusic.Stop();
            form.lossSound.Play();
        }

    }
}
