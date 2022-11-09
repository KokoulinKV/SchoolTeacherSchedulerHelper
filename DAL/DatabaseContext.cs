﻿using Domain;
using Microsoft.EntityFrameworkCore;
using Windows.Storage;

namespace DAL
{
    /// <summary>
    /// Класс БД контекста
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Сущность Выходного для БД контекста
        /// </summary>
        public DbSet<DayOff> DaysOff { get; set; }

        /// <summary>
        /// Конструктор для EF
        /// </summary>
        public DatabaseContext()
        {
        }

        /// <summary>
        /// Конфигурация подключения к БД при создании экземпляра контекста
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source =" + Path.Combine(ApplicationData.Current.LocalFolder.Path, "base.db"));
        }
    }
}