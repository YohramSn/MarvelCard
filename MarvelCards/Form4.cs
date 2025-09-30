using MySql.Data.MySqlClient;
using Mysqlx.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MarvelCards.Form4;

namespace MarvelCards
{
    public partial class Form4 : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        string strSQL;
        Random random = new Random();
        public Form4()
        {
            InitializeComponent();
        }

        public class Carta
        {
            public string Nome { get; set; }
            public int Id { get; set; }
            public int Saude { get; set; }
            public int Dano { get; set; }
            public Image Imagem { get; set; }
        }
        private List<PictureBox> listaPictureBoxJogador;
        private List<PictureBox> listaPictureBoxOponente;
        private List<int> cartasJogador1;
        private List<int> cartasJogador2;
        private void Form4_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            List<int> todasCartas = Enumerable.Range(1, 20).ToList();
            todasCartas = todasCartas.OrderBy(x => random.Next()).ToList();

            cartasJogador1 = todasCartas.Take(5).ToList();
            cartasJogador2 = todasCartas.Skip(5).Take(5).ToList();

            listaPictureBoxJogador = new List<PictureBox> { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            listaPictureBoxOponente = new List<PictureBox> { pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10 };

            // mostra as cartas
            MostrarCartas(cartasJogador1, listaPictureBoxJogador);
            MostrarCartas(cartasJogador2, listaPictureBoxOponente);
            foreach (var pb in listaPictureBoxOponente)
            {
                pb.Image = Properties.Resources.backcard;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
            }




        }
        public void MostrarCartas(List<int> idsCartas, List<PictureBox> pictureBoxes)
        {

            string strConexao = "server=localhost;uid=root;pwd=;database=cardwar;";

            using (var conexao = new MySqlConnection(strConexao))
            {
                conexao.Open();

                for (int i = 0; i < idsCartas.Count; i++)
                {
                    string query = "SELECT nome, foto FROM carta WHERE idcarta = @Id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.AddWithValue("@Id", idsCartas[i]);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nome = reader.GetString("nome");
                                byte[] dadosImagem = (byte[])reader["foto"];

                                using (MemoryStream ms = new MemoryStream(dadosImagem))
                                {
                                    pictureBoxes[i].Image = Image.FromStream(ms);
                                    pictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;

                                    // Agora o Tag armazena ID + Nome
                                    pictureBoxes[i].Tag = new Carta
                                    {
                                        Id = idsCartas[i],
                                        Nome = nome,
                                        Imagem = pictureBoxes[i].Image
                                    };
                                    pictureBoxes[i].Click -= pictureBox_Click;
                                    pictureBoxes[i].Click += pictureBox_Click;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null && pb.Tag != null)
            {
                // Converte o Tag de volta para a classe Carta
                Carta cartaSelecionada = (Carta)pb.Tag;

                // Agora pega o ID normalmente
                cartaSelecionadaId = cartaSelecionada.Id;

                MessageBox.Show($"Você escolheu a carta: {cartaSelecionada.Nome} (ID: {cartaSelecionadaId})");
            }
        }
        private int cartaSelecionadaId;
        private Carta CarregarCarta(int idCarta)
        {
            Carta carta = null;
            string strConexao = "server=localhost;uid=root;pwd=;database=cardwar;";

            using (var conexao = new MySqlConnection(strConexao))
            {
                conexao.Open();
                string query = "SELECT idcarta, nome, saude, dano, foto FROM carta WHERE idcarta = @Id";

                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", idCarta);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            carta = new Carta
                            {
                                Id = reader.GetInt32("idcarta"),
                                Nome = reader.GetString("nome"),
                                Saude = reader.GetInt32("saude"),
                                Dano = reader.GetInt32("dano")
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("foto")))
                            {
                                byte[] dadosImagem = (byte[])reader["foto"];
                                using (MemoryStream ms = new MemoryStream(dadosImagem))
                                {
                                    carta.Imagem = Image.FromStream(ms);
                                }
                            }
                        }
                    }
                }
            }
            return carta;
        }
        private int pontos1 = 0;
        private int pontos2 = 0;
        private void ReiniciarJogo()
        {
            pictureBox12.Image = null;
            pictureBox11.Image = null;
            // Zera placar e seleção
            pontos1 = 0;
            pontos2 = 0;
            cartaSelecionadaId = 0;

            // Embaralha cartas novamente
            Random random = new Random();
            List<int> todasCartas = Enumerable.Range(1, 20).ToList();
            todasCartas = todasCartas.OrderBy(x => random.Next()).ToList();

            cartasJogador1 = todasCartas.Take(5).ToList();
            cartasJogador2 = todasCartas.Skip(5).Take(5).ToList();

            // Limpa todas as PictureBox
            foreach (var pb in listaPictureBoxJogador.Concat(listaPictureBoxOponente))
            {
                pb.Image = null;
                pb.Tag = null;
            }

            // Redistribui as cartas
            MostrarCartas(cartasJogador1, listaPictureBoxJogador);
            MostrarCartas(cartasJogador2, listaPictureBoxOponente);

        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (cartaSelecionadaId == 0)
            {
                MessageBox.Show("Selecione uma carta primeiro!");
                return;
            }

            Random rnd = new Random();

            // pega só cartas que ainda estão "na mão" do oponente
            var cartasDisponiveisOponente = cartasJogador2
                .Where(id => listaPictureBoxOponente.Any(pb =>
                {
                    var carta = pb.Tag as Carta; // recupera o objeto Carta
                    return carta != null && carta.Id == id;
                }))
                .ToList();

            if (cartasDisponiveisOponente.Count == 0)
            {
                MessageBox.Show("O oponente não tem mais cartas!");
                return;
            }

            // escolhe uma carta aleatória da mão do oponente
            int cartaOponenteId = cartasDisponiveisOponente[rnd.Next(cartasDisponiveisOponente.Count)];

            // carrega as cartas do banco
            Carta minhaCarta = CarregarCarta(cartaSelecionadaId);
            Carta cartaOponente = CarregarCarta(cartaOponenteId);

            // mostra as cartas no centro
            pictureBox11.Image = minhaCarta.Imagem;
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox12.Image = cartaOponente.Imagem;
            pictureBox12.SizeMode = PictureBoxSizeMode.StretchImage;

            // decide vencedor
            string resultado;
            if (minhaCarta.Dano > cartaOponente.Saude)
            {
                resultado = "Você ganhou a rodada!";
                pontos1++;
            }
            else if (cartaOponente.Dano > minhaCarta.Saude)
            {
                resultado = "O oponente ganhou a rodada!";
                pontos2++;
            }
            else
            {
                resultado = "Empate!";
            }

            MessageBox.Show(resultado + $"\nPlacar: Jogador {pontos1} x {pontos2} Oponente");

            // remove carta do jogador
            foreach (PictureBox pb in listaPictureBoxJogador)
            {
                if (pb.Tag is Carta carta && carta.Id == cartaSelecionadaId)
                {
                    pb.Image = null;
                    pb.Tag = null;
                    break;
                }
            }

            // remove a carta jogada das pictureBox do oponente
            foreach (PictureBox pb in listaPictureBoxOponente)
            {
                if (pb.Tag is Carta carta && carta.Id == cartaOponenteId)
                {
                    pb.Image = cartaOponente.Imagem;
                    pb.Tag = null;
                    break;
                }
            }

                // atualiza listas
                cartasJogador1.Remove(cartaSelecionadaId);
                cartasJogador2.Remove(cartaOponenteId);

                // reseta a escolha do jogador
                cartaSelecionadaId = 0;

                // verifica fim de jogo
                if (pontos1 == 3 || pontos2 == 3 ||
        (cartasJogador1.Count == 0 && cartasJogador2.Count == 0))
                {
                    string vencedor;
                    if (pontos1 > pontos2)
                        vencedor = "Jogador";
                    else if (pontos2 > pontos1)
                        vencedor = "Oponente";
                    else
                        vencedor = "Ninguém (empate)";

                    DialogResult resposta = MessageBox.Show(
                        $"Fim de jogo! {vencedor} venceu.\nPlacar final: {pontos1} x {pontos2}\n\nDeseja jogar novamente?",
                        "Fim de Jogo",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (resposta == DialogResult.Yes)
                    {
                        ReiniciarJogo();
                    }
                    else
                    {
                        Form2 form = new Form2();
                        this.Close(); // fecha o form / jogo
                        form.Show();
                    }
                }
            }


            private void pictureBox1_Click(object sender, EventArgs e)
            {

            }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            this.Close();
            form.Show();
        }
    }
    }

