﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OzonTech.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TechSupportEntities : DbContext
    {
        public TechSupportEntities()
            : base("name=TechSupportEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<CategoryRequest> CategoryRequest { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Managment> Managment { get; set; }
        public virtual DbSet<TypeUsers> TypeUsers { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
