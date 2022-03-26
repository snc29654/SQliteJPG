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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pathname="";

            openFileDialog1.Filter = "JPEGファイル|*.jpg";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                pathname= openFileDialog1.FileName;
            }




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
                sql += "  no INTEGER NOT NULL, ";
                sql += "  title TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                string name = textBox2.Text;
                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する
                byte[] file_binary_from = File.ReadAllBytes(pathname);

                // データを挿入する
                sql = $" INSERT INTO sample(no,title,file_binary) VALUES (1,'写真', @file_binary)";
                cmd.CommandText = sql;
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
                    File.WriteAllBytes(@"C:\temp\image2.jpg", file_binary_to);
                }


            }
            finally
            {
                // データベースを切断する
                con.Close();
            }
            Bitmap image = new Bitmap(@"C:\temp\image2.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = image;
            con.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {


        }
    }
}
