using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Api.Data.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            SHA256Managed sha = new SHA256Managed();
            byte[] buffer = Encoding.Default.GetBytes("123456");
            byte[] dataCifrada = sha.ComputeHash(buffer);
            string strPassword = BitConverter.ToString(dataCifrada).Replace("-", "");

            builder.HasData(
                new User() { Id = 1, Email = "admin@test.com", Password = strPassword, RoleId = 1, Name="Admin" }
                );
        }
    }
}
