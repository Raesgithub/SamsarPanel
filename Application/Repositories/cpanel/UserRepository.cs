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
    public class UserRepository
    {
    

        public async Task<PaggingDto<UserVM>> GetAll(PaggingDto<UserVM> pagging)
        {
            if (pagging.Page>0)
            {
                if (pagging.Page + 1 > pagging.TotalPages) return pagging;
            }
            


            var page =pagging.Page * pagging.Take;
            
            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
                var where = @"where UserName like N'%'+@search+'%' or Firstname like  N'%'+@search+'%' 
                                             or Lastname like  N'%'+@search+'%' or Email like  N'%'+@search+'%' 
                                             or PhoneNumber like  N'%'+@search+'%'";
                var query = $@"declare @search nvarchar ='{pagging.Search}'
                                            declare @skip int={page}
                                            declare @take int={pagging.Take}
                                             --تعداد کل رکوردها
                                            select count(*)  from AspNetUsers {where}
                                             --دریافت داده
                                             select Email,Firstname,Lastname,PhoneNumber,Id,UserName,Avater,LastDateLogin,LoginCount, Firstname+' ' +Lastname as  FullName
                                             from AspNetUsers {where}
                                             order by Email
                                             offset @skip rows fetch next @take rows only ";
                var res = await con.QueryMultipleAsync(query);
                pagging.TotalRecords = (await res.ReadAsync<int>()).First();
                pagging.TotalPages = (pagging.TotalRecords % pagging.Take) == 0
                                                 ? pagging.TotalRecords / pagging.Take
                                                 : ((int)(pagging.TotalRecords / pagging.Take)) + 1;

                var newValues = (await res.ReadAsync<UserVM>()).ToList();
                newValues.InsertRange(0, pagging.Values);
                pagging.Values = newValues;
                return pagging;


            }
        }
        public async Task<UserVM?> GetById(string id)
        {
            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
                var query = $@"select Email,Firstname,Lastname,PhoneNumber,Id,UserName,Avater,LastDateLogin,LoginCount
                                             from AspNetUsers where id='{id}'";
                return await con.QuerySingleOrDefaultAsync<UserVM>(query);

            }
        }
        public async Task<ResultDto> UpdateAsync(UserVM userVM)
        {
            try
            {
                var query = $@"update aspnetusers 
                                        set firstname='{userVM.Firstname}', lastname='{userVM.Lastname}',
                                        email='{userVM.Email}', phonenumber='{userVM.PhoneNumber}'
                                        where id='{userVM.Id}' ";
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    res = await con.ExecuteAsync(query);
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
        public async Task<ResultDto> InsertAsync(UserVM userVM)
        {
            try
            {
                var query = $@"INSERT INTO aspnetusers (username, email, Firstname,Lastname,PhoneNumber) VALUES ()";
 
                                        
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    res = await con.ExecuteAsync(query);
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
        public async Task<ResultDto> UpdateAvatarAsync(string filename,string userId)
        {
            //
            try
            {
                var query = $@"update aspnetusers  set Avater='{filename}'   where id='{userId}' ";
                var res = 0;
                using (var con = new SqlConnection(ConstantCpanel.connectionString))
                {
                    res = await con.ExecuteAsync(query);
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

                return new ResultDto { IsSuccess = false, Message = ex.InnerException!=null? ex.InnerException.Message:ex.Message };

            }

        }

    }
}
