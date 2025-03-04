﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20250214120135_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.EF.SeedingHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SeedingHistory");
                });

            modelBuilder.Entity("Core.EventBus.IntegrationEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("JsonPayLoad")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IntegrationEvents");
                });

            modelBuilder.Entity("Core.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorMsg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExecutedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessage");
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CanceledAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuoteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ShipmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ShippedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.OwnsOne("Domain.Money", "TotalPrice", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("TotalPrice");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.OwnsOne("Domain.Currency", "Currency", b2 =>
                                {
                                    b2.Property<Guid>("MoneyOrderId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Code")
                                        .IsRequired()
                                        .HasMaxLength(5)
                                        .HasColumnType("nvarchar(5)")
                                        .HasColumnName("CurrencyCode");

                                    b2.Property<string>("Symbol")
                                        .IsRequired()
                                        .HasMaxLength(5)
                                        .HasColumnType("nvarchar(5)")
                                        .HasColumnName("CurrencySymbol");

                                    b2.HasKey("MoneyOrderId");

                                    b2.ToTable("Orders");

                                    b2.WithOwner()
                                        .HasForeignKey("MoneyOrderId");
                                });

                            b1.Navigation("Currency")
                                .IsRequired();
                        });

                    b.OwnsMany("Domain.OrderLine", "OrderLines", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.HasKey("OrderId", "Id");

                            b1.ToTable("OrderLine");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.OwnsOne("Domain.ProductItem", "ProductItem", b2 =>
                                {
                                    b2.Property<Guid>("OrderLineOrderId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("OrderLineId")
                                        .HasColumnType("int");

                                    b2.Property<Guid>("ProductId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("ProductName")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<int>("Quantity")
                                        .HasColumnType("int");

                                    b2.HasKey("OrderLineOrderId", "OrderLineId");

                                    b2.ToTable("OrderLine");

                                    b2.WithOwner()
                                        .HasForeignKey("OrderLineOrderId", "OrderLineId");

                                    b2.OwnsOne("Domain.Currency", "Currency", b3 =>
                                        {
                                            b3.Property<Guid>("ProductItemOrderLineOrderId")
                                                .HasColumnType("uniqueidentifier");

                                            b3.Property<int>("ProductItemOrderLineId")
                                                .HasColumnType("int");

                                            b3.Property<string>("Code")
                                                .IsRequired()
                                                .ValueGeneratedOnUpdateSometimes()
                                                .HasMaxLength(5)
                                                .HasColumnType("nvarchar(5)")
                                                .HasColumnName("CurrencyCode");

                                            b3.Property<string>("Symbol")
                                                .IsRequired()
                                                .ValueGeneratedOnUpdateSometimes()
                                                .HasMaxLength(5)
                                                .HasColumnType("nvarchar(5)")
                                                .HasColumnName("CurrencySymbol");

                                            b3.HasKey("ProductItemOrderLineOrderId", "ProductItemOrderLineId");

                                            b3.ToTable("OrderLine");

                                            b3.WithOwner()
                                                .HasForeignKey("ProductItemOrderLineOrderId", "ProductItemOrderLineId");
                                        });

                                    b2.OwnsOne("Domain.Money", "UnitPrice", b3 =>
                                        {
                                            b3.Property<Guid>("ProductItemOrderLineOrderId")
                                                .HasColumnType("uniqueidentifier");

                                            b3.Property<int>("ProductItemOrderLineId")
                                                .HasColumnType("int");

                                            b3.Property<decimal>("Amount")
                                                .HasColumnType("decimal(18,2)");

                                            b3.HasKey("ProductItemOrderLineOrderId", "ProductItemOrderLineId");

                                            b3.ToTable("OrderLine");

                                            b3.WithOwner()
                                                .HasForeignKey("ProductItemOrderLineOrderId", "ProductItemOrderLineId");

                                            b3.OwnsOne("Domain.Currency", "Currency", b4 =>
                                                {
                                                    b4.Property<Guid>("MoneyProductItemOrderLineOrderId")
                                                        .HasColumnType("uniqueidentifier");

                                                    b4.Property<int>("MoneyProductItemOrderLineId")
                                                        .HasColumnType("int");

                                                    b4.Property<string>("Code")
                                                        .IsRequired()
                                                        .ValueGeneratedOnUpdateSometimes()
                                                        .HasMaxLength(5)
                                                        .HasColumnType("nvarchar(5)")
                                                        .HasColumnName("CurrencyCode");

                                                    b4.Property<string>("Symbol")
                                                        .IsRequired()
                                                        .ValueGeneratedOnUpdateSometimes()
                                                        .HasMaxLength(5)
                                                        .HasColumnType("nvarchar(5)")
                                                        .HasColumnName("CurrencySymbol");

                                                    b4.HasKey("MoneyProductItemOrderLineOrderId", "MoneyProductItemOrderLineId");

                                                    b4.ToTable("OrderLine");

                                                    b4.WithOwner()
                                                        .HasForeignKey("MoneyProductItemOrderLineOrderId", "MoneyProductItemOrderLineId");
                                                });

                                            b3.Navigation("Currency")
                                                .IsRequired();
                                        });

                                    b2.Navigation("Currency")
                                        .IsRequired();

                                    b2.Navigation("UnitPrice")
                                        .IsRequired();
                                });

                            b1.Navigation("ProductItem")
                                .IsRequired();
                        });

                    b.Navigation("OrderLines");

                    b.Navigation("TotalPrice")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
