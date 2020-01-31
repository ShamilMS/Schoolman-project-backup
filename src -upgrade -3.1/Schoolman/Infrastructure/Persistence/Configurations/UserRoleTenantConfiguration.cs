﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    class UserRoleTenantConfiguration : IEntityTypeConfiguration<UserRoleTenant>
    {
        public void Configure(EntityTypeBuilder<UserRoleTenant> userRoleTenant)
        {
            userRoleTenant.Property(model => model.RelationId)
                          .ValueGeneratedOnAdd()
                          .IsRequired();

            userRoleTenant.HasIndex(model => model.RelationId);

            userRoleTenant.HasOne(urt => urt.Tenant)
                          .WithMany("_userRoleTenantRelation")
                          //.WithMany(t => t.UserRoleTenantRelation)
                          .HasForeignKey(urt => urt.TenantId)
                          .OnDelete(DeleteBehavior.Restrict);

            userRoleTenant.HasOne(urt => urt.Role)
                          .WithMany("_userRoleTenantRelation")
                         //.WithMany(t => t.UserRoleTenantRelation)
                         .HasForeignKey(urt => urt.RoleId)
                         .OnDelete(DeleteBehavior.Restrict);

            userRoleTenant.HasOne(urt => urt.User)
                          //.WithMany(t => t.UserRoleTenantRelation)
                          .WithMany("_userRoleTenantRelation")
                         .HasForeignKey(urt => urt.UserId)
                         .OnDelete(DeleteBehavior.Cascade);

            userRoleTenant.HasKey(model => new { model.UserId, model.RoleId, model.TenantId });

            userRoleTenant.ToTable("UserRoleTenantRelations");
        }
    }
}