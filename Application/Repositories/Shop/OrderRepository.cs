using Application.Dtos.cpanel;
using Dapper;
using Domain.Models.shop;
using Domain.Resourses;
using Domain.ViewModels;
using MD.PersianDateTime;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repositories.cpanel
{
    public class OrderRepository
    {
        /// <summary>
        /// درج سفارش جدید
        /// </summary>
        public async Task<bool> InsertAsync(int proId, ContactModel contactModel)
        {
            try
            {
                var persianDate = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");

                const string query = @"
                    INSERT INTO orders (Phone, ProductId, Cdate, IsNew, Email, FullName, Message, Subject)
                    VALUES (@Phone, @ProductId, @Cdate, 1, @Email, @FullName, @Message, @Subject)";

                var parameters = new
                {
                    Phone = contactModel.Phone ?? string.Empty,
                    ProductId = proId,
                    Cdate = persianDate,
                    Email = contactModel.Email ?? string.Empty,
                    FullName = contactModel.FullName ?? string.Empty,
                    Message = contactModel.Message ?? string.Empty,
                    Subject = contactModel.Subject ?? string.Empty
                };

                using var con = new SqlConnection(ConstantCpanel.connectionString);
                var res = await con.ExecuteAsync(query, parameters);

                return res == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در ثبت سفارش: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// گرفتن لیست سفارش‌ها با صفحه‌بندی (۱۰ تایی)
        /// </summary>
        public async Task<(IEnumerable<OrderViewModel> data, int total)> GetPagedOrdersAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                using var con = new SqlConnection(ConstantCpanel.connectionString);

                string queryData = @"
                    SELECT Id, FullName, Email, Phone, Subject, Message, Cdate, ProductId, IsNew
                    FROM orders
                    ORDER BY Id DESC
                    OFFSET (@Offset) ROWS
                    FETCH NEXT @PageSize ROWS ONLY";

                string queryCount = "SELECT COUNT(*) FROM orders";

                var total = await con.ExecuteScalarAsync<int>(queryCount);
                var data = await con.QueryAsync<OrderViewModel>(queryData, new
                {
                    Offset = (page - 1) * pageSize,
                    PageSize = pageSize
                });

                return (data, total);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در گرفتن سفارش‌ها: {ex.Message}");
                return (Enumerable.Empty<OrderViewModel>(), 0);
            }
        }
    }
}
