using Hackathon_TEK.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hackathon_TEK.Repository
{
    /// <summary>
    /// Базовый репозиторий
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AbstractRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly HackathonContext _db;
        private readonly ILogger<IndexModel> _logger;

        public AbstractRepository(HackathonContext db, ILogger<IndexModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Добавляет объект
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Add(T model)
        {
            try
            {
                _db.Add(model);
                _db.SaveChanges();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавляет элементы указанной коллекции
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> AddRange(IEnumerable<T> models)
        {
            try
            {
                _db.AddRange(models);
                _db.SaveChanges();

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении файлов: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Update(T model)
        {
            try
            {
                _db.Update(model);
                _db.SaveChanges();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Update(int id)
        {
            try
            {
                var model = _db.Set<T>().AsNoTracking().FirstOrDefault(p => p.Id == id);

                if (model != null)
                {
                    _db.Update(model);
                    _db.SaveChanges();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление элементов указанной коллекции
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> UpdateRange(IEnumerable<T> models)
        {
            try
            {
                _db.UpdateRange(models);
                _db.SaveChanges();

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении файлов: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Remove(T model)
        {
            try
            {
                model.IsDelete = true;

                _db.Update(model);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление объекта по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Remove(int id)
        {
            try
            {
                var model = _db.Set<T>().AsNoTracking().FirstOrDefault(p => p.Id == id);

                if (model != null)
                {
                    model.IsDelete = true;

                    _db.Update(model);
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет элементы указанной коллекции
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual bool RemoveRange(IEnumerable<T> models)
        {
            try
            {
                foreach (var model in models)
                    model.IsDelete = true;

                _db.UpdateRange(models);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении файлов: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Delete(T model)
        {
            try
            {
                _db.Remove(model);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить коллекцию
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetListQuery()
        {
            return _db.Set<T>().AsNoTracking().Where(p => !p.IsDelete).AsQueryable();
        }

        /// <summary>
        /// Получить коллекцию с удаленными объектами
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetListQueryWithDeleted()
        {
            return _db.Set<T>().AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// Получить коллекцию
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetList()
        {
            return _db.Set<T>().AsNoTracking().Where(p => !p.IsDelete).ToList();
        }

        /// <summary>
        /// Получить коллекцию с удаленными объектами
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetListWithDeleted()
        {
            return _db.Set<T>().AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// Возвращает первый элемент последовательности или значение по умолчанию, если ни одного элемента не найдено
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T SearchById(int id)
        {
            return GetListQuery().FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Проверяет существование хотя бы одного элемента в последовательности 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<T, bool>> func)
        {
            return GetListQuery().Any(func);
        }

        /// <summary>
        /// Возвращает первый элемент последовательности или значение по умолчанию, если ни одного элемента не найдено
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual T FirstOrDefault(Expression<Func<T, bool>> func)
        {
            return GetListQuery().FirstOrDefault(func);
        }
    }
}