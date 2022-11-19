using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DAL;
using Domain.Dtos;
using Domain.Factories;
using Microsoft.EntityFrameworkCore;

namespace School_Teacher_Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        public event Action? CloseWindowEvent;

        /// <summary>
        /// Контекст БД
        /// </summary>
        private DatabaseContext? Context { get; set; }

        private TabItem _tabUserPage;

        /// <summary>
        /// Конструктор главного окна приложения
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));

            this.Context = new DatabaseContext();
            this.Context.Database.Migrate();
            this.CheckAndUpdateSystemDaysOff();

            this.MainControlRender();
        }

        private void CheckAndUpdateSystemDaysOff()
        {
            var daysOffInDataBase = Context.DaysOff.ToList();
            var daysOffInFactory = DayOffFactory.GenerateDaysOffEntities();

            foreach (var dayOff in daysOffInFactory)
            {
                if (!daysOffInDataBase.Any(d => d.Date == dayOff.Date))
                {
                    Context.DaysOff.Add(dayOff);
                }
            }

            Context.SaveChanges();
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

        private void ShowMainControl(object sender, RoutedEventArgs e)
        {
            MainControlRender();
        }

        private void ShowDayOffControl(object sender, RoutedEventArgs e)
        {
            DayOffControlRender();
        }

        private void MainControlRender()
        {
            if (MainTab.Items.Count > 0 && MainTab.SelectedContent.GetType().FullName == typeof(MainControl).ToString())
            {
                return;
            }

            MainTab.Items.Clear();
            _tabUserPage = new TabItem { Content = new MainControl(Context) };
            MainTab.Items.Add(_tabUserPage);
            MainTab.Items.Refresh();
        }

        private void DayOffControlRender()
        {
            if (MainTab.Items.Count > 0 && MainTab.SelectedContent.GetType().FullName == typeof(DayOffControl).ToString())
            {
                return;
            }

            MainTab.Items.Clear();
            _tabUserPage = new TabItem { Content = new DayOffControl(Context) };
            MainTab.Items.Add(_tabUserPage);
            MainTab.Items.Refresh();
        }
    }
}