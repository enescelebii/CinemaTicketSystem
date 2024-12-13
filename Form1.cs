using CinemaTicketSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaTicketSystem
{
    public partial class Form1 : Form
    {
        private Timer timeUpdater;
        private DatabaseHelper dbHelper;
        private HashSet<PictureBox> toplamSecilenKoltuklar = new HashSet<PictureBox>();
        private bool isMouseDown = false;
        private Point secimBaslangicNoktasi;
        private List<PictureBox> geciciSecilenKoltuklar = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
            InitializeTimeLabel(); 
            dbHelper = new DatabaseHelper();
            dbHelper.CreateDatabase();
            SinemaVerileriniHazirla();
            SinemaSalonlariniListele();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            //KaydetButonu.Click += KaydetButonu_Click;
            panelSeats.MouseDown += PanelSeats_MouseDown;
            panelSeats.MouseMove += PanelSeats_MouseMove;
            panelSeats.MouseUp += PanelSeats_MouseUp;
            treeView1.BringToFront();
        }

        private void InitializeTimeLabel()
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblTime.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
            lblTime.AutoSize = true;

            timeUpdater = new Timer();
            timeUpdater.Interval = 1000;
            timeUpdater.Tick += TimeUpdater_Tick;
            timeUpdater.Start();
        }
        private void TimeUpdater_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void SinemaVerileriniHazirla()
        {
            dbHelper.AddSalon("Salon 1", "Avengers", "10:00");
            dbHelper.AddSalon("Salon 1", "Inception", "14:00");
            dbHelper.AddSalon("Salon 2", "Interstellar", "12:00");
            dbHelper.AddSalon("Salon 2", "Martian", "16:00");
            dbHelper.AddSalon("Salon 3", "FightCLUP", "15:00");
            dbHelper.AddSalon("Salon 3", "AROG", "18:00");

            dbHelper.AddSeats(1, 20);
            dbHelper.AddSeats(2, 20);
            dbHelper.AddSeats(3, 25);
            dbHelper.AddSeats(4, 25);
            dbHelper.AddSeats(5, 30);
            dbHelper.AddSeats(6, 30);

        }

        private void SinemaSalonlariniListele()
        {
            var salonlar = dbHelper.GetSalonlarWithMovies();
            foreach (var salon in salonlar)
            {
                var salonNode = treeView1.Nodes.Add(salon.Name);
                foreach (var film in salon.Films)
                {
                    salonNode.Nodes.Add($"{film.Name} ({film.Time})");
                }
            }
            treeView1.Font = new Font(treeView1.Font.FontFamily, 8, FontStyle.Bold);
            treeView1.ExpandAll();
            treeView1.AfterSelect += TreeView1_AfterSelect;

        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node.Parent == null) return;

            string seciliSalon = e.Node.Parent.Text;
            string seciliFilm = e.Node.Text.Split('(')[0].Trim();

            if (string.IsNullOrEmpty(seciliSalon) || string.IsNullOrEmpty(seciliFilm))
            {
                MessageBox.Show("Salon ya da film bilgisi eksik.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int salonId = GetSalonIdFromName(seciliSalon, seciliFilm);
            if (salonId == -1)
            {
                MessageBox.Show("Salon ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool[] koltuklar = dbHelper.GetSeats(salonId);
            KoltuklariPaneldeGoster(koltuklar);
           
        }

        private void PanelSeats_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            secimBaslangicNoktasi = e.Location;
            geciciSecilenKoltuklar.Clear();
        }

        private void PanelSeats_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;

            Rectangle secimAlanı = GetSelectionRectangle(secimBaslangicNoktasi, e.Location);
            foreach (Control kontrol in panelSeats.Controls)
            {
                if (kontrol is PictureBox pictureBox && !geciciSecilenKoltuklar.Contains(pictureBox))
                    if (secimAlanı.IntersectsWith(pictureBox.Bounds) &&
                        pictureBox.Image != Properties.Resources.chairFull)
                    {
                        pictureBox.Image = Properties.Resources.chairSelected;
                        geciciSecilenKoltuklar.Add(pictureBox);
                    }
            }
        }

        private void PanelSeats_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;

            foreach (var koltuk in geciciSecilenKoltuklar)
            {
                if (!toplamSecilenKoltuklar.Contains(koltuk))
                {
                    toplamSecilenKoltuklar.Add(koltuk);
                    koltuk.Image = Properties.Resources.chairFull;
                }
                koltuk.Image = Properties.Resources.chairFull;
            }

            UpdateSelectedCount();
        }

        // Seçim alanının dikdörtgen şeklinde hesaplanması
        private Rectangle GetSelectionRectangle(Point baslangic, Point bitis)
        {
            return new Rectangle(
                Math.Min(baslangic.X, bitis.X),
                Math.Min(baslangic.Y, bitis.Y),
                Math.Abs(baslangic.X - bitis.X),
                Math.Abs(baslangic.Y - bitis.Y)
            );
        }

        // Koltukları panelde gösterme
        private void KoltuklariPaneldeGoster(bool[] koltuklar)
        {
            int satirSayisi = 5;
            int koltukGenislik = 60, koltukYukseklik = 60, aralik = 10;
            int sutunSayisi = koltuklar.Length / satirSayisi;

            int toplamGenislik = sutunSayisi * (koltukGenislik + aralik) - aralik;
            int toplamYukseklik = satirSayisi * (koltukYukseklik + aralik) - aralik;

            int baslangicX = (panelSeats.Width - toplamGenislik) / 2;
            int baslangicY = (panelSeats.Height - toplamYukseklik) / 2;

            panelSeats.AutoScroll = true;
            panelSeats.Controls.Clear();
            //panelSeats.Controls.Add(SelectedCount);
            //panelSeats.Controls.Add(CancelBtn);
            //panelSeats.Controls.Add(KaydetButonu);
            //panelSeats.Controls.Add(logout);


            for (int i = 0; i < koltuklar.Length; i++)
            {
                int koltukNumarasi = i + 1;
                var pictureBox = new PictureBox
                {
                    Width = koltukGenislik,
                    Height = koltukYukseklik,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = koltuklar[i] ? Properties.Resources.chairFull : Properties.Resources.chair,
                    Tag = i
                };

                pictureBox.Location = new Point(
                    baslangicX + (i % sutunSayisi) * (koltukGenislik + aralik),
                    baslangicY + (i / sutunSayisi) * (koltukYukseklik + aralik)
                );

                pictureBox.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    string text = koltukNumarasi.ToString();
                    using (Font font = new Font("Arial", 8, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        SizeF textSize = g.MeasureString(text, font);
                        PointF textLocation = new PointF(
                            (pictureBox.Width - textSize.Width) / 2,
                            (pictureBox.Height - textSize.Height - 10) / 2
                        );
                        g.DrawString(text, font, brush, textLocation);
                    }
                };

                
                pictureBox.Click += SeatPicture_Click;

                panelSeats.Controls.Add(pictureBox);
            }

            //panelSeats.Invalidate();
        }

        private void SeatPicture_Click(object sender, EventArgs e)
        {
            var clickedSeat = (PictureBox)sender;

            int seatNumber = (int)clickedSeat.Tag + 1; // Koltuk numarası
            string seciliSalon = treeView1.SelectedNode?.Parent?.Text;
            string seciliFilm = treeView1.SelectedNode?.Text.Split('(')[0].Trim();

            if (string.IsNullOrEmpty(seciliSalon) || string.IsNullOrEmpty(seciliFilm))
            {
                MessageBox.Show("Salon veya film seçilmedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int salonId = GetSalonIdFromName(seciliSalon, seciliFilm);
            if (salonId == -1)
            {
                MessageBox.Show("Salon ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Veritabanından koltuğun dolu olup olmadığını kontrol et
            if (dbHelper.IsSeatBooked(salonId, seatNumber))
            {
                MessageBox.Show("Bu koltuk daha önce rezerve edilmiş ve seçilemez.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Koltuk tam seçilmişse (dolmuşsa) tıklanmasına izin verme
            if (clickedSeat.Image == Properties.Resources.chairFull)
                return;

            if (toplamSecilenKoltuklar.Contains(clickedSeat))
            {
                clickedSeat.Image = Properties.Resources.chair;
                toplamSecilenKoltuklar.Remove(clickedSeat);
            }
            else
            {
                clickedSeat.Image = Properties.Resources.chairFull;
                toplamSecilenKoltuklar.Add(clickedSeat);
            }

            UpdateSelectedCount();
        }


        public int GetSalonIdFromName(string salonName, string filmName)
        {
            int salonId = -1;
            using (var connection = dbHelper.Connect())
            {
                string query = "SELECT id FROM Salons WHERE name = @salonName AND film_name = @filmName LIMIT 1";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@salonName", salonName);
                command.Parameters.AddWithValue("@filmName", filmName);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    salonId = Convert.ToInt32(reader["id"]);
                }
                else
                {
                    Console.WriteLine($"Salon veya film bulunamadı: Salon - {salonName}, Film - {filmName}");
                }
            }

            return salonId;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            foreach (var koltuk in toplamSecilenKoltuklar)
            {
                koltuk.Image = Properties.Resources.chair;
            }

            toplamSecilenKoltuklar.Clear();
            UpdateSelectedCount();
        }

        private void UpdateSelectedCount()
        {
            SelectedCount.Text = $"Seçili Koltuk: {toplamSecilenKoltuklar.Count}";
        }

        private void logout_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private async void KaydetButonu_Click(object sender, EventArgs e)
        {
            if (toplamSecilenKoltuklar.Count == 0)
            {
                MessageBox.Show("Herhangi bir koltuk seçilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string seciliSalon = treeView1.SelectedNode?.Parent?.Text;
            string seciliFilm = treeView1.SelectedNode?.Text.Split('(')[0].Trim();

            if (string.IsNullOrEmpty(seciliSalon) || string.IsNullOrEmpty(seciliFilm))
            {
                MessageBox.Show("Salon veya film seçilmedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int salonId = GetSalonIdFromName(seciliSalon, seciliFilm);
            if (salonId == -1)
            {
                MessageBox.Show("Salon ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Asenkron güncelleme işlemi
            foreach (var koltuk in toplamSecilenKoltuklar)
            {
                int seatNumber = (int)koltuk.Tag + 1; // Koltuk numarası tag ile alınır.
                await dbHelper.UpdateSeatStatusAsync(salonId, seatNumber, true); // Koltuğu rezerve et
            }

            MessageBox.Show("Seçilen koltuklar başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Paneli yenile ve güncel koltuk durumlarını göster
            bool[] koltuklar = dbHelper.GetSeats(salonId);
            KoltuklariPaneldeGoster(koltuklar);
            toplamSecilenKoltuklar.Clear();
            UpdateSelectedCount();
        }

        
    }
}