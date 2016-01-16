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
using System.Data;

namespace MakingAMillionBy30
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IO io;
        DataTable dtAch;
        List<string> datesList;
        List<string> daysToList;
        List<string> TargetAmountList;
        List<string> dailyWageBeforeTaxList;
        List<string> dailyWageAfterTaxList;
        List<string> hourlyWageBeforeTaxList;
        List<string> hourlyWageAfterTaxList;

        public MainWindow()
        {
            InitializeComponent();
            CreateDataGrid();
            CalculateExpensesFirstTime();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DateTime newAchDate;
            newAchDate = AchDatePicker.SelectedDate.Value;
            datesList.Add(newAchDate.ToString("d"));
        }

        private void AddDataToDataGrid()
        {
            CalculateDaysTo(datesList);
            CalculateTargetAmount();
            CalculateDailyEarnings();
            CalculateHourlyEarnings();

            dtAch.Rows.Clear();

            for (int i = 0; i < datesList.Count; i++)
            {
                dtAch.Rows.Add(datesList[i], daysToList[i], TargetAmountList[i], dailyWageBeforeTaxList[i], dailyWageAfterTaxList[i], hourlyWageBeforeTaxList[i], hourlyWageAfterTaxList[i]);
            }


            achDataGrid.ItemsSource = dtAch.DefaultView;

            daysToList.Clear();
            TargetAmountList.Clear();
            dailyWageBeforeTaxList.Clear();
            dailyWageAfterTaxList.Clear();
            hourlyWageAfterTaxList.Clear();
            hourlyWageBeforeTaxList.Clear();

        }

        private void CreateDataGrid()
        {
            dtAch = new DataTable();
            datesList = new List<string>();
            daysToList = new List<string>();

            io = new IO();
            datesList = io.GetDates();


            foreach (string value in CreateDataGridList())
            {
                dtAch.Columns.Add(value, typeof(string));

            }

        }

        private List<string> CreateDataGridList()
        {
            List<string> columnsList = new List<string>();

            columnsList.Add("From date");
            columnsList.Add("Days to");
            columnsList.Add("Target Amount incl. exp.");
            columnsList.Add("Daily income b. tax");
            columnsList.Add("Daily income a. tax");
            columnsList.Add("Hourly wage b. tax");
            columnsList.Add("Hourly wage a. tax");

            return columnsList;
        }

        private void CalculateDaysTo(List<string> datesList)
        {
            daysToList = new List<string>();
            DateTime targetDate = new DateTime();
            DateTime tempDate = new DateTime();
            TimeSpan difference = new TimeSpan();

            targetDate = TargetDateDatePicker.SelectedDate.Value;

            for (int i = 0; i < datesList.Count; i++)
            {
                tempDate = DateTime.Parse(datesList[i]);
                difference = tempDate - targetDate;
                daysToList.Add(difference.Days.ToString());
            }
        }

        private void CalculateTargetAmount()
        {
            try
            {
                TargetAmountList = new List<string>();
                for (int i = 0; i < datesList.Count; i++)
                {
                    string targetAmount = (((double.Parse(TargetAmountTextBox.Text) + (double.Parse(ExpensesTotalTextBlock.Text) * (double.Parse(daysToList[i]) / 30)
                          + (double.Parse(ExpensesDailyLunchTextBox.Text) * double.Parse(daysToList[i]))
                          + double.Parse(ExpensesVacationTextBox.Text))) - (double.Parse(MonthlyPaymentTextBox.Text) * (double.Parse(daysToList[i]) / 30)))).ToString();
                    TargetAmountList.Add(targetAmount);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Calculate total expenses first");
               
            }
            
        }

        private void CalculateDailyEarnings()
        {
            dailyWageBeforeTaxList = new List<string>();
            dailyWageAfterTaxList = new List<string>();

            for (int i = 0; i < datesList.Count; i++)
            {
                string temp = (double.Parse(TargetAmountList[i]) / double.Parse(daysToList[i])).ToString();
                dailyWageAfterTaxList.Add(temp);
            }

            for (int i = 0; i < datesList.Count; i++)
            {
                string temp = (double.Parse(dailyWageAfterTaxList[i]) * 1.42).ToString();
                dailyWageBeforeTaxList.Add(temp);
            }
        }

        private void CalculateHourlyEarnings()
        {
            hourlyWageAfterTaxList = new List<string>();
            hourlyWageBeforeTaxList = new List<string>();

            for (int i = 0; i < datesList.Count; i++)
            {
                string temp = (double.Parse(dailyWageAfterTaxList[i]) / 8).ToString();
                hourlyWageAfterTaxList.Add(temp);
            }

            for (int i = 0; i < datesList.Count; i++)
            {
                string temp = (double.Parse(dailyWageBeforeTaxList[i]) / 8).ToString();
                hourlyWageBeforeTaxList.Add(temp);
            }
        }

        private void Expenses(object sender, TextChangedEventArgs e)
        {
            try
            {
                ExpensesTotalTextBlock.Text = (double.Parse(ExpensesRentTextbox.Text) + double.Parse(ExpensesPhonebillTextBox.Text)
                    + double.Parse(ExpensesGymTextBox.Text) + double.Parse(ExpensesMiscTextBox.Text) + double.Parse(ExpensesDailyLunchTextBox.Text)
                    + double.Parse(ExpensesVacationTextBox.Text)).ToString();
            }
            catch (NullReferenceException)
            {
            }
            
        }

        private void CalculateMoneyToEarn(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AddDataToDataGrid();
        }

        private void CalculateExpensesFirstTime()
        {
            ExpensesTotalTextBlock.Text = (double.Parse(ExpensesRentTextbox.Text) + double.Parse(ExpensesPhonebillTextBox.Text)
                    + double.Parse(ExpensesGymTextBox.Text) + double.Parse(ExpensesMiscTextBox.Text) + double.Parse(ExpensesDailyLunchTextBox.Text)
                    + double.Parse(ExpensesVacationTextBox.Text)).ToString();
        }
    }
}
