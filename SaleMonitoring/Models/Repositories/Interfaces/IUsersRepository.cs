﻿using SalaryCalc.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalc.Models.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Получить идентификатор текущего пользователя.
        /// </summary>
        /// <returns>Идентификатор пользователя.</returns>
        string GetCurrentUserId();

        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns>Набор пользователей.</returns>
        IQueryable<User> GetUsers();

        /// <summary>
        /// Получить пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Пользователь.</returns>
        User GetUserById(string id);

        /// <summary>
        /// Получить пользователя по его имени.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Пользователь.</returns>
        User GetUserByUserName(string userName);

        /// <summary>
        /// Получить заработную плату по дате.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="Year">Год заработной платы.</param>
        /// <param name="Month">Месяц заработной платы.</param>
        /// <returns>Заработная плата.</returns>
        Salary GetSalaryByDate(string userId, ushort Year, byte Month);

        /// <summary>
        /// Получить все заработные платы сотрудника.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Набор заработных плат.</returns>
        IQueryable<Salary> GetSalaries(string id);

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        /// <param name="user">Объект класса User.</param>
        /// <param name="user">Пароль.</param>
        void SaveUser(User user);

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        void DeleteUser(string id);
    }
}
