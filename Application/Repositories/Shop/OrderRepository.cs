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
        
        public async Task<bool> InsertAsync(int proId,string phone)
        {
            try
            {
                var query = $@"INSERT INTO orders (Phone, ProductId, Cdate,IsNew)
                                VALUES (N'{phone}', {proId}, {new PersianDateTime(DateTime.Now).ToString()},1)";
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    res = await con.ExecuteAsync(query);
                }
                return res == 1;
            }
            catch (Exception ex)
            {

                return false;

            }


        }
      

    }
}
