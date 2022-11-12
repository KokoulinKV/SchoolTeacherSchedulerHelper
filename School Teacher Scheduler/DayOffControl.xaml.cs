using DAL;
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
        public DayOffControl(DatabaseContext context)
        {
            InitializeComponent();

            DayOffDatesListRender(context);
        }

        private void DayOffDatesListRender(DatabaseContext context)
        {
            var datesFromContext = context.DaysOff.ToList();
            dayOffDatesList.ItemsSource = datesFromContext.Select(d => GetDateOnlyString(d.Date)).ToList();
        }

        private void OnMouseLeftButtonUpNewDayOffDate(object sender, RoutedEventArgs e)
        {
            datePickerDayOff.IsDropDownOpen = true;
        }

        private string GetDateOnlyString(DateOnly date)
        {
            return date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
    }
}