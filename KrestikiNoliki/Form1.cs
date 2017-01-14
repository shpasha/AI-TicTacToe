using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KrestikiNoliki
{
    public partial class Form1 : Form
    {
        static int num = 9;
        double[,] map = new double[3,3];
        PictureBox[,] mapp = new PictureBox[3, 3];
        char turn = 'X';
        int[] history = new int[18];
        double[] res = new double[num];
        Net net;

        public double Func(double res)
        {
            return 1 / (1 + Math.Exp(-res));
        }

        public double Func1(double res)
        {
            return Math.Exp(res) / Math.Pow((Math.Exp(res) + 1), 2);
        }

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    map[i, j] = 0;
                }
            for (int i = 0; i < 9; i++)
            {
                mapp[i / 3, i % 3] = Controls["pictureBox" + (i + 1)] as PictureBox;
        }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void reset()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    map[i, j] = 0;
                    mapp[i, j].Image = Properties.Resources._0;
                    turn = 'X';
                }

            
                
        }
        private void Move(int y, int x)
        {
            if (turn == 'X')
            {
                mapp[y, x].Image = Properties.Resources.X;
                map[y, x] = 1;
                turn = 'O';
            }
            else
            {
                mapp[y, x].Image = Properties.Resources.O;
                map[y, x] = 0.5;
                turn = 'X';
            }
        }

        private void playerMove(object sender)
        {
            int x = Int32.Parse((sender as PictureBox).Tag.ToString()) % 3;
            int y = Int32.Parse((sender as PictureBox).Tag.ToString()) / 3;
            Move(y, x);
        }

        int AIres = 0;
        private void AiMove()
        {
            AIres = 0;
            double[] input = new double[9];
            for (int i = 0; i < 9; i++)
            {
                input[i] = map[i / 3, i % 3];
            }

            double[] output = net.getOutput(input);
            double max = -1000;

            for (int i = 0; i < 9; i++)
            {
               // listBox1.Items.Add(output[i].ToString());
                if (output[i] > max)
                {
                    max = output[i];
                    AIres = i;
                }
            }
           // listBox1.Items.Add("_____________");
        }

        private char checkWinner()
        {
            bool b = false;

            if (map[0, 0] == map[0, 1] && map[0, 1] == map[0, 2] && map[0, 0] != 0 ||
                map[1, 0] == map[1, 1] && map[1, 1] == map[1, 2] && map[1, 0] != 0 ||
                map[2, 0] == map[2, 1] && map[2, 1] == map[2, 2] && map[2, 0] != 0 ||

                map[0, 0] == map[1, 0] && map[1, 0] == map[2, 0] && map[0, 0] != 0 ||
                map[0, 1] == map[1, 1] && map[1, 1] == map[2, 1] && map[0, 1] != 0 ||
                map[0, 2] == map[1, 2] && map[1, 2] == map[2, 2] && map[0, 2] != 0 ||

                map[0, 0] == map[1, 1] && map[1, 1] == map[2, 2] && map[0, 0] != 0 ||
                map[0, 2] == map[1, 1] && map[1, 1] == map[2, 0] && map[0, 2] != 0)
                b = true;

            if (b)  {
                if (turn == 'X') return 'O'; else return 'X';
            }
            b = true;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (map[i, j] == 0) b = false;
            if (b) return 'N';
            return '0';
        }

        
        
        private void save() {
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < net.layers[j].Count; i++)
                {
                    using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter((50*j+i).ToString() + ".txt"))
                    {
                        for (int k = 0; k < net.layers[j][0].getWeightsCount(); k++)
                        {
                            file.Write(net.layers[j][i].getWeightsI(k).ToString() + " ");
                        }
                    }
                }
            }
        }
        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            playerMove(sender);

            if (checkWinner() != '0')
            {
                MessageBox.Show(checkWinner().ToString());
                reset();
                return;
            }
            AiMove();
            Move(AIres / 3, AIres % 3);
           
            if (checkWinner() != '0')
            {
                MessageBox.Show(checkWinner().ToString());
                reset();
                return;
            }

            //save();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (timer1.Enabled)
            {
                button3.Text = "Прекратить обучение";
            }
            else {
                button3.Text = "Начать обучение";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int n = 2;
            net = new Net(n);
            string[] lines;
            double[] weights;
            int num1 = 0;
            for (int k = 0; k < n; k++)
            {
                if (k == 0) num = 50; else num = 9;
                if (k == 0) num1 = 9; else num1 = 50;
                for (int i = 0; i < num; i++)
                {
                    weights = new double[num1];
                  //  using (System.IO.StreamReader file =
                   //         new System.IO.StreamReader((num*k+i).ToString() + ".txt"))
                  //  {
                      //  lines = file.ReadLine().Split(' ');
                        for (int j = 0; j < num1; j++)
                        {
                        double d = r.NextDouble();
               
                            weights[j] = d/3;
                        }
                   // }
                    net.pushNeuron(k, num1, weights);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int c = 0, c1 = 0, k = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            int x = 0, y = 0;

            do
            {
                x = (r.Next() % 3);
                y = (r.Next() % 3);
            } while (map[y, x] != 0);
            
            Move(y, x);
            /*history[k] = 3 * y + x;
            k++;
            if (checkWinner() != '0')
            {
                if (checkWinner() == 'O')
                {
                    reset();
                    train();
                }
                reset();
                k = 0;
                return;
            }

            do
            {
                x = (r.Next() % 3);
                y = (r.Next() % 3);
            } while (map[y, x] != 0);

            history[k] = 3 * y + x;
            Move(y, x);
            k++;

            if (checkWinner() != '0')
            {
                if (checkWinner() == 'O')
                {
                    reset();
                    train();
                }
                reset();
                k = 0;
                return;
            }

            
            */
            
            AiMove();

            if (map[AIres/3,AIres%3] != 0)
            {
                if (checkWinner() != '0')
                {
                    reset();
                }
                else
                {
                    train();
                    c1++;
                }
                label1.Text = (c).ToString();
                
                reset();
                
            } else Move(AIres / 3, AIres % 3);
            c++;
            


            if (c == 100)
            {
                label2.Text = (c1).ToString();
                c1 = 0;
                c = 0;
            }
        }

        private void train()
        {
           // System.Diagnostics.Debugger.Break();
            double[] trueRes = new double[9];
            
            for (int i = 0; i < 9; i++)
            {
                if (i == AIres)
                {
                    trueRes[i] = 0;
                }
                else
                {
                    trueRes[i] = 1;
                }
            }
            net.train(trueRes, Convert.ToDouble(textBox1.Text));
            save();
             /*
            for (int i = 1; i <= k; i+=2)
            {
                Move(history[i - 1] / 3, history[i - 1] % 3);
                AiMove();
               // listBox1.Items.Add(AIres.ToString() + " " + history[i].ToString());
              //  listBox1.Items.Add("");
                if (AIres != history[i])
                {

                    for (int j = 0; j < 9; j++)
                    {
                        if (j == history[i]) trueRes[j] = 1; else trueRes[j] = 0;
                   //     listBox1.Items[listBox1.Items.Count - 1] += trueRes[j].ToString();
                    }
                    net.train(trueRes, Convert.ToDouble(textBox1.Text));
                    save();
                    break; 
                } else Move(AIres / 3, AIres % 3);

            }
 
            for (int i = 0; i < 18; i++)
               history[i] = 0;*/
        }



        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            
        }
    }
}
