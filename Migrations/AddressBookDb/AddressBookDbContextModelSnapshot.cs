using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AddressBook.Data;
using AddressBook.Models;

namespace AddressBook.Migrations.AddressBookDb
{
    [DbContext(typeof(AddressBookDbContext))]
    partial class AddressBookDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("AddressBook.Models.ContactAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressType");

                    b.Property<string>("City");

                    b.Property<int?>("ContactPersonId");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("StreetAdd");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.HasKey("Id");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("ContactAddress");
                });

            modelBuilder.Entity("AddressBook.Models.ContactPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("GenderType");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .HasMaxLength(12);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ContactPerson");
                });

            modelBuilder.Entity("AddressBook.Models.ContactAddress", b =>
                {
                    b.HasOne("AddressBook.Models.ContactPerson", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId");
                });
        }
    }
}
