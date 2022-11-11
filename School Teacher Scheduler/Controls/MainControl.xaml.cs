using DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace School_Teacher_Scheduler
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
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
        /// Контекст БД
        /// </summary>
        private DatabaseContext Context;

        /// <summary>
        /// Конструктор MainControl
        /// </summary>
        public MainControl(DatabaseContext context)
        {
            InitializeComponent();

            Context = context;
            CheckBoxesDaysOfWeek = new List<CheckBox> { mon, tue, wed, thur, fri, sat };
        }

        /// <summary>
        /// Обработчик события нажатия кнопки открытия календаря выбора даты начала
        /// </summary>
        private void StartDatePickerButton_Click(object sender, RoutedEventArgs e)
        {
            datePickerStart.IsDropDownOpen = true;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки открытия календаря выбора даты окончания
        /// </summary>
        private void EndDatePickerButton_Click(object sender, RoutedEventArgs e)
        {
            datePickerEnd.IsDropDownOpen = true;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки получения списка дат
        /// </summary>
        private void GetDatesButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCheckedDaysOfWeekList();

            if (!DaysOfWeek.Any())
            {
                ShowEmptyDaysOfWeekDialog();
                return;
            }

            List<DateOnly> allDatesInPeriod;

            try
            {
                allDatesInPeriod = GetDateRange();
            }
            catch
            {
                return;
            }

            dateList.ItemsSource = null;
            ResultDates.Clear();
            copyDates.IsEnabled = false;

            foreach (var date in allDatesInPeriod)
            {
                if (DaysOfWeek.Contains(date.DayOfWeek))
                {
                    ResultDates.Add(GetDateOnlyString(date));
                }
            }

            dateList.ItemsSource = ResultDates;
            copyDates.IsEnabled = ResultDates.Any();
        }

        /// <summary>
        /// Обработчик нажатия на клавишу копирования списка дат
        /// </summary>
        private void CopyDatesButton_Click(object sender, RoutedEventArgs e)
        {
            var text = string.Empty;
            foreach (var date in ResultDates)
            {
                text = $"{text}{date}\r\n";
            }
            Clipboard.SetText(text);
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
        /// Метод получения списка дат на выбранном промежутке с учетом праздничных дней
        /// </summary>
        /// <returns>Список дат на выбранном промежутке с учетом праздничных дней</returns>
        private List<DateOnly> GetDateRange()
        {
            if (datePickerStart.SelectedDate is null || datePickerEnd.SelectedDate is null)
            {
                ShowEmptyDatesDialog();
                throw new ArgumentNullException();
            }

            if (DateTime.Compare((DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate) > 0)
            {
                ShowWrongPeriodDialog();
                throw new ArgumentException();
            }

            var allDatesInPeriod = GetAllDateRange(
                DateOnly.FromDateTime(datePickerStart.SelectedDate.Value),
                DateOnly.FromDateTime(datePickerEnd.SelectedDate.Value)).ToList();

            var daysOff = Context.DaysOff.OrderBy(d => d.Date).ToList();

            foreach (var dayOff in daysOff)
            {
                var inList = allDatesInPeriod.SingleOrDefault(d => d == dayOff.Date);
                if (inList != default)
                {
                    allDatesInPeriod.Remove(inList);
                }
            }

            return allDatesInPeriod;
        }

        /// <summary>
        /// Метод получения даты в виде строки в формате dd.MM.yyyy
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Дата в строковом виде</returns>
        private string GetDateOnlyString(DateOnly date)
        {
            return date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
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
        /// в случае попытки получения списка дат при не верно выбранных границах периода планирования
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
        /// Генератор списка дат в заданном календарном промежутке
        /// </summary>
        /// <param name="startDate">Дата начала интервала</param>
        /// <param name="endDate">Дата окончания интервала</param>
        /// <returns>Список дат</returns>
        private static IEnumerable<DateOnly> GetAllDateRange(DateOnly startDate, DateOnly endDate)
        {
            if (endDate < startDate)
            {
                DialogWindow.Show($"Дата окончания не может опережать дату начала!\r\n" +
                            $"Дата начала: {startDate}\r\n" +
                            $"Дата окончания: {endDate}",
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