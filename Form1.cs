namespace JuegoCarros
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer gameTimer;
        private PictureBox playerCar;
        private List<PictureBox> obstacles = new List<PictureBox>();
        private Random random = new Random();
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

        /*
        private void Form1_Load(object sender, EventArgs e)
        {

        }*/
    }
}
