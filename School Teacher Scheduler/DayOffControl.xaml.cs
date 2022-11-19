using DAL;
using Domain;
using Domain.Dtos;
using Domain.Extensions;
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
    /// Interaction logic for DayOffControl.xaml
    /// </summary>
    public partial class DayOffControl : UserControl
    {
        private List<DayOffDto> dayOffs = new List<DayOffDto>();
        private DatabaseContext Context;

        private DateOnly StudyYearStartDate = new();
        private DateOnly StudyYearEndDate = new();

        public DayOffControl(DatabaseContext context)
        {
            InitializeComponent();
            Context = context;
            InitializeStudyYear();

            DayOffDatesListRender();
        }

        private void InitializeStudyYear()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var firstStudyDay = new DateOnly(today.Year, 9, 1);
            var yearOfStartStudyYear = today > firstStudyDay ? today.Year : today.Year - 1;

            StudyYearStartDate = new DateOnly(yearOfStartStudyYear, 9, 1);
            StudyYearEndDate = new DateOnly(yearOfStartStudyYear + 1, 5, 31);
        }

        private void DayOffDatesListRender()
        {
            var allDaysOff = Context.DaysOff.OrderBy(d => d.Date);
            if (allDaysOff is null)
            {
                DialogWindow.Show($"В базе данных не найдено записей о праздничных-выходных днях",
                            "Внимание!",
                            MessageBoxButton.OK);
                return;
            }

            dayOffs = allDaysOff
                .Where(d => d.Date >= StudyYearStartDate)
                .Where(d => d.Date <= StudyYearEndDate)
                .Select(d => d.ToDto()).ToList();
            dayOffDatesList.ItemsSource = dayOffs.Select(d => d.DateString).ToList();
        }

        private void OnMouseLeftButtonUpNewDayOffDate(object sender, RoutedEventArgs e)
        {
            datePickerDayOff.IsDropDownOpen = true;
        }

        private void DeleteDayOffButton_Click(object sender, RoutedEventArgs e)
        {
            var dayOff = dayOffs.SingleOrDefault(d => d.DateString == dayOffDatesList.SelectedItem.ToString());
            if (dayOff is null)
            {
                DialogWindow.Show($"Не удалось найти выбраный выходной {dayOffDatesList.SelectedItem.ToString()} в базе данных",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            if (dayOff.CreatedBySystem)
            {
                DialogWindow.Show($"Невозможно удалить дату официального в РФ выходного.",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            var removingDayOff = Context.DaysOff.FirstOrDefault(d => d.Id == dayOff.Id);

            if (removingDayOff is null)
            {
                DialogWindow.Show($"Не удалось найти выбраный выходной {dayOffDatesList.SelectedItem} в базе данных",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            var res = DialogWindow.Show($"Вы действительно хотите удалить дату {dayOffDatesList.SelectedItem} из списка выходных дней текущего {StudyYearStartDate.Year}-{StudyYearEndDate.Year} учебного года?",
                            "Удаление",
                            MessageBoxButton.OKCancel);
            if (res == true)
            {
                Context.Remove(removingDayOff);
                Context.SaveChanges();

                DayOffDatesListRender();
            }
        }

        private void DayOffDatesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!deleteDayOffButton.IsEnabled)
                deleteDayOffButton.IsEnabled = true;
        }

        private void AddDayOffButton_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerDayOff.SelectedDate is null)
            {
                DialogWindow.Show($"Не выбрана дата выходного дня для добавления",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            var curDateOnly = DateOnly.FromDateTime((DateTime)datePickerDayOff.SelectedDate);
            if (curDateOnly < StudyYearStartDate || curDateOnly > StudyYearEndDate)
            {
                DialogWindow.Show($"Дата {curDateOnly} выходит за рамки текущего {StudyYearStartDate.Year}-{StudyYearEndDate.Year} учебного года",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            if (dayOffs.Any(d => d.Date == curDateOnly))
            {
                DialogWindow.Show($"Дата {curDateOnly} уже содержится в базе данных как выходной день в текущем {StudyYearStartDate.Year}-{StudyYearEndDate.Year} учебном году",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            var newDate = (DateTime)datePickerDayOff.SelectedDate;
            Context.DaysOff.Add(
                new DayOff(
                    new DayOffDto
                    {
                        Date = DateOnly.FromDateTime(newDate),
                        CreatedBySystem = false
                    }
                    )
                );
            Context.SaveChanges();

            DialogWindow.Show($"Дата {curDateOnly} была успешно добавлена в список праздничных-выходных в текущем {StudyYearStartDate.Year}-{StudyYearEndDate.Year} учебном году!",
                           "Успех",
                           MessageBoxButton.OK);

            DayOffDatesListRender();
        }
    }
}