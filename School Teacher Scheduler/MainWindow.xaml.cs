using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace School_Teacher_Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<DayOfWeek> DaysOfWeek = new();

        private List<CheckBox> CheckBoxesDaysOfWeek = new();

        private List<string> ResultDates = new();

        public MainWindow()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            //SetTheme();

            CheckBoxesDaysOfWeek = new List<CheckBox> { mon, tue, wed, thur, fri, sat };
        }

        public event Action CloseWindowEvent;

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (null != CloseWindowEvent)
            {
                CloseWindowEvent();
            }
            this.Close();
        }

        private void OnMouseLeftButtonUpDateStart(object sender, RoutedEventArgs e)
        {
            datePickerStart.IsDropDownOpen = true;
        }

        private void OnMouseLeftButtonUpDateEnd(object sender, RoutedEventArgs e)
        {
            datePickerEnd.IsDropDownOpen = true;
        }

        private void getDates_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerStart.SelectedDate is null || datePickerEnd.SelectedDate is null)
            {
                var time = CheckDatesFilled();
                MessageBox.Show($"Не указана дата {time}!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dateList.ItemsSource = null;
            ResultDates.Clear();
            copyDates.IsEnabled = false;

            var allDatesInPeriod = GetDateRange(datePickerStart.SelectedDate.Value, datePickerEnd.SelectedDate.Value).ToList();
            if (!allDatesInPeriod.Any())
            {
                return;
            }

            UpdateCheckedDaysOfWeekList();

            foreach (var date in allDatesInPeriod)
            {
                if (DaysOfWeek.Contains(date.DayOfWeek))
                {
                    ResultDates.Add(GetDateOnlyString(date));
                }
            }

            dateList.ItemsSource = ResultDates;
            copyDates.IsEnabled = true;
        }

        private string GetDateOnlyString(DateTime date)
        {
            return date.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        private void UpdateCheckedDaysOfWeekList()
        {
            DaysOfWeek.Clear();

            if (mon.IsChecked == true)
            {
                DaysOfWeek.Add(DayOfWeek.Monday);
            }

            if (tue.IsChecked == true)
            {
                DaysOfWeek.Add(DayOfWeek.Tuesday);
            }

            if (wed.IsChecked == true)
            {
                DaysOfWeek.Add(DayOfWeek.Wednesday);
            }

            if (thur.IsChecked == true)
            {
                DaysOfWeek.Add(DayOfWeek.Thursday);
            }

            if (fri.IsChecked == true)
            {
                DaysOfWeek.Add(DayOfWeek.Friday);
            }

            if (sat.IsChecked == true)
            {
                DaysOfWeek.Add(DayOfWeek.Saturday);
            }
        }

        private static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                MessageBox.Show($"Дата окончания не может опережать дату начала:\r\nДата начала: {startDate}\r\nДата окончания: {endDate}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                yield break;
            }

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }

        private void copyDates_Click(object sender, RoutedEventArgs e)
        {
            var text = string.Empty;
            foreach (var date in ResultDates)
            {
                text = $"{text}{date}\r\n";
            }
            Clipboard.SetText(text);
        }

        private string CheckDatesFilled()
        {
            return datePickerStart.SelectedDate is null
                ? "начала"
                : datePickerEnd.SelectedDate is null
                    ? "оконачания"
                    : string.Empty;
        }
    }
}