using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PersonelMaasTakipSistemi
{
    public partial class Form1 : Form
    {
        public class Personel
        {
            public string No { get; set; }
            public string AdSoyad { get; set; }
            public string Departman { get; set; }
            public double Maas { get; set; }
            public int Mesai { get; set; }
            public double Prim { get; set; }
            public double ToplamMaas { get; set; }
        }

        List<Personel> personeller = new List<Personel>();
        int seciliIndex = -1; // Seçili satırın indexi

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Sütunları manuel olarak tanımla
            dataGridView1.Columns.Add("No", "Personel No");
            dataGridView1.Columns.Add("AdSoyad", "Ad Soyad");
            dataGridView1.Columns.Add("Departman", "Departman");
            dataGridView1.Columns.Add("Maas", "Temel Maaş");
            dataGridView1.Columns.Add("Mesai", "Mesai Saati");
            dataGridView1.Columns.Add("Prim", "Prim");
            dataGridView1.Columns.Add("ToplamMaas", "Toplam Maaş");

            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
        }

        // Grid'i listeden yeniden doldur
        private void GridiYenile()
        {
            dataGridView1.Rows.Clear();
            foreach (Personel p in personeller)
            {
                dataGridView1.Rows.Add(
                    p.No,
                    p.AdSoyad,
                    p.Departman,
                    p.Maas.ToString("F2") + " TL",
                    p.Mesai + " saat",
                    p.Prim.ToString("F2") + " TL",
                    p.ToplamMaas.ToString("F2") + " TL"
                );
            }
            seciliIndex = -1;
        }

        // Satıra tıklayınca textbox'lara doldur
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= personeller.Count) return;

            seciliIndex = e.RowIndex;
            Personel p = personeller[seciliIndex];

            txtNo.Text = p.No;
            txtAdSoyad.Text = p.AdSoyad;
            txtDepartman.Text = p.Departman;
            txtMaas.Text = p.Maas.ToString();
            txtMesai.Text = p.Mesai.ToString();
            txtPrim.Text = p.Prim.ToString();
        }

        // Sayısal alanları kontrol eden yardımcı metot
        private bool NotkontrolEt(out double maas, out int mesai, out double prim)
        {
            maas = 0; mesai = 0; prim = 0;

            if (!double.TryParse(txtMaas.Text, out maas) || maas < 0)
            {
                MessageBox.Show("Maaş geçerli bir sayı olmalıdır!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtMesai.Text, out mesai) || mesai < 0)
            {
                MessageBox.Show("Mesai geçerli bir tam sayı olmalıdır!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!double.TryParse(txtPrim.Text, out prim) || prim < 0)
            {
                MessageBox.Show("Prim geçerli bir sayı olmalıdır!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // EKLE
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNo.Text) ||
                string.IsNullOrWhiteSpace(txtAdSoyad.Text) ||
                string.IsNullOrWhiteSpace(txtDepartman.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double maas, prim;
            int mesai;
            if (!NotkontrolEt(out maas, out mesai, out prim)) return;

            Personel p = new Personel();
            p.No = txtNo.Text.Trim();
            p.AdSoyad = txtAdSoyad.Text.Trim();
            p.Departman = txtDepartman.Text.Trim();
            p.Maas = maas;
            p.Mesai = mesai;
            p.Prim = prim;
            p.ToplamMaas = p.Maas + (p.Mesai * 200) + p.Prim;

            personeller.Add(p);
            GridiYenile();
            AlanlariTemizle();
            MessageBox.Show("Personel eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // SİL
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (seciliIndex < 0 || seciliIndex >= personeller.Count)
            {
                MessageBox.Show("Lütfen önce bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult sonuc = MessageBox.Show("Bu personeli silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                personeller.RemoveAt(seciliIndex);
                GridiYenile();
                AlanlariTemizle();
            }
        }

        // GÜNCELLE
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (seciliIndex < 0 || seciliIndex >= personeller.Count)
            {
                MessageBox.Show("Lütfen önce bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double maas, prim;
            int mesai;
            if (!NotkontrolEt(out maas, out mesai, out prim)) return;

            Personel p = personeller[seciliIndex];
            p.No = txtNo.Text.Trim();
            p.AdSoyad = txtAdSoyad.Text.Trim();
            p.Departman = txtDepartman.Text.Trim();
            p.Maas = maas;
            p.Mesai = mesai;
            p.Prim = prim;
            p.ToplamMaas = p.Maas + (p.Mesai * 200) + p.Prim;

            GridiYenile();
            AlanlariTemizle();
            MessageBox.Show("Personel güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // HESAPLA
        private void btnHesapla_Click(object sender, EventArgs e)
        {
            double maas, prim;
            int mesai;
            if (!NotkontrolEt(out maas, out mesai, out prim)) return;

            double toplam = maas + (mesai * 200) + prim;
            MessageBox.Show(
                "Temel Maaş     : " + maas + " TL\n" +
                "Mesai (" + mesai + " x 200) : " + (mesai * 200) + " TL\n" +
                "Prim           : " + prim + " TL\n" +
                "─────────────────────\n" +
                "Toplam Maaş    : " + toplam + " TL",
                "Maaş Hesaplama",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // TEMİZLE
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            AlanlariTemizle();
        }

        private void AlanlariTemizle()
        {
            txtNo.Text = "";
            txtAdSoyad.Text = "";
            txtDepartman.Text = "";
            txtMaas.Text = "";
            txtMesai.Text = "";
            txtPrim.Text = "";
            seciliIndex = -1;
        }
    }
}
