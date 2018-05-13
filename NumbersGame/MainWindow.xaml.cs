using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NumbersGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] table = new int[4, 4];
        private List<int> list = new List<int>();
        public Button emptyBtn;
        private int totalMoves = 0;

        public MainWindow()
        {
            InitializeComponent();
            initGrid();
        }

        public void initGrid() {

            Button dynamicButton;
            System.Random random = new System.Random();
            int number;

            for (int x = 0; x <= table.GetUpperBound(0); x++ )
            {
                for (int y = 0; y <= table.GetUpperBound(1); y++)
                {
                    while (true)
                    {
                        number = random.Next(16);
                        if (!list.Contains(number)) break; 
                    }
                    
                    list.Add(number);
                    table[x, y] = number;
                    dynamicButton = new Button();
                    Grid.SetRow(dynamicButton, x);
                    Grid.SetColumn(dynamicButton, y);

                    dynamicButton.Click += btn_Click;
                    dynamicButton.Content = number;
                    if (number == 0) 
                    {
                        dynamicButton.Content = String.Empty;
                        dynamicButton.Background = new SolidColorBrush(Colors.Aqua);
                        emptyBtn = dynamicButton;
                    }
                    gridMain.Children.Add(dynamicButton);
                }
            }
            markAllowedMoves();
        }

        protected void markAllowedMoves()
        {
            int col;
            int row;
            int emptyRow = Grid.GetRow(emptyBtn);
            int emptyCol = Grid.GetColumn(emptyBtn);

            foreach (Control ctrl in gridMain.Children)
            {
                row = Grid.GetRow(ctrl);
                col = Grid.GetColumn(ctrl);
                if (
                    (!(row - 1 < 0) || !(row + 1 > 3) || !(col - 1 < 0) || !(col + 1 > 3))
                    &&
                    (
                        (row == emptyRow + 1 && col == emptyCol) || (row == emptyRow - 1 && col == emptyCol) 
                        ||
                        (col == emptyCol + 1 && row == emptyRow) || (col == emptyCol - 1 && row == emptyRow)
                    )
                    )
                {
                    ctrl.Background = new SolidColorBrush(Colors.Aqua);
                    ctrl.BorderBrush = new SolidColorBrush(Colors.Aqua);
                    ctrl.IsEnabled = true;
                }
                else
                {
                    ctrl.Background = new SolidColorBrush(Colors.White);
                    ctrl.IsEnabled = false;
                }
            }
        }

        public void btn_Click (object sender, EventArgs e)
        {
            totalMoves++;
            int row = Grid.GetRow((sender as Button));
            int col = Grid.GetColumn((sender as Button));
            int emptyRow = Grid.GetRow(emptyBtn);
            int emptyCol = Grid.GetColumn(emptyBtn);

            Grid.SetColumn(emptyBtn, col);
            Grid.SetRow(emptyBtn, row);

            Grid.SetColumn((sender as Button), emptyCol);
            Grid.SetRow((sender as Button), emptyRow);

            table[emptyRow, emptyCol] = table[row, col];
            table[row, col] = 0;

            markAllowedMoves();
            if (checkIsFinished()) {
                showSuccessMessage();
            }
        }

        private bool checkIsFinished()
        {
            int number = 0;
            for (int x = 0; x <= table.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= table.GetUpperBound(1); y++)
                {
                    if(table[x,y] != number) {
                        return false;
                    }
                    number++;
                }
            }
            return true;
        }

        private void showSuccessMessage()
        {
            if (checkIsFinished())
            {
                foreach (Control ctrl in gridMain.Children)
                {
                    ctrl.Background = new SolidColorBrush(Colors.White);
                    ctrl.IsEnabled = false;
                }
                string line1 = "Congratulations, you finish the game with "+totalMoves+" total moves.";
                string line2 = " \n\nDo you want to play one more game?";
                MessageBoxResult rs = MessageBox.Show(line1 + line2, "Congratulations", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    gridMain.Children.Clear();
                    table = new int[4, 4];
                    list = new List<int>();
                    totalMoves = 0;
                    initGrid();
                }
            }
        }

    }
}
