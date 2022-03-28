using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;


namespace SQliteJPG
{
    public partial class Form1 : Form
    {
        string pathname = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, "test.db");

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            try
            {
                // データベースと接続する
                con.Open();

                // SQLコマンドを宣言する
                SQLiteCommand cmd = con.CreateCommand();

                // テーブルを作成する
                string sql = "";
                sql += "CREATE TABLE IF NOT EXISTS sample ";
                sql += "( ";
                sql += "  no INTEGER PRIMARY KEY AUTOINCREMENT, ";
                sql += "  title TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する
                byte[] file_binary_from = File.ReadAllBytes(pathname);

                // データを挿入する
                sql = $" INSERT INTO sample(no,title,file_binary) VALUES (NULL,@name, @file_binary)";
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SQLiteParameter("@name", textBox2.Text));
                cmd.Parameters.Add("@file_binary", DbType.Binary).Value = file_binary_from;
                cmd.ExecuteNonQuery();

                // データを取得する
                sql = $" SELECT * FROM sample ";
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    // BLOBのファイルをバイト配列に変換する
                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                }


            }
            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "JPEGファイル|*.jpg";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                pathname = openFileDialog1.FileName;
                Bitmap image = new Bitmap(pathname);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = image;
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();


            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, "test.db");

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname= date.ToString("yyyyMMddHHmmss");
            try
            {
                // データベースと接続する
                con.Open();

                // SQLコマンドを宣言する
                SQLiteCommand cmd = con.CreateCommand();

                // テーブルを作成する
                string sql = "";
                sql += "CREATE TABLE IF NOT EXISTS sample ";
                sql += "( ";
                sql += "  no INTEGER NOT NULL, ";
                sql += "  title TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                int number = int.Parse(textBox3.Text);
                // データを取得する
                sql = $" SELECT * FROM sample WHERE no = " + number;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                while (reader.Read() == true)
                {
                    count++;
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    // BLOBのファイルをバイト配列に変換する
                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\temp\"+ jpgname + @".jpg", file_binary_to);
                }
                if (count == 0)
                {

                    MessageBox.Show("検索NOが存在しません");
                    return;

                }


            }
            catch (Exception)
            {
                MessageBox.Show("検索キーが未入力です");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            Bitmap image = new Bitmap(@"C:\temp\" + jpgname + @".jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = image;
            con.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();


            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, "test.db");

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");

            try
            {
                // データベースと接続する
                con.Open();

                // SQLコマンドを宣言する
                SQLiteCommand cmd = con.CreateCommand();

                // テーブルを作成する
                string sql = "";
                sql += "CREATE TABLE IF NOT EXISTS sample ";
                sql += "( ";
                sql += "  no INTEGER NOT NULL, ";
                sql += "  title TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                // データを取得する
                sql = $" SELECT * FROM sample";
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                while (reader.Read() == true)
                {
                    count++;
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    // BLOBのファイルをバイト配列に変換する
                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\temp\" + jpgname + @".jpg", file_binary_to);
                }
                if (count == 0)
                {

                    MessageBox.Show("DBが空です");
                    return;

                }

            }
            catch (Exception)
            {
                MessageBox.Show("DBが空です");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            Bitmap image = new Bitmap(@"C:\temp\" + jpgname + @".jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = image;
            con.Close();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Clear();


            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, "test.db");

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");

            try
            {
                // データベースと接続する
                con.Open();

                // SQLコマンドを宣言する
                SQLiteCommand cmd = con.CreateCommand();

                // テーブルを作成する
                string sql = "";
                sql += "CREATE TABLE IF NOT EXISTS sample ";
                sql += "( ";
                sql += "  no INTEGER NOT NULL, ";
                sql += "  title TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                int number = int.Parse(textBox3.Text);
                // データを取得する

                sql = $" DELETE FROM sample WHERE no = " + number;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();


            }
            catch (Exception)
            {
                MessageBox.Show("検索キーが未入力です");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();

        }
    }
}
