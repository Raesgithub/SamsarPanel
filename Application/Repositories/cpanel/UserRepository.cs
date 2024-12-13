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
      // models
      // viewmodels
      //dtos 

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
                                             select Email,Firstname,Lastname,PhoneNumber,Id,UserName,Avater,LastDateLogin,LoginCount
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

        public async Task<bool> UpdateAsync(UserVM userVM)
        {
            
            var query = $@"update aspnetusers 
                                        set firstname='{userVM.Firstname}', lastname='{userVM.Lastname}',
                                        email='{userVM.Email}', phonenumber='{userVM.PhoneNumber}'
                                        where id='{userVM.UserId}' ";
            var res = 0;
            using (var con = new SqlConnection(ConstantCpanel.connectionString))
            {
              res=  await con.ExecuteAsync(query);
            }
            return res==0?false:true;

        }

    }
}
