using System;
using System.Windows;
using System.Windows.Input;

namespace School_Teacher_Scheduler
{
    /// <summary>
    /// Класс диалогового окна
    /// </summary>
    public partial class DialogWindow : Window
    {
        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        public event Action? CloseWindowEvent;

        /// <summary>
        /// Конструктор класса диалогового окна
        /// </summary>
        public DialogWindow()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
        }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message
        {
            set { this.message.Text = value; }
            get { return this.message.Text; }
        }

        /// <summary>
        /// Метод отображения диалогового окна
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="msg">Сообщение</param>
        /// <param name="type">Тип кнопок</param>
        public static bool? Show(string msg, string title, MessageBoxButton type)
        {
            var msgBox = new DialogWindow();

            switch (type)
            {
                case MessageBoxButton.OK:
                    msgBox.yesNoGrid.Visibility = Visibility.Collapsed;
                    msgBox.ok.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.OKCancel:
                    msgBox.yesNoGrid.Visibility = Visibility.Visible;
                    msgBox.ok.Visibility = Visibility.Collapsed;
                    break;

                default: throw new ArgumentException("Диалоговое окно может работать только с кнопками 'OK' и 'OKCancel'");
            }

            msgBox.ResizeMode = ResizeMode.NoResize;
            msgBox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            msgBox.Title = title;
            msgBox.Message = msg;

            return msgBox.ShowDialog();
        }

        /// <summary>
        /// Обработчик нажатия кнопки 'Ок'
        /// </summary>
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки 'Отмена'
        /// </summary>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки 'Да'
        /// </summary>
        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Метод закрытия окна
        /// </summary>
        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (null != CloseWindowEvent)
            {
                CloseWindowEvent();
            }
            this.Close();
        }
    }
}