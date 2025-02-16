using Application.Dtos.cpanel;
using Dapper;
using Domain.Resourses;
using Domain.ViewModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.cpanel
{
    public class ProductRepository
    {
        // models
        // viewmodels
        //dtos 



        public async Task<PaggingDto<ProductVM>> GetAllByCatalogId(int catalogId,PaggingDto<ProductVM> pagging)
        {
            if (pagging.Page > 0)
            {
                if (pagging.Page + 1 > pagging.TotalPages) return pagging;
            }
            var page = pagging.Page * pagging.Take;

            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
                var where = @"where Name like N'%'+@search+'%' or Description like  N'%'+@search+'%' 
                                             or Mdate like  N'%'+@search+'%' ";
                var query = $@"declare @search nvarchar ='{pagging.Search}'
                                            declare @skip int={page}
                                            declare @take int={pagging.Take}
                                             --تعداد کل رکوردها
                                            select count(*)  from Products {where}
                                             --دریافت داده
                                             select Id,Name,Price,IsPublish,Images,Mdate 
                                             from Products {where}
                                             order by Mdate
                                             offset @skip rows fetch next @take rows only ";
                var res = await con.QueryMultipleAsync(query);
                pagging.TotalRecords = (await res.ReadAsync<int>()).First();
                pagging.TotalPages = (pagging.TotalRecords % pagging.Take) == 0
                                                 ? pagging.TotalRecords / pagging.Take
                                                 : ((int)(pagging.TotalRecords / pagging.Take)) + 1;

                var newValues = (await res.ReadAsync<ProductVM>()).ToList();
                newValues.InsertRange(0, pagging.Values);
                pagging.Values = newValues;
                return pagging;


            }
        }

        public async Task<PaggingDto<ProductVM>> GetAll(PaggingDto<ProductVM> pagging)
        {
            if (pagging.Page>0)
            {
                if (pagging.Page + 1 > pagging.TotalPages) return pagging;
            }
            var page =pagging.Page * pagging.Take;
            
            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
                var where = @"where Name like N'%'+@search+'%' or Description like  N'%'+@search+'%' 
                                             or Mdate like  N'%'+@search+'%' ";
                var query = $@"declare @search nvarchar ='{pagging.Search}'
                                            declare @skip int={page}
                                            declare @take int={pagging.Take}
                                             --تعداد کل رکوردها
                                            select count(*)  from Products {where}
                                             --دریافت داده
                                             select Id,Name,Price,IsPublish,Images,Mdate 
                                             from Products {where}
                                             order by Mdate
                                             offset @skip rows fetch next @take rows only ";
                var res = await con.QueryMultipleAsync(query);
                pagging.TotalRecords = (await res.ReadAsync<int>()).First();
                pagging.TotalPages = (pagging.TotalRecords % pagging.Take) == 0
                                                 ? pagging.TotalRecords / pagging.Take
                                                 : ((int)(pagging.TotalRecords / pagging.Take)) + 1;

                var newValues = (await res.ReadAsync<ProductVM>()).ToList();
                newValues.InsertRange(0, pagging.Values);
                pagging.Values = newValues;
                return pagging;
            }
        }

        public async Task<List<ProductVM>> GetPopular(int take=12)
        {
            
          
            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
                var query = $@" select top({take}) Id,Name,Price,PriceOld,seen,Images 
                                             from Products where IsPublish=1 order by Seen DESC";
                return  (await con.QueryAsync<ProductVM>(query)).ToList();
            }
        }


        public async Task<List<ProductVM>> GetProducts_With_CatalogId(int id,int take = 12)
        {


            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
                var query = $@" select top({take}) Id,Name,Price,PriceOld,seen,Images 
                                             from Products where IsPublish=1  and catalogid={id} order by Seen DESC";
                return (await con.QueryAsync<ProductVM>(query)).ToList();
            }
        }


        public async Task<ResultDto> DeleteAsync(int id)
        {
            try
            {
                var query = $@"delete from products where id='{id}' ";
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

        public async Task<ResultDto> UpdateStatusAsync(string id,bool status)
        {
            try
            {
                var query = $@"update aspnetusers 
                                        set IsSuspend={(status==true?1:0)}   where id='{id}' ";
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    //isert update delete
                    res = await con.ExecuteAsync(query);
                }
                if (res == 0)
                {
                    return new ResultDto { IsSuccess = false, Message = "قادر به تغییر وضعیت نشدیم" };
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
        public async Task<ResultDto> InsertAsync(Domain.Models.shop.Product product)
        {
            try
            {
                var query = $@"INSERT INTO products (Name, Description, Price,PriceOld,Feauchers,IsPublish,
                                 Images,Cdate,Mdate,CatalogId) VALUES (@Name, @Description, @Price,@PriceOld,
                                 @Feauchers,@IsPublish,@Images,@Cdate,@Mdate,@CatalogId)";
                                        
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    res = await con.ExecuteAsync(query,product);
                }
                if (res == 0)
                {
                    return new ResultDto { IsSuccess = false, Message = "قادر به ثبت اطلاعات نشدیم . اطلاعات ورودی معتبر نمی باشد" };
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
