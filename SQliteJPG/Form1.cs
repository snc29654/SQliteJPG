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
using System.Threading;
using OpenCvSharp;
using System.Diagnostics;

namespace SQliteJPG
{
    public partial class Form1 : Form
    {
        string pathname = "";
        string dbname = "";
        int[] no_data = new int[10];

        int samnale_slide_stop = 0;


        public Form1()
        {
            InitializeComponent();
            makedir();
            ReadAll();
            button18.Hide();
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1_MouseDown(pictureBox1, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));

            pictureBox1.MouseUp += pictureBox1_MouseUp;
            pictureBox1_MouseUp(pictureBox1, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));


        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();




            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

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
                sql += "  filename TEXT, ";
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
                sql = $" INSERT INTO sample(no,title,filename,file_binary) VALUES (NULL,@name, @filename,@file_binary)";
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SQLiteParameter("@name", textBox2.Text));
                string filePath = Path.GetFileName(pathname);

                cmd.Parameters.Add(new SQLiteParameter("@filename", filePath));
                cmd.Parameters.Add("@file_binary", DbType.Binary).Value = file_binary_from;
                cmd.ExecuteNonQuery();

                // データを取得する
                /*
                                sql = $" SELECT * FROM sample ";
                                cmd.CommandText = sql;
                                SQLiteDataReader reader = cmd.ExecuteReader();
                                while (reader.Read() == true)
                                {
                                    textBox1.Text += reader["no"].ToString();
                                    textBox1.Text += " : ";
                                    textBox1.Text += reader["title"].ToString();
                                    textBox1.Text += " : ";
                                    textBox1.Text += reader["filename"].ToString();
                                    textBox1.Text += " : ";
                                    textBox1.Text += "\r\n";
                                    // BLOBのファイルをバイト配列に変換する
                                    byte[] file_binary_to = (byte[])reader["file_binary"];

                                    // ファイルに書き出す
                                }
                */


            }
            catch (Exception)
            {
                MessageBox.Show("ファイル読み出ししてください");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();
            ReadAll();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            pictureBox1.Show();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox9.Hide();
            pictureBox10.Hide();
            pictureBox11.Hide();

            openFileDialog1.Filter = "JPEGファイル|*.jpg";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                pathname = openFileDialog1.FileName;
                Bitmap image = new Bitmap(pathname);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = image;
                string filePath = Path.GetFileName(pathname);
                textBox4.Text = filePath;
                SrchJPG();
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Show();
            button18.Show();
            set_no();
            textBox1.Clear();
            //画像のクリア



            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox9.Hide();
            pictureBox10.Hide();
            pictureBox11.Hide();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

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

                int number = int.Parse(textBox3.Text);
                // データを取得する
                sql = $" SELECT * FROM sample WHERE no = " + number;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                textBox2.Clear();
                while (reader.Read() == true)
                {
                    count++;
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();

                    textBox2.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    // BLOBのファイルをバイト配列に変換する
                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\jpgtemp\" + jpgname + @".jpg", file_binary_to);
                }
                if (count == 0)
                {

                    MessageBox.Show("行番をクリックしてください");
                    return;

                }


            }
            catch (Exception)
            {
                MessageBox.Show("行番をクリックしてください");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }

            System.IO.FileStream fs;
            fs = new System.IO.FileStream(@"C:\jpgtemp\" + jpgname + @".jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = System.Drawing.Image.FromStream(fs);
            fs.Close();


            con.Close();

        }

        private void button3_Click_another(object sender, EventArgs e)
        {
            pictureBox1.Show();
            button18.Show();

            //button6.Hide();
            //button7.Hide();

            textBox1.Clear();
            //画像のクリア



            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox9.Hide();
            pictureBox10.Hide();
            pictureBox11.Hide();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            //string jpgname = date.ToString("yyyyMMddHHmmss");
            string jpgname = "jpgtemp";
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

                int number = int.Parse(textBox3.Text);
                // データを取得する
                sql = $" SELECT * FROM sample WHERE no = " + number;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                textBox2.Clear();
                while (reader.Read() == true)
                {
                    count++;
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();

                    textBox2.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    // BLOBのファイルをバイト配列に変換する
                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\jpgtemp\" + jpgname + @".jpg", file_binary_to);
                }
                if (count == 0)
                {

                    MessageBox.Show("行番をクリックしてください");
                    return;

                }


            }
            catch (Exception)
            {
                MessageBox.Show("行番をクリックしてください");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }

            System.IO.FileStream fs;
            fs = new System.IO.FileStream(@"C:\jpgtemp\" + jpgname + @".jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = System.Drawing.Image.FromStream(fs);
            fs.Close();


            con.Close();

        }




        private void button4_Click(object sender, EventArgs e)
        {
            textBox8.Text = "password";
            button18.Hide();
            //button6.Show();
            //button7.Show();
            pictureBox1.Hide();
            pictureBox2.Show();
            pictureBox3.Show();
            pictureBox4.Show();
            pictureBox5.Show();
            pictureBox6.Show();
            pictureBox7.Show();
            pictureBox8.Show();
            pictureBox9.Show();
            pictureBox10.Show();
            pictureBox11.Show();


            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;
            pictureBox11.Image = null;

            ReadAll();


        }

        private int button4_Click_no()
        {
            textBox8.Text = "password";
            button18.Hide();
            //button6.Show();
            //button7.Show();
            pictureBox1.Hide();
            pictureBox2.Show();
            pictureBox3.Show();
            pictureBox4.Show();
            pictureBox5.Show();
            pictureBox6.Show();
            pictureBox7.Show();
            pictureBox8.Show();
            pictureBox9.Show();
            pictureBox10.Show();
            pictureBox11.Show();


            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;
            pictureBox11.Image = null;

            return ReadAll_no();


        }


        private void makedir()
        {
            string path = @"C:\jpgtemp";

            if (Directory.Exists(path))
            {
            }
            else
            {
                Directory.CreateDirectory(path);
                MessageBox.Show("C:jpgtempを作成しました");
            }

            path = @"C:\html_link";

            if (Directory.Exists(path))
            {
            }
            else
            {
                Directory.CreateDirectory(path);
                MessageBox.Show("C:html_linkを作成しました");
            }

        }

        private void Writehtml()
        {
            //ファイル名
            var fileName = @"C:\html_link\test.html";


            //書き込むテキスト
            var days = new string[] {
                "<!DOCTYPE html>\n",
                "<html>\n",
                "<head>\n",
                "<title> 画像表示 </title>\n",
                "</head>\n",
                "<body>\n",
                "<h1>画像表示</h1>\n",
                "<h2>jpg0</h2>\n",
                "<img src='C:/jpgtemp/0.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg1</h2>\n",
                "<img src='C:/jpgtemp/1.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg2</h2>\n",
                "<img src='C:/jpgtemp/2.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg3</h2>\n",
                "<img src='C:/jpgtemp/3.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg4</h2>\n",
                "<img src='C:/jpgtemp/4.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg5</h2>\n",
                "<img src='C:/jpgtemp/5.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg6</h2>\n",
                "<img src='C:/jpgtemp/6.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg7</h2>\n",
                "<img src='C:/jpgtemp/7.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg8</h2>\n",
                "<img src='C:/jpgtemp/8.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "<h2>jpg9</h2>\n",
                "<img src='C:/jpgtemp/9.jpg' width="+textBox15.Text+" height="+textBox16.Text+">\n",
                "</body>\n",
                "</html>\n"
            };

            try
            {
                //ファイルをオープンする
                using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("utf-8")))
                {
                    foreach (var day in days)
                    {
                        //テキストを書き込む
                        sw.Write(day);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Writehtml2()
        {
            //ファイル名
            var fileName = @"C:\html_link\test2.html";


            //書き込むテキスト
            var days = new string[] {
"<!DOCTYPE html>",
"<html>",
"<head>",
"<title> 画像表示 </title>",
"<script language=\"JavaScript\">",
    "<!--\n",
    "i = 0;\n",
    "url = \"C:/jpgtemp/\";\n",
    "img = new Array(\n",
        "\"0.jpg\",\n",
        "\"1.jpg\",\n",
        "\"2.jpg\",\n",
        "\"3.jpg\",\n",
        "\"4.jpg\",\n",
        "\"5.jpg\",\n",
        "\"6.jpg\",\n",
        "\"7.jpg\",\n",
        "\"8.jpg\",\n",
        "\"9.jpg\"\n",
    ");\n",
    "function change(){\n",
        "i++;\n",
        "if(i >= img.length) {\n",
            "i = 0;\n",
        "}\n",
        "document.body.background = url + img[i];\n",
    "}\n",
    "function tm(){\n",
        "document.body.background = url + img[i];\n",
        "tm = setInterval(\"change()\",1000);\n",
    "}\n",
    "//-->\n",
    "</script>\n",
"</head>\n",
"<body onLoad=\"tm()\">\n",
"<h1>画像表示</h1>\n",
"</body>\n",
"</html>\n",
            };

            try
            {
                //ファイルをオープンする
                using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("utf-8")))
                {
                    foreach (var day in days)
                    {
                        //テキストを書き込む
                        sw.Write(day);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ReadAll()
        {
            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");
            dataGridView1.ColumnCount = 4;
            try
            {

                dataGridView1.Columns[0].HeaderText = "行番";
                dataGridView1.Columns[1].HeaderText = "No";
                dataGridView1.Columns[2].HeaderText = "写真説明";
                dataGridView1.Columns[3].HeaderText = "ファイル名";

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
                sql += "  filename TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                string from_no = textBox5.Text;
                string to_no = textBox6.Text;

                // データを取得する
                sql = $" SELECT * FROM sample WHERE no BETWEEN " + from_no + $" AND " + to_no;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                int row = 0;
                while (reader.Read() == true)
                {

                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\jpgtemp\" + count.ToString() + @".jpg", file_binary_to);

                    System.IO.FileStream fs;
                    fs = new System.IO.FileStream(@"C:\jpgtemp\" + count.ToString() + @".jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);

                    //DataGridViewImageColumnの作成
                    DataGridViewImageColumn column = new DataGridViewImageColumn();
                    //列の名前を設定
                    column.Name = "Image" + count.ToString();
                    //Icon型ではなく、Image型のデータを表示する
                    //デフォルトでFalseなので、変更する必要はない
                    column.ValuesAreIcons = false;
                    //値の設定されていないセルに表示するイメージを設定する
                    column.Image = new Bitmap(System.Drawing.Image.FromStream(fs));
                    //イメージを縦横の比率を維持して拡大、縮小表示する
                    column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    //イメージの説明
                    //セルをクリップボードにコピーした時に使用される
                    column.Description = "イメージ";



                    Bitmap image = new Bitmap(System.Drawing.Image.FromStream(fs));


                    switch (row)
                    {
                        case 0:
                            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox2.Image = image;
                            break;
                        case 1:
                            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox3.Image = image;
                            break;
                        case 2:
                            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox4.Image = image;
                            break;
                        case 3:
                            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox5.Image = image;
                            break;
                        case 4:
                            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox6.Image = image;
                            break;
                        case 5:
                            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox7.Image = image;
                            break;
                        case 6:
                            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox8.Image = image;
                            break;
                        case 7:
                            pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox9.Image = image;
                            break;
                        case 8:
                            pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox10.Image = image;
                            break;
                        case 9:
                            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox11.Image = image;
                            break;
                    }



                    dataGridView1.Rows.Add(row, reader["no"].ToString(), reader["title"].ToString(), reader["filename"].ToString());
                    dataGridView1.Columns.Add(column);
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 40;
                    dataGridView1.Columns[2].Width = 100;
                    dataGridView1.Columns[3].Width = 200;
                    string s = reader["no"].ToString();

                    no_data[row] = int.Parse(s);


                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["filename"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";

                    fs.Close();



                    count++;
                    row++;
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
            con.Close();


        }

        private int ReadAll_no()
        {
            textBox1.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

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
                sql += "  no INTEGER PRIMARY KEY AUTOINCREMENT, ";
                sql += "  title TEXT, ";
                sql += "  filename TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                string from_no = textBox5.Text;
                string to_no = textBox6.Text;

                // データを取得する
                sql = $" SELECT * FROM sample WHERE no BETWEEN " + from_no + $" AND " + to_no;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                int row = 0;
                while (reader.Read() == true)
                {

                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\jpgtemp\" + count.ToString() + @".jpg", file_binary_to);

                    System.IO.FileStream fs;
                    fs = new System.IO.FileStream(@"C:\jpgtemp\" + count.ToString() + @".jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);


                    Bitmap image = new Bitmap(System.Drawing.Image.FromStream(fs));


                    switch (row)
                    {
                        case 0:
                            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox2.Image = image;
                            break;
                        case 1:
                            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox3.Image = image;
                            break;
                        case 2:
                            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox4.Image = image;
                            break;
                        case 3:
                            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox5.Image = image;
                            break;
                        case 4:
                            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox6.Image = image;
                            break;
                        case 5:
                            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox7.Image = image;
                            break;
                        case 6:
                            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox8.Image = image;
                            break;
                        case 7:
                            pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox9.Image = image;
                            break;
                        case 8:
                            pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox10.Image = image;
                            break;
                        case 9:
                            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox11.Image = image;
                            break;
                    }



                    string s = reader["no"].ToString();

                    no_data[row] = int.Parse(s);


                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["filename"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";

                    fs.Close();



                    count++;
                    row++;
                }


                if (count == 0)
                {

                    MessageBox.Show("最後まで読み出しました");
                    return 1;

                }

            }
            catch (Exception)
            {
                MessageBox.Show("最後まで読み出しました");
                return 1;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();

            return 0;

        }


        private void ReadLAST()
        {
            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");
            dataGridView1.ColumnCount = 4;
            try
            {

                dataGridView1.Columns[0].HeaderText = "行番";
                dataGridView1.Columns[1].HeaderText = "No";
                dataGridView1.Columns[2].HeaderText = "写真説明";
                dataGridView1.Columns[3].HeaderText = "ファイル名";

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
                sql += "  filename TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                string from_no = textBox5.Text;
                string to_no = textBox6.Text;

                // データを取得する
                sql = $"select * from sample where no in ( select max( no ) from sample )";
                //sql = $" SELECT * FROM sample WHERE no BETWEEN "+ from_no  + $" AND "+ to_no;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                int row = 0;
                while (reader.Read() == true)
                {

                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\jpgtemp\" + count.ToString() + @".jpg", file_binary_to);

                    System.IO.FileStream fs;
                    fs = new System.IO.FileStream(@"C:\jpgtemp\" + count.ToString() + @".jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);

                    //DataGridViewImageColumnの作成
                    DataGridViewImageColumn column = new DataGridViewImageColumn();
                    //列の名前を設定
                    column.Name = "Image" + count.ToString();
                    //Icon型ではなく、Image型のデータを表示する
                    //デフォルトでFalseなので、変更する必要はない
                    column.ValuesAreIcons = false;
                    //値の設定されていないセルに表示するイメージを設定する
                    column.Image = new Bitmap(System.Drawing.Image.FromStream(fs));
                    //イメージを縦横の比率を維持して拡大、縮小表示する
                    column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    //イメージの説明
                    //セルをクリップボードにコピーした時に使用される
                    column.Description = "イメージ";



                    Bitmap image = new Bitmap(System.Drawing.Image.FromStream(fs));


                    switch (row)
                    {
                        case 0:
                            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox2.Image = image;
                            break;
                        case 1:
                            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox3.Image = image;
                            break;
                        case 2:
                            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox4.Image = image;
                            break;
                        case 3:
                            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox5.Image = image;
                            break;
                        case 4:
                            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox6.Image = image;
                            break;
                        case 5:
                            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox7.Image = image;
                            break;
                        case 6:
                            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox8.Image = image;
                            break;
                        case 7:
                            pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox9.Image = image;
                            break;
                        case 8:
                            pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox10.Image = image;
                            break;
                        case 9:
                            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox11.Image = image;
                            break;
                    }



                    dataGridView1.Rows.Add(row, reader["no"].ToString(), reader["title"].ToString(), reader["filename"].ToString());
                    dataGridView1.Columns.Add(column);
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 40;
                    dataGridView1.Columns[2].Width = 100;
                    dataGridView1.Columns[3].Width = 200;
                    string s = reader["no"].ToString();

                    no_data[row] = int.Parse(s);


                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["filename"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";

                    fs.Close();



                    count++;
                    row++;
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
            con.Close();


        }



        private void SrchAll()
        {
            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");
            dataGridView1.ColumnCount = 4;
            try
            {

                dataGridView1.Columns[0].HeaderText = "行番";
                dataGridView1.Columns[1].HeaderText = "No";
                dataGridView1.Columns[2].HeaderText = "写真説明";
                dataGridView1.Columns[3].HeaderText = "ファイル名";

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
                sql += "  filename TEXT, ";
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

                string word_per = "%" + textBox4.Text + "%";
                sql = "SELECT * FROM sample WHERE title LIKE" +
                $" '{word_per}' OR filename LIKE" +
                $" '{word_per}' ORDER BY NO ASC";

                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                int row = 0;
                while (reader.Read() == true)
                {
                    dataGridView1.Rows.Add(row, reader["no"].ToString(), reader["title"].ToString(), reader["filename"].ToString());
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 40;
                    dataGridView1.Columns[2].Width = 100;
                    dataGridView1.Columns[3].Width = 200;

                    count++;
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["filename"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    row++;
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
            con.Close();


        }

        private void SrchJPG()
        {
            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");
            dataGridView1.ColumnCount = 4;
            try
            {

                dataGridView1.Columns[0].HeaderText = "行番";
                dataGridView1.Columns[1].HeaderText = "No";
                dataGridView1.Columns[2].HeaderText = "写真説明";
                dataGridView1.Columns[3].HeaderText = "ファイル名";

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
                sql += "  filename TEXT, ";
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

                string word_per = "%" + textBox4.Text + "%";
                sql = "SELECT * FROM sample WHERE title LIKE" +
                $" '{word_per}' OR filename LIKE" +
                $" '{word_per}' ORDER BY NO ASC";

                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                int row = 0;
                while (reader.Read() == true)
                {
                    dataGridView1.Rows.Add(row, reader["no"].ToString(), reader["title"].ToString(), reader["filename"].ToString());
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 40;
                    dataGridView1.Columns[2].Width = 100;
                    dataGridView1.Columns[3].Width = 200;

                    count++;
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["filename"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    row++;
                }
                if (count == 0)
                {

                    MessageBox.Show("DB登録未です");
                    return;

                }
                else
                {

                    MessageBox.Show("DB登録済です");


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
            con.Close();


        }



        private void SrchAll_Fix()
        {
            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");
            dataGridView1.ColumnCount = 3;
            try
            {

                dataGridView1.Columns[0].HeaderText = "行番";
                dataGridView1.Columns[1].HeaderText = "No";
                dataGridView1.Columns[2].HeaderText = "写真説明";

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

                // データを取得する

                string word_per = "%" + textBox2.Text + "%";
                sql = "SELECT * FROM sample WHERE title LIKE" +
                $" '{word_per}' ORDER BY NO ASC";

                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                int row = 0;
                while (reader.Read() == true)
                {
                    dataGridView1.Rows.Add(row, reader["no"].ToString(), reader["title"].ToString());

                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 40;
                    dataGridView1.Columns[2].Width = 100;
                    dataGridView1.Columns[3].Width = 200;
                    count++;
                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";
                    row++;
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
            con.Close();


        }


        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //set_no();

            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

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

                int number = int.Parse(textBox3.Text);
                // データを取得する

                sql = $" DELETE FROM sample WHERE no = " + number;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                ReadAll();

            }
            catch (Exception)
            {
                MessageBox.Show("行番をクリックしてください");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //set_no();

            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

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

                int number = int.Parse(textBox3.Text);
                // データを取得する

                sql = $" UPDATE sample SET title = @name WHERE no = " + number; ;
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SQLiteParameter("@name", textBox2.Text));

                SQLiteDataReader reader = cmd.ExecuteReader();
                ReadAll();

            }
            catch (Exception)
            {
                MessageBox.Show("行番をクリックしてください");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();


        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string s = dataGridView1[dataGridView1.CurrentCell.ColumnIndex + 1, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                textBox3.Text = s;
            }
            catch (Exception)
            {
                MessageBox.Show("行番をクリックしてください");
            }

        }

        private void set_no()
        {
            try
            {
                string s = dataGridView1[dataGridView1.CurrentCell.ColumnIndex + 1, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                textBox3.Text = s;
            }
            catch (Exception)
            {
                MessageBox.Show("行番をクリックしてください");
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {

            SrchAll();


        }

        private void button10_Click(object sender, EventArgs e)
        {

            try
            {
                string[] picList = Directory.GetFiles(@"C:\jpgtemp", "*.jpg");
                foreach (string f in picList)
                {
                    File.Delete(f);
                }
                MessageBox.Show("jpgtemp配下のjpgを削除しました");
            }
            catch (Exception)
            {
                MessageBox.Show("削除は最初だけ可能です");
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button3_Click(sender, e);
        }

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {

        }

        string[] files;

        private void button11_Click(object sender, EventArgs e)
        {

            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox9.Hide();
            pictureBox10.Hide();
            pictureBox11.Hide();

            pictureBox1.Show();


            openFileDialog1.Filter = "JPEGファイル|*.jpg";
            DialogResult dr = openFileDialog1.ShowDialog();

            string s3 = "";
            s3 = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);



            files = System.IO.Directory.GetFiles(
            s3, "*.jpg", System.IO.SearchOption.AllDirectories);




            Thread t = new Thread(new ThreadStart(ManySet));

            t.Start();


        }

        private void ManySet()
        {
            textBox1.Clear();
            textBox2.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

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
                sql += "  filename TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                foreach (string file in files)
                {

                    try
                    {


                        byte[] file_binary_from = File.ReadAllBytes(file);

                        // データを挿入する
                        sql = $" INSERT INTO sample(no,title,filename,file_binary) VALUES (NULL,@name, @filename,@file_binary)";
                        cmd.CommandText = sql;
                        cmd.Parameters.Add(new SQLiteParameter("@name", textBox2.Text));
                        string filePath = Path.GetFileName(file);

                        cmd.Parameters.Add(new SQLiteParameter("@filename", filePath));
                        cmd.Parameters.Add("@file_binary", DbType.Binary).Value = file_binary_from;
                        cmd.ExecuteNonQuery();
                        textBox4.Text = filePath;


                        Bitmap image = new Bitmap(file);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Image = image;


                    }
                    catch (Exception)
                    {
                        //MessageBox.Show("ファイル読み出ししてください");
                        continue;
                    }


                }

            }
            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();
            //ReadAll();
            MessageBox.Show("一括登録が完了しました");

        }

        private void SlideShow()
        {
            textBox1.Clear();
            textBox2.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

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
                sql += "  filename TEXT, ";
                sql += "  file_binary BLOB ";
                sql += ") ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // データを全て削除する
                //sql = "DELETE FROM sample ";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                foreach (string file in files)
                {
                    if (samnale_slide_stop == 1)
                    {
                        samnale_slide_stop = 0;
                        return;
                    }


                    try
                    {


                        byte[] file_binary_from = File.ReadAllBytes(file);

                        // データを挿入する
                        sql = $" INSERT INTO sample(no,title,filename,file_binary) VALUES (NULL,@name, @filename,@file_binary)";
                        cmd.CommandText = sql;
                        cmd.Parameters.Add(new SQLiteParameter("@name", textBox2.Text));
                        string filePath = Path.GetFileName(file);

                        cmd.Parameters.Add(new SQLiteParameter("@filename", filePath));
                        cmd.Parameters.Add("@file_binary", DbType.Binary).Value = file_binary_from;
                        //cmd.ExecuteNonQuery();
                        textBox4.Text = filePath;


                        Bitmap image = new Bitmap(file);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Image = image;


                    }
                    catch (Exception)
                    {
                        //MessageBox.Show("ファイル読み出ししてください");
                        continue;
                    }


                }

            }
            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();
            //ReadAll();
            MessageBox.Show("プレビューが完了しました");

        }



        private void button12_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox2.Text);

        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                int sel = listBox1.SelectedIndex;
                listBox1.Items.RemoveAt(sel);
            }
            catch (Exception)
            {
                MessageBox.Show("リストを選択してください");
                return;
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {

                textBox2.Text = listBox1.SelectedItem.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("リストを選択してください");
                return;
            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {

                textBox7.Text = listBox2.SelectedItem.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("リストを選択してください");
                return;
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "DBファイル|*.db";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                dbname = openFileDialog1.FileName;
                string path1 = dbname;
                textBox7.Text = path1;
            }

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[6].ToString();

            button3_Click_another(sender, e);

        }

        private void 次の10件_Click(object sender, EventArgs e)
        {
            int num = int.Parse(textBox6.Text);

            textBox5.Text = (num + 1).ToString();
            textBox6.Text = (num + 10).ToString();
            button4_Click(sender, e);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            int num = int.Parse(textBox5.Text);

            textBox5.Text = (num - 10).ToString();
            textBox6.Text = (num - 1).ToString();
            button4_Click(sender, e);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[0].ToString();

            button3_Click_another(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[1].ToString();

            button3_Click_another(sender, e);

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[2].ToString();

            button3_Click_another(sender, e);

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[3].ToString();

            button3_Click_another(sender, e);

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[4].ToString();

            button3_Click_another(sender, e);

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[5].ToString();

            button3_Click_another(sender, e);

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[7].ToString();

            button3_Click_another(sender, e);

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[8].ToString();

            button3_Click_another(sender, e);

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            textBox3.Text = no_data[9].ToString();

            button3_Click_another(sender, e);

        }

        private void button18_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void button19_Click(object sender, EventArgs e)
        {

            if (textBox8.Text != "12345678")
            {


                MessageBox.Show("パスワードが不一致です");
                return;


            }
            textBox1.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            // EXEの起動パスを取得する
            string exePath = System.Windows.Forms.Application.StartupPath;

            // DBフルパスを組みたてる
            string dbFullPath = System.IO.Path.Combine(exePath, textBox7.Text);

            // 接続先データベースを指定する
            SQLiteConnection con = new SQLiteConnection(String.Format($"Data Source = {dbFullPath}"));

            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;

            string jpgname = date.ToString("yyyyMMddHHmmss");
            dataGridView1.ColumnCount = 3;
            try
            {

                dataGridView1.Columns[0].HeaderText = "行番";
                dataGridView1.Columns[1].HeaderText = "No";
                dataGridView1.Columns[2].HeaderText = "写真説明";

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
                sql = "DROP TABLE sample ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                // ファイルをバイト配列に変換する

                string from_no = textBox5.Text;
                string to_no = textBox6.Text;

                // データを取得する
                sql = $" SELECT * FROM sample WHERE no BETWEEN " + from_no + $" AND " + to_no;
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                int count = 0;
                int row = 0;
                while (reader.Read() == true)
                {

                    byte[] file_binary_to = (byte[])reader["file_binary"];

                    // ファイルに書き出す
                    File.WriteAllBytes(@"C:\jpgtemp\" + count.ToString() + @".jpg", file_binary_to);

                    System.IO.FileStream fs;
                    fs = new System.IO.FileStream(@"C:\jpgtemp\" + count.ToString() + @".jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);

                    //DataGridViewImageColumnの作成
                    DataGridViewImageColumn column = new DataGridViewImageColumn();
                    //列の名前を設定
                    column.Name = "Image" + count.ToString();
                    //Icon型ではなく、Image型のデータを表示する
                    //デフォルトでFalseなので、変更する必要はない
                    column.ValuesAreIcons = false;
                    //値の設定されていないセルに表示するイメージを設定する
                    column.Image = new Bitmap(System.Drawing.Image.FromStream(fs));
                    //イメージを縦横の比率を維持して拡大、縮小表示する
                    column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    //イメージの説明
                    //セルをクリップボードにコピーした時に使用される
                    column.Description = "イメージ";



                    Bitmap image = new Bitmap(System.Drawing.Image.FromStream(fs));


                    switch (row)
                    {
                        case 0:
                            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox2.Image = image;
                            break;
                        case 1:
                            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox3.Image = image;
                            break;
                        case 2:
                            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox4.Image = image;
                            break;
                        case 3:
                            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox5.Image = image;
                            break;
                        case 4:
                            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox6.Image = image;
                            break;
                        case 5:
                            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox7.Image = image;
                            break;
                        case 6:
                            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox8.Image = image;
                            break;
                        case 7:
                            pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox9.Image = image;
                            break;
                        case 8:
                            pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox10.Image = image;
                            break;
                        case 9:
                            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox11.Image = image;
                            break;
                    }



                    dataGridView1.Rows.Add(row, reader["no"].ToString(), reader["title"].ToString());
                    dataGridView1.Columns.Add(column);
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 40;
                    dataGridView1.Columns[2].Width = 100;
                    dataGridView1.Columns[3].Width = 200;

                    string s = reader["no"].ToString();

                    no_data[row] = int.Parse(s);


                    textBox1.Text += reader["no"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += reader["title"].ToString();
                    textBox1.Text += " : ";
                    textBox1.Text += "\r\n";

                    fs.Close();



                    count++;
                    row++;
                }


                if (count == 0)
                {

                    pictureBox2.Hide();
                    pictureBox3.Hide();
                    pictureBox4.Hide();
                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    pictureBox7.Hide();
                    pictureBox8.Hide();
                    pictureBox9.Hide();
                    pictureBox10.Hide();
                    pictureBox11.Hide();
                    MessageBox.Show("削除完了です");
                    return;

                }

            }
            catch (Exception)
            {
                pictureBox2.Hide();
                pictureBox3.Hide();
                pictureBox4.Hide();
                pictureBox5.Hide();
                pictureBox6.Hide();
                pictureBox7.Hide();
                pictureBox8.Hide();
                pictureBox9.Hide();
                pictureBox10.Hide();
                pictureBox11.Hide();
                MessageBox.Show("削除完了です");
                return;
            }

            finally
            {
                // データベースを切断する
                con.Close();
            }
            con.Close();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox9.Hide();
            pictureBox10.Hide();
            pictureBox11.Hide();

            MessageBox.Show("削除完了です");

        }

        private void button20_Click(object sender, EventArgs e)
        {

            listBox1.Items.Clear();
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                //ファイルパスをテキストボックスに入れる
                StreamReader ro = new StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("utf-8"));

                string text = ro.ReadToEnd();

                text = text.Replace(Environment.NewLine, "\r");
                text = text.Trim('\r');
                string[] s2 = text.Split('\r');

                foreach (string item in s2)
                {
                    listBox1.Items.Add(item);
                }

                ro.Close();

            }


        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            textBox8.Text = "password";
            button18.Hide();
            //button6.Show();
            //button7.Show();
            pictureBox1.Hide();
            pictureBox2.Show();
            pictureBox3.Show();
            pictureBox4.Show();
            pictureBox5.Show();
            pictureBox6.Show();
            pictureBox7.Show();
            pictureBox8.Show();
            pictureBox9.Show();
            pictureBox10.Show();
            pictureBox11.Show();


            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;
            pictureBox11.Image = null;

            ReadLAST();


        }

        private void button22_Click(object sender, EventArgs e)
        {
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox9.Hide();
            pictureBox10.Hide();
            pictureBox11.Hide();

            pictureBox1.Show();


            openFileDialog1.Filter = "JPEGファイル|*.jpg";
            DialogResult dr = openFileDialog1.ShowDialog();

            string s3 = "";
            s3 = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);



            files = System.IO.Directory.GetFiles(
            s3, "*.jpg", System.IO.SearchOption.AllDirectories);




            Thread t = new Thread(new ThreadStart(SlideShow));

            t.Start();

        }

        private void button23_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(next_ten));

            t.Start();

        }

        private void next_ten()
        {
            int num = int.Parse(textBox6.Text);

            textBox5.Text = (num + 1).ToString();
            textBox6.Text = (num + 10).ToString();
            if (button4_Click_no() == 1) return;
            if (samnale_slide_stop == 1)
            {
                samnale_slide_stop = 0;
                return;
            }
            next_ten();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            samnale_slide_stop = 1;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            samnale_slide_stop = 1;

        }

        private void button26_Click(object sender, EventArgs e)
        {
            Mat mat2;

            try
            {


                // 画像の読み込み
                using (Mat mat = new Mat(@"C:\jpgtemp\jpgtemp.jpg"))


                    mat2 = mat.Clone(new Rect(int.Parse(textBox9.Text), int.Parse(textBox10.Text), int.Parse(textBox11.Text), int.Parse(textBox12.Text)));
                Cv2.ImWrite(@"C:\jpgtemp\jpgtemp2.jpg", mat2);

                {
                    // 画像をウィンドウに表示
                    //Cv2.ImShow("切り出し", mat2);

                    System.IO.FileStream fs;
                    fs = new System.IO.FileStream(@"C:\jpgtemp\jpgtemp2.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Image = System.Drawing.Image.FromStream(fs);
                    fs.Close();


                }
            }
            catch (Exception)
            {
                MessageBox.Show("範囲を見直してください");
            }


        }

        private void button26_Click_L(object sender, EventArgs e)
        {
            /*
            using (Mat mat = new Mat(@"C:\jpgtemp\jpgtempL.jpg"))
            {
                // 画像をウィンドウに表示
                Cv2.ImShow("sample_show", mat);
            }
            */
            System.IO.FileStream fs;
            fs = new System.IO.FileStream(@"C:\jpgtemp\jpgtempL.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = System.Drawing.Image.FromStream(fs);
            fs.Close();



        }

        private void button26_Click_R(object sender, EventArgs e)
        {
            /*
            using (Mat mat = new Mat(@"C:\jpgtemp\jpgtempR.jpg"))
            {
                // 画像をウィンドウに表示
                Cv2.ImShow("sample_show", mat);
            }
            */
            System.IO.FileStream fs;
            fs = new System.IO.FileStream(@"C:\jpgtemp\jpgtempR.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = System.Drawing.Image.FromStream(fs);
            fs.Close();


        }



        private void button27_Click(object sender, EventArgs e)
        {
            Mat src = Cv2.ImRead(@"C:\jpgtemp\jpgtemp.jpg");

            RotateAndResize(src, out Mat dest_L, false);
            Cv2.ImWrite(@"C:\jpgtemp\jpgtempL.jpg", dest_L);

            RotateAndResize(src, out Mat dest_R, true);
            Cv2.ImWrite(@"C:\jpgtemp\jpgtempR.jpg", dest_R);


            button26_Click_R(sender, e);
        }


        public static void RotateAndResize(Mat src, out Mat dest, bool isRight/*左倒しが基本*/)
        {
            dest = new Mat();

            // 本当は大きい辺に合わせるが横長画像と仮定する
            var center = new Point2f(src.Cols / 2, src.Cols / 2);

            Mat rMat = Cv2.GetRotationMatrix2D(center, 90, 1);
            Cv2.WarpAffine(src, dest, rMat, dest.Size());

            if (isRight)
            {
                Cv2.Flip(dest, dest, FlipMode.XY);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Mat src = Cv2.ImRead(@"C:\jpgtemp\jpgtemp.jpg");

            RotateAndResize(src, out Mat dest_L, false);
            Cv2.ImWrite(@"C:\jpgtemp\jpgtempL.jpg", dest_L);

            RotateAndResize(src, out Mat dest_R, true);
            Cv2.ImWrite(@"C:\jpgtemp\jpgtempR.jpg", dest_R);


            button26_Click_L(sender, e);

        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            //MessageBox.Show("MouseDown");

            int xCoordinate = e.X * int.Parse(textBox13.Text);
            int yCoordinate = e.Y * int.Parse(textBox14.Text);

            textBox9.Text = xCoordinate.ToString();
            textBox10.Text = yCoordinate.ToString();


        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            //MessageBox.Show("MouseDown");

            int xCoordinate = e.X * int.Parse(textBox13.Text);
            int yCoordinate = e.Y * int.Parse(textBox14.Text);

            textBox11.Text = xCoordinate.ToString();
            textBox12.Text = yCoordinate.ToString();


        }

        private void button29_Click(object sender, EventArgs e)
        {
            Writehtml();

            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                                "C:\\html_link\\test.html");
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Writehtml2();

            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                                "C:\\html_link\\test2.html");

        }
    }

}
