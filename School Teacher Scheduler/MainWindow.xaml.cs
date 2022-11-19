using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
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
        private DatabaseContext? Context;

        /// <summary>
        /// Активный контрол в TabControl
        /// </summary>
        private TabItem _tabUserPage = null!;

        /// <summary>
        /// Конструктор главного окна приложения
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseContext();

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));

            MainControlRender();
        }

        /// <summary>
        /// Метод инициализации контекста базы данных, а также ее создание и наполнение справочников
        /// </summary>
        private void InitializeDatabaseContext()
        {
            Context = new DatabaseContext();
            Context.Database.Migrate();

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

        /// <summary>
        /// Обработчик события нажатия на кнопку меню 'Главная'
        /// </summary>
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainControlRender();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку меню 'Выходные'
        /// </summary>
        private void DaysOffMenuButton_Click(object sender, RoutedEventArgs e)
        {
            DayOffControlRender();
        }

        /// <summary>
        /// Метод отрисовки MainControl в главном окне приложения
        /// </summary>
        private void MainControlRender()
        {
            if (MainTab.Items.Count > 0 && MainTab.SelectedContent.GetType().FullName == typeof(MainControl).ToString())
            {
                return;
            }

            if (Context is null)
            {
                DialogWindow.Show($"Произошла ошибка работы с базой данных. Необходимо перезапустить программу.",
                    "Ошибка",
                    MessageBoxButton.OK);
                return;
            }

            MainTab.Items.Clear();
            _tabUserPage = new TabItem { Content = new MainControl(Context) };
            MainTab.Items.Add(_tabUserPage);
            MainTab.Items.Refresh();
        }

        /// <summary>
        /// Метод отрисовки DayOffControl в главном окне приложения
        /// </summary>
        private void DayOffControlRender()
        {
            if (MainTab.Items.Count > 0 && MainTab.SelectedContent.GetType().FullName == typeof(DayOffControl).ToString())
            {
                return;
            }

            if (Context is null)
            {
                DialogWindow.Show($"Произошла ошибка работы с базой данных. Необходимо перезапустить программу.",
                    "Ошибка",
                    MessageBoxButton.OK);
                return;
            }

            MainTab.Items.Clear();
            _tabUserPage = new TabItem { Content = new DayOffControl(Context) };
            MainTab.Items.Add(_tabUserPage);
            MainTab.Items.Refresh();
        }
    }
}