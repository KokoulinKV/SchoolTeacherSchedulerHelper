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

        public DayOffControl(DatabaseContext context)
        {
            InitializeComponent();
            Context = context;
            DayOffDatesListRender();
        }

        private void DayOffDatesListRender()
        {
            dayOffs = Context.DaysOff.Select(d => d.ToDto())
                .ToList();
            dayOffDatesList.ItemsSource = dayOffs.Select(d => d.DateString).ToList();
        }

        private void OnMouseLeftButtonUpNewDayOffDate(object sender, RoutedEventArgs e)
        {
            datePickerDayOff.IsDropDownOpen = true;
        }

        private string GetDateOnlyString(DateOnly date)
        {
            return date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
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
                DialogWindow.Show($"Невозможно удалить выходной, заданный системой.",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            var removingDayOff = Context.DaysOff.FirstOrDefault(d => d.Id == dayOff.Id);

            if (removingDayOff is null)
            {
                DialogWindow.Show($"Не удалось найти выбраный выходной {dayOffDatesList.SelectedItem.ToString()} в базе данных",
                            "Ошибка",
                            MessageBoxButton.OK);
                return;
            }

            Context.Remove(removingDayOff);
            Context.SaveChanges();

            DayOffDatesListRender();
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
                DialogWindow.Show($"Не выбрана дата выходного",
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

            DayOffDatesListRender();
        }
    }
}