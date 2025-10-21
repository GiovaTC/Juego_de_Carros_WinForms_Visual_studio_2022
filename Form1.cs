using System.Drawing.Text;

namespace JuegoCarros
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer gameTimer;
        private PictureBox playerCar;
        private List<PictureBox> obstacles = new List<PictureBox>();
        private Random rnd = new Random();
        private int Speed = 6;
        private int obstacleSpeed = 4;
        private int score = 0;
        private Label lblScore;
        private Button btnStart;
        private Button btnSave;
        private TextBox txtPlayer;


        // TODO: Cambia aquí tu cadena de conexión Oracle
        // Ejemplo EZCONNECT: "User Id=MYUSER;Password=MYPASS;Data Source=hostname:1521/ORCLPDB1"
        private string oracleConnectionString = "User Id=system;Password=Tapiero123;Data Source=localhost:1521/orcl";

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Juego de Carros - VS2022 + Oracle 19c";
            this.ClientSize = new Size(400, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblScore = new Label() { Text = "Score: 0", Location = new Point(10, 10), AutoSize = true };
            this.Controls.Add(lblScore);

            txtPlayer = new TextBox() { Location = new Point(100, 8), Width = 180, Text = "Jugador1" };
            this.Controls.Add(txtPlayer);

            btnStart = new Button() { Text = "Iniciar", Location = new Point(290, 6), Width = 80 };
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);

            btnSave = new Button() { Text = "Guardar", Location = new Point(290, 40), Width = 80, Enabled = false };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            playerCar = new PictureBox();
            playerCar.Size = new Size(40, 70);
            playerCar.BackColor = Color.Blue;
            playerCar.Location = new Point((this.ClientSize.Width - playerCar.Width) / 2, this.ClientSize.Height - playerCar.Height - 30);
            this.Controls.Add(playerCar);

            gameTimer = new Timer();
            gameTimer.Interval = 20; // ~50 FPS
            gameTimer.Tick += GameTimer_Tick;

            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
        }

        private bool leftPressed = false;
        private bool rightPressed = false;


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) leftPressed = true;
            if (e.KeyCode == Keys.Right) rightPressed = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) leftPressed = false;
            if (e.KeyCode == Keys.Right) rightPressed = false;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            ResetGame();
            gameTimer.Start();
            btnSave.Enabled = false;
            btnStart.Enabled = false;
        }

        private void ResetGame()
        {
            // limpiar obstaculos
            foreach (var obs in obstacles) this.Controls.Remove(obs);
            obstacles.Clear();
            score = 0;
            lblScore.Text = "Score: 0";
            playerCar.Location = new Point((this.ClientSize.Width - playerCar.Width) / 2, this.ClientSize.Height - playerCar.Height - 30);
        }

        private int tickCounter = 0;

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            tickCounter++;

            // mover jugador
            if (leftPressed) playerCar.Left = Math.Max(0, playerCar.Left - Speed);
            if (rightPressed) playerCar.Left = Math.Min(this.ClientSize.Width - playerCar.Width, playerCar.Left + Speed);

            // crear obstaculos cada cierta cantidad de ticks
            if (tickCounter % 30 == 0)
            {
                CreateObstacle();
            }

            // mover obstaculos
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                var obs = obstacles[i];
                obs.Top += obstacleSpeed;
                // chequear colision
                if (obs.Top > this.ClientSize.Height)
                {
                    // pasado de pantalla -> quitar y sumar puntaje
                    this.Controls.Remove(obs);
                    obstacles.RemoveAt(i);
                    score += 10;
                    lblScore.Text = "Score: " + score;
                    continue;
                }

                // collision
                if (obs.Bounds.IntersectsWith(playerCar.Bounds))
                {
                    GameOver();
                    return;
                }

                // aumentar dificultad cada tantos puntos
                if (score > 0 && score % 100 == 0)
                {
                    obstacleSpeed = 6 + (score / 100);
                }
            }

            Private void CreateObstacle()
            {
                var width = rnd.Next(30, 80);
                var obs = new PictureBox();
                obs.Size = new Size(width, rnd.Next(20, 50));
                obs.BackColor = Color.Red;
                int x = rnd.Next(0, this.ClientSize.Width - obs.Width);
                obs.Location = new Point(x, -obs.Height);
                obstacles.Add(obs);
                this.Controls.Add(obs);
                obs.BringToFront();
                playerCar.BringToFront();
            }
        }
    }
}
