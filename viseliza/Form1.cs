using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Hangman_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        StringBuilder guessedLetters = new StringBuilder(); // Чтобы изменять строку
        string word = "";
        int amount = 0;
        int guessedAmount = 0;
        List<Label> labels = new List<Label>();// новый список для всех меток (Label'ов) (подчеркивания с буквами)
        enum BodyParts // перечисление используется для хранения всех частей тела человечка-смертника
        {
            Head,
            Left_Eye,
            Right_Eye,
            Mouth,
            Right_Arm,
            Left_Arm,
            Body,
            Right_Leg,
            Left_Leg,
        }

        private void DrawGallows()
        {
            /*Следующие 5 строк - это код для 
             * рисования фонарного столба, как 
             * только программа будет запущена*/
            Graphics g = panel1.CreateGraphics();// для рисования на панели
            Pen p = new Pen(Color.Brown, 10);// для создания ручки (Pen)
            g.DrawLine(p, new Point(130, 248), new Point(130, 10));// снизу-вверх первая линия виселицы
            g.DrawLine(p, new Point(135, 5), new Point(65, 5));// справа-налево вторая линия виселицы
            g.DrawLine(p, new Point(60, 0), new Point(60, 50));// сверху-вниз третья линия виселицы
        }

        void DrawHangPost()
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Brown, 10);
            g.DrawLine(p, new Point(130, 248), new Point(130, 10));
            g.DrawLine(p, new Point(135, 5), new Point(65, 5));
            g.DrawLine(p, new Point(60, 0), new Point(60, 50));
            /**
             * Следующие восемь строк предназначены для различных частей тела, 
             * которые вызывают функцию "DrawBodypart()", чтобы рисовать эти 
             * части тела каждый раз, когда пользователь вводит неправильную букву
             */
            DrawBodyPart(BodyParts.Head);
            DrawBodyPart(BodyParts.Left_Eye);
            DrawBodyPart(BodyParts.Right_Eye);
            DrawBodyPart(BodyParts.Mouth);
            DrawBodyPart(BodyParts.Body);
            DrawBodyPart(BodyParts.Left_Arm);
            DrawBodyPart(BodyParts.Right_Arm);
            DrawBodyPart(BodyParts.Left_Leg);
            DrawBodyPart(BodyParts.Right_Leg);
            MessageBox.Show(GetRandomWord()); // появится окно сообщения со случайным словом
        }

        void DrawBodyPart(BodyParts bp)
        {
            Graphics g = panel1.CreateGraphics(); // доступ к графике для рисования различных частей тела
            Pen p = new Pen(Color.Blue, 2);
            if (bp == BodyParts.Head)// рисуем голову
            {
                g.DrawEllipse(p, 40, 50, 40, 40);
            }
            else if (bp == BodyParts.Left_Eye)// рисуем левый глаз
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 50, 60, 5, 5);

            }
            else if (bp == BodyParts.Right_Eye)// рисуем правый глаз
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 63, 60, 5, 5);
            }
            else if (bp == BodyParts.Mouth) // нарисовать рот
            {
                g.DrawArc(p, 50, 60, 20, 20, 45, 90);
            }
            else if (bp == BodyParts.Body)// рисуем тело
            {
                g.DrawLine(p, new Point(60, 90), new Point(60, 170));
            }
            else if (bp == BodyParts.Left_Arm)// рисуем левую руку
            {
                g.DrawLine(p, new Point(60, 100), new Point(30, 85));
            }
            else if (bp == BodyParts.Right_Arm)//рисуем правую руку
            {
                g.DrawLine(p, new Point(60, 100), new Point(90, 85));// первая точка - где начинаем рисовать тело
            }
            else if (bp == BodyParts.Left_Leg)// рисуем левую ногу
            {
                g.DrawLine(p, new Point(60, 170), new Point(30, 190));
            }
            else if (bp == BodyParts.Right_Leg)// рисуем правую ногу
            {
                g.DrawLine(p, new Point(60, 170), new Point(90, 190));
            }
        }

        void MakeLabels()
        {
          word = GetRandomWord();
          char[] chars = word.ToCharArray();
          int between = 330 / chars.Length - 1; // используем минус 1, потому что последний символ '\n', поэтому пропуск + between - это пробелы между символами подчеркивания
            for (int i = 0; i < chars.Length; i++) // цикл for используется для добавления всех следующих функций для каждого слова, чтобы пользователь мог воспроизводить их снова и снова
            {
                labels.Add(new Label()); // создание для каждого символа метки для его отображения
                labels[i].Location = new Point((i * between) + 10, 80); // 80, чтобы оставаться на той же оси y
                labels[i].Text = "_"; // добавляем подчёркивания
                labels[i].Parent = groupBox2; //Parent - поле, где показываются букви и Счёт
                labels[i].BringToFront(); // вывод всегда на передний фон
                labels[i].CreateControl(); // Создалась кнопка
            }
            label1.Text = "Счет Букв В Слове: " + (chars.Length).ToString(); // здесь отображается метка "Счет Букв В Слове", показывающая, сколько букв в слове
        }
        string GetRandomWord() //наугад выбирает слово из файла
        {

            FileStream fs = new FileStream("text.txt", FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);  
            string wordList = sr.ReadToEnd();
            string[] words = wordList.Split('\n');
            Random random = new Random();
            string riddle = words[random.Next(0, words.Length)].Replace("\n", "").Replace("\r", "");
            fs.Close();
            return riddle;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
        
            MessageBox.Show("ХАЧУ ПЯТЕРОЧКУ"); // приветсвие
            DrawGallows();
            MakeLabels();
            Console.ForegroundColor = ConsoleColor.Green;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Textboxenter();
        }

        public void Textboxenter()
        {

            if (textBox1.Text.Length >= 1)
            {
                char letter = textBox1.Text.ToLower().ToCharArray()[0]; // вводимые значения переводятся в символьные
                if (!char.IsLetter(letter)) // если введенная буква не является алфавитом, то появится сообщение об ошибке
                {
                    MessageBox.Show("Никаких Специальных Символов И Циферок", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (guessedLetters.ToString().Contains(letter))
                {

                    MessageBox.Show("Такая буква уже есть)0)");
                }

                else if (word.Contains(letter)) // проверка на буквы
                {

                    guessedLetters.Append(letter);
                    
                   
                    char[] letters = word.ToCharArray();
                    for (int i = 0; i < letters.Length; i++)
                    {
                       
                        if (letters[i] == letter)
                        {
                            guessedAmount++;
                            labels[i].Text = letter.ToString();
                            
                        }
                    }
                    if (guessedAmount == word.Length)
                    {
                        DialogResult dialogResult = MessageBox.Show("Конечна", "УРАРАРА!РАРР!А! Я Заслужил Автомат 5?", MessageBoxButtons.OK);
                        if (dialogResult == DialogResult.OK)
                        {
                            restart();
                        }
                    }

                }

                else
                {
                    MessageBox.Show("Буковка Неправильная");
                    /*
                     * Условие else используется, если введенная буква 
                     * не совпадает с буквой в самом слове, то появится 
                     * неправильная буква в пропущенной метке, чтобы
                     * пользователь знал, что не следует вводить эту 
                     * букву снова, и если они введут 9 неправильных 
                     * букв, то программа выполнит гг вп для висельника
                     */
                    label2.Text += " " + letter.ToString() + ",";
                    DrawBodyPart((BodyParts)amount);
                    amount++;
                    if (amount == 9)
                    {
                        DialogResult dialogResult = MessageBox.Show("Придется Поставить Автомат 5", "Как Можно Было Проиграть??", MessageBoxButtons.OK);
                        if (dialogResult == DialogResult.OK)
                        {
                            restart();
                        }
                    }
                }
            }
            textBox1.Text = "";
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        /* чтобы можно было нажимать Enter*/
        {
            if (e.KeyCode==Keys.Enter)
            {
                Textboxenter();
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void restart()
        {
            /*обнуление текущей игры, 
             * чтобы начать новую*/
            Refresh();
            guessedLetters = new StringBuilder();
            word = "";
            guessedAmount = 0;
            amount = 0;
            DrawGallows();
            labels = new List<Label>();
            label2.Text = "Счет Неверных Букв: ";
            MakeLabels();
        }
        private void button3_Click(object sender, EventArgs e)
        {

            restart();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}