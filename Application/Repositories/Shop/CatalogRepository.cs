using Application.Dtos.cpanel;
using Dapper;
using Domain.Models.shop;
using Domain.Resourses;
using Domain.ViewModels;
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
                //var query = $@"insert into Catalogs(Name,Logo) values(N'{cat.Name}',N'{cat.Logo}')";
                var query = $@" INSERT INTO Catalogs (Name, Logo) 
                                        VALUES (N'{cat.Name}', N'{cat.Logo}');
                                        SELECT CAST(SCOPE_IDENTITY() AS int);";

                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    //isert update delete
                    //res = await con.ExecuteAsync(query);
                    var newId = await con.ExecuteScalarAsync<int>(query);
                    return new ResultDto { IsSuccess = true, Message = newId.ToString() };
                }
               

                //if (res == 0)
                //{
                //    return new ResultDto { IsSuccess = false, Message = "قادر به افزودن دسته جدید نشدیم" };
                //}
                //else
                //{
                //    return new ResultDto { IsSuccess = true };
                //}
            }
            catch (Exception ex)
            {

                return new ResultDto { IsSuccess = false, Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };

            }
        }
        public async Task<ResultDto> DeleteAsync(int id)
        {
            try
            {
                var query = $@"delete from Catalogs where id={id}";
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    //isert update delete
                    res = await con.ExecuteAsync(query);
                }
                if (res == 0)
                {
                    return new ResultDto { IsSuccess = false, Message = "قادر به حذف نشدیم" };
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

        public async Task<List<Catalog?>> GetAllAsync()
        {
            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
                var query = $@"select Id,Name,Logo from Catalogs";
                 return  (await con.QueryAsync<Catalog>(query)).ToList();
            }
            
        }

    }
}
