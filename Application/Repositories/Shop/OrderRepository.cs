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
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.cpanel
{
    public class OrderRepository
    {

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
                // لاگ کردن خطا برای دیباگ
                Console.WriteLine($"خطا در ثبت سفارش: {ex.Message}");
                return false;
            }
        }
    }
}
