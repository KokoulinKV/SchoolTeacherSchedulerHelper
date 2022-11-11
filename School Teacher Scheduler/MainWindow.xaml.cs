using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
using Domain;

namespace School_Teacher_Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Список выбранных дней недели
        /// </summary>
        private readonly List<DayOfWeek> DaysOfWeek = new();

        /// <summary>
        /// Чекбоксы дней недели
        /// </summary>
        private List<CheckBox> CheckBoxesDaysOfWeek = new();

        /// <summary>
        /// Список полученных дат
        /// </summary>
        private List<string> ResultDates = new();

        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        public event Action? CloseWindowEvent;

        /// <summary>
        /// Контекст БД
        /// </summary>
        private DatabaseContext? Context { get; set; }

        /// <summary>
        /// Конструктор главного окна приложения
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));

            CheckBoxesDaysOfWeek = new List<CheckBox> { mon, tue, wed, thur, fri, sat };

            this.Context = new DatabaseContext();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки открытия календаря выбора даты начала
        /// </summary>
        private void onMouseLeftButtonUpDateStart(object sender, RoutedEventArgs e)
        {
            datePickerStart.IsDropDownOpen = true;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки открытия календаря выбора даты окончания
        /// </summary>
        private void onMouseLeftButtonUpDateEnd(object sender, RoutedEventArgs e)
        {
            datePickerEnd.IsDropDownOpen = true;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки получения списка дат
        /// </summary>
        private void GetDates_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerStart.SelectedDate is null || datePickerEnd.SelectedDate is null)
            {
                ShowEmptyDatesDialog();
                return;
            }

            if (DateTime.Compare((DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate) > 0)
            {
                ShowWrongPeriodDialog();
                return;
            }

            UpdateCheckedDaysOfWeekList();

            if (!DaysOfWeek.Any())
            {
                ShowEmptyDaysOfWeekDialog();
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

        /// <summary>
        /// Обработчик нажатия на клавишу копирования списка дат
        /// </summary>
        private void copyDates_Click(object sender, RoutedEventArgs e)
        {
            var text = string.Empty;
            foreach (var date in ResultDates)
            {
                text = $"{text}{date}\r\n";
            }
            Clipboard.SetText(text);
        }

        /// <summary>
        /// Метод вызывающий диалоговое окно об ошибке,
        /// в случае попытки получения списка дат при не выбранном(ых) значении границы календарного периода
        /// </summary>
        private void ShowEmptyDatesDialog()
        {
            var dateBoundaryForDialog = datePickerStart.SelectedDate is null
                ? "начала"
                : datePickerEnd.SelectedDate is null
                    ? "окончания"
                    : string.Empty;
            DialogWindow.Show($"Не указана дата {dateBoundaryForDialog}!", "Ошибка", MessageBoxButton.OK);
        }

        /// <summary>
        /// Метод вызывающий диалоговое окно об ошибке,
        /// в случае попытки получения списка дат при не выбранном(ых) днях недели
        /// </summary>
        private void ShowWrongPeriodDialog()
        {
            DialogWindow.Show($"Дата конца периода планирования не может опережать дату его начала. ", "Ошибка", MessageBoxButton.OK);
        }

        /// <summary>
        /// Метод вызывающий диалоговое окно об ошибке,
        /// в случае попытки получения списка дат при не выбранном(ых) днях недели
        /// </summary>
        private void ShowEmptyDaysOfWeekDialog()
        {
            DialogWindow.Show($"Не выбраны дни недели.", "Ошибка", MessageBoxButton.OK);
        }

        /// <summary>
        /// Метод получения даты в виде строки в формате dd.MM.yyyy
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Дата в строковом виде</returns>
        private string GetDateOnlyString(DateTime date)
        {
            return date.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Обновление списка выбранных дней недели
        /// </summary>
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

        /// <summary>
        /// Метод зактрытия окна
        /// </summary>
        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (CloseWindowEvent != null)
            {
                CloseWindowEvent();
            }
            this.Close();
        }

        /// <summary>
        /// Генератор списка дат в заданном календарном промежутке
        /// </summary>
        /// <param name="startDate">Дата начала интервала</param>
        /// <param name="endDate">Дата окончания интервала</param>
        /// <returns>Список дат</returns>
        private static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                DialogWindow.Show($"Дата окончания не может опережать дату начала!\r\n" +
                            $"Дата начала: {DateOnly.FromDateTime(startDate)}\r\n" +
                            $"Дата окончания: {DateOnly.FromDateTime(endDate)}",
                            "Ошибка",
                            MessageBoxButton.OK);
                yield break;
            }

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
    }
}