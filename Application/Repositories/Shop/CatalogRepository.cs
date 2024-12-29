using Application.Dtos.cpanel;
using Dapper;
using Domain.Models.shop;
using Domain.Resourses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Shop
{
    public class CatalogRepository
    {
        public async Task<ResultDto> AddAsync(Catalog cat) 
        {
            try
            {
                var query = $@"insert into Catalogs(Name,Logo) values('{cat.Name}','{cat.Logo}')";
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    //isert update delete
                    res = await con.ExecuteAsync(query);
                }
                if (res == 0)
                {
                    return new ResultDto { IsSuccess = false, Message = "قادر به افزودن دسته جدید نشدیم" };
                }
                else
                {
                    return new ResultDto { IsSuccess = true };
                }
            }
            catch (Exception ex)
            {

                return new ResultDto { IsSuccess = false, Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };

            }
        }
    }
}
