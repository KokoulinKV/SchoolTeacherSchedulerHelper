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
using Domain;
using Domain.Dtos;
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

        /// <summary>
        /// Конструктор главного окна приложения
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));

            this.Context = new DatabaseContext();
            this.Context.Database.Migrate();
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

        private TabItem _tabUserPage;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainTab.Items.Clear(); //Clear previous Items in the user controls which is my tabItems
            var userControls = new MainControl();
            _tabUserPage = new TabItem { Content = userControls };
            MainTab.Items.Add(_tabUserPage); // Add User Controls
            MainTab.Items.Refresh();
        }
    }
}