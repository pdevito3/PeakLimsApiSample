﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PeakLims.Databases;

#nullable disable

namespace PeakLims.Migrations
{
    [DbContext(typeof(PeakLimsDbContext))]
    partial class PeakLimsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("ACC")
                .StartsAt(10005702L);

            modelBuilder.HasSequence("PAT")
                .StartsAt(10145702L);

            modelBuilder.Entity("PanelTest", b =>
                {
                    b.Property<Guid>("PanelsId")
                        .HasColumnType("uuid")
                        .HasColumnName("panels_id");

                    b.Property<Guid>("TestsId")
                        .HasColumnType("uuid")
                        .HasColumnName("tests_id");

                    b.HasKey("PanelsId", "TestsId")
                        .HasName("pk_panel_test");

                    b.HasIndex("TestsId")
                        .HasDatabaseName("ix_panel_test_tests_id");

                    b.ToTable("panel_test", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.AccessionComments.AccessionComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccessionId")
                        .HasColumnType("uuid")
                        .HasColumnName("accession_id");

                    b.Property<string>("Comment")
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("EndingAccessionState")
                        .HasColumnType("text")
                        .HasColumnName("ending_accession_state");

                    b.Property<string>("InitialAccessionState")
                        .HasColumnType("text")
                        .HasColumnName("initial_accession_state");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<Guid?>("OriginalCommentId")
                        .HasColumnType("uuid")
                        .HasColumnName("original_comment_id");

                    b.Property<Guid?>("ParentAccessionCommentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_accession_comment_id");

                    b.HasKey("Id")
                        .HasName("pk_accession_comments");

                    b.HasIndex("AccessionId")
                        .HasDatabaseName("ix_accession_comments_accession_id");

                    b.HasIndex("ParentAccessionCommentId")
                        .HasDatabaseName("ix_accession_comments_parent_accession_comment_id");

                    b.ToTable("accession_comments", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Accessions.Accession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AccessionNumber")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("accession_number")
                        .HasDefaultValueSql("concat('ACC', nextval('\"ACC\"'))");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<Guid?>("HealthcareOrganizationId")
                        .HasColumnType("uuid")
                        .HasColumnName("healthcare_organization_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("uuid")
                        .HasColumnName("patient_id");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_accessions");

                    b.ToTable("accessions", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Containers.Container", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ContainerNumber")
                        .HasColumnType("text")
                        .HasColumnName("container_number");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_containers");

                    b.ToTable("containers", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.HealthcareOrganizationContacts.HealthcareOrganizationContact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AccessionId")
                        .HasColumnType("uuid")
                        .HasColumnName("accession_id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<Guid>("HealthcareOrganizationId")
                        .HasColumnType("uuid")
                        .HasColumnName("healthcare_organization_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Npi")
                        .HasColumnType("text")
                        .HasColumnName("npi");

                    b.HasKey("Id")
                        .HasName("pk_healthcare_organization_contacts");

                    b.HasIndex("AccessionId")
                        .HasDatabaseName("ix_healthcare_organization_contacts_accession_id");

                    b.HasIndex("HealthcareOrganizationId")
                        .HasDatabaseName("ix_healthcare_organization_contacts_healthcare_organization_id");

                    b.ToTable("healthcare_organization_contacts", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.HealthcareOrganizations.HealthcareOrganization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_healthcare_organizations");

                    b.ToTable("healthcare_organizations", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Panels.Panel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("PanelCode")
                        .HasColumnType("text")
                        .HasColumnName("panel_code");

                    b.Property<string>("PanelName")
                        .HasColumnType("text")
                        .HasColumnName("panel_name");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_panels");

                    b.ToTable("panels", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Patients.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("Ethnicity")
                        .HasColumnType("text")
                        .HasColumnName("ethnicity");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("InternalId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("internal_id")
                        .HasDefaultValueSql("concat('PAT', nextval('\"PAT\"'))");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Race")
                        .HasColumnType("text")
                        .HasColumnName("race");

                    b.Property<string>("Sex")
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.HasKey("Id")
                        .HasName("pk_patients");

                    b.ToTable("patients", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.RolePermissions.RolePermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("Permission")
                        .HasColumnType("text")
                        .HasColumnName("permission");

                    b.Property<string>("Role")
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_role_permissions");

                    b.ToTable("role_permissions", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Samples.Sample", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly?>("CollectionDate")
                        .HasColumnType("date")
                        .HasColumnName("collection_date");

                    b.Property<string>("CollectionSite")
                        .HasColumnType("text")
                        .HasColumnName("collection_site");

                    b.Property<Guid?>("ContainerId")
                        .HasColumnType("uuid")
                        .HasColumnName("container_id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<Guid?>("ParentSampleId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_sample_id");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("uuid")
                        .HasColumnName("patient_id");

                    b.Property<decimal?>("Quantity")
                        .HasColumnType("numeric")
                        .HasColumnName("quantity");

                    b.Property<DateOnly?>("ReceivedDate")
                        .HasColumnType("date")
                        .HasColumnName("received_date");

                    b.Property<string>("SampleNumber")
                        .HasColumnType("text")
                        .HasColumnName("sample_number");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_samples");

                    b.HasIndex("ContainerId")
                        .HasDatabaseName("ix_samples_container_id");

                    b.HasIndex("ParentSampleId")
                        .HasDatabaseName("ix_samples_parent_sample_id");

                    b.HasIndex("PatientId")
                        .HasDatabaseName("ix_samples_patient_id");

                    b.ToTable("samples", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.TestOrders.TestOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AccessionId")
                        .HasColumnType("uuid")
                        .HasColumnName("accession_id");

                    b.Property<Guid?>("AssociatedPanelId")
                        .HasColumnType("uuid")
                        .HasColumnName("associated_panel_id");

                    b.Property<string>("CancellationComments")
                        .HasColumnType("text")
                        .HasColumnName("cancellation_comments");

                    b.Property<string>("CancellationReason")
                        .HasColumnType("text")
                        .HasColumnName("cancellation_reason");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<DateOnly?>("DueDate")
                        .HasColumnType("date")
                        .HasColumnName("due_date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<int?>("TatSnapshot")
                        .HasColumnType("integer")
                        .HasColumnName("tat_snapshot");

                    b.Property<Guid?>("TestId")
                        .HasColumnType("uuid")
                        .HasColumnName("test_id");

                    b.HasKey("Id")
                        .HasName("pk_test_orders");

                    b.HasIndex("AccessionId")
                        .HasDatabaseName("ix_test_orders_accession_id");

                    b.HasIndex("AssociatedPanelId")
                        .HasDatabaseName("ix_test_orders_associated_panel_id");

                    b.HasIndex("TestId")
                        .HasDatabaseName("ix_test_orders_test_id");

                    b.ToTable("test_orders", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Tests.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("Methodology")
                        .HasColumnType("text")
                        .HasColumnName("methodology");

                    b.Property<string>("Platform")
                        .HasColumnType("text")
                        .HasColumnName("platform");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("TestCode")
                        .HasColumnType("text")
                        .HasColumnName("test_code");

                    b.Property<string>("TestName")
                        .HasColumnType("text")
                        .HasColumnName("test_name");

                    b.Property<int>("TurnAroundTime")
                        .HasColumnType("integer")
                        .HasColumnName("turn_around_time");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_tests");

                    b.ToTable("tests", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("Identifier")
                        .HasColumnType("text")
                        .HasColumnName("identifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("PeakLims.Domain.Users.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on");

                    b.Property<string>("Role")
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_roles");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_roles_user_id");

                    b.ToTable("user_roles", (string)null);
                });

            modelBuilder.Entity("PanelTest", b =>
                {
                    b.HasOne("PeakLims.Domain.Panels.Panel", null)
                        .WithMany()
                        .HasForeignKey("PanelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_panel_test_panels_panels_id");

                    b.HasOne("PeakLims.Domain.Tests.Test", null)
                        .WithMany()
                        .HasForeignKey("TestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_panel_test_tests_tests_id");
                });

            modelBuilder.Entity("PeakLims.Domain.AccessionComments.AccessionComment", b =>
                {
                    b.HasOne("PeakLims.Domain.Accessions.Accession", "Accession")
                        .WithMany("Comments")
                        .HasForeignKey("AccessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_accession_comments_accessions_accession_id");

                    b.HasOne("PeakLims.Domain.AccessionComments.AccessionComment", "ParentAccessionComment")
                        .WithMany()
                        .HasForeignKey("ParentAccessionCommentId")
                        .HasConstraintName("fk_accession_comments_accession_comments_parent_accession_comm");

                    b.Navigation("Accession");

                    b.Navigation("ParentAccessionComment");
                });

            modelBuilder.Entity("PeakLims.Domain.HealthcareOrganizationContacts.HealthcareOrganizationContact", b =>
                {
                    b.HasOne("PeakLims.Domain.Accessions.Accession", null)
                        .WithMany("Contacts")
                        .HasForeignKey("AccessionId")
                        .HasConstraintName("fk_healthcare_organization_contacts_accessions_accession_id");

                    b.HasOne("PeakLims.Domain.HealthcareOrganizations.HealthcareOrganization", "HealthcareOrganization")
                        .WithMany("Contacts")
                        .HasForeignKey("HealthcareOrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_healthcare_organization_contacts_healthcare_organizations_h");

                    b.Navigation("HealthcareOrganization");
                });

            modelBuilder.Entity("PeakLims.Domain.HealthcareOrganizations.HealthcareOrganization", b =>
                {
                    b.OwnsOne("PeakLims.Domain.Addresses.Address", "PrimaryAddress", b1 =>
                        {
                            b1.Property<Guid>("HealthcareOrganizationId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("City")
                                .HasColumnType("text")
                                .HasColumnName("primary_address_city");

                            b1.Property<string>("Country")
                                .HasColumnType("text")
                                .HasColumnName("primary_address_country");

                            b1.Property<string>("Line1")
                                .HasColumnType("text")
                                .HasColumnName("primary_address_line1");

                            b1.Property<string>("Line2")
                                .HasColumnType("text")
                                .HasColumnName("primary_address_line2");

                            b1.Property<string>("PostalCode")
                                .HasColumnType("text")
                                .HasColumnName("primary_address_postal_code");

                            b1.Property<string>("State")
                                .HasColumnType("text")
                                .HasColumnName("primary_address_state");

                            b1.HasKey("HealthcareOrganizationId");

                            b1.ToTable("healthcare_organizations");

                            b1.WithOwner()
                                .HasForeignKey("HealthcareOrganizationId")
                                .HasConstraintName("fk_healthcare_organizations_healthcare_organizations_id");
                        });

                    b.Navigation("PrimaryAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("PeakLims.Domain.Patients.Patient", b =>
                {
                    b.OwnsOne("PeakLims.Domain.Lifespans.Lifespan", "Lifespan", b1 =>
                        {
                            b1.Property<Guid>("PatientId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<int?>("Age")
                                .HasColumnType("integer")
                                .HasColumnName("age");

                            b1.Property<DateOnly?>("DateOfBirth")
                                .HasColumnType("date")
                                .HasColumnName("date_of_birth");

                            b1.HasKey("PatientId");

                            b1.ToTable("patients");

                            b1.WithOwner()
                                .HasForeignKey("PatientId")
                                .HasConstraintName("fk_patients_patients_id");
                        });

                    b.Navigation("Lifespan")
                        .IsRequired();
                });

            modelBuilder.Entity("PeakLims.Domain.Samples.Sample", b =>
                {
                    b.HasOne("PeakLims.Domain.Containers.Container", "Container")
                        .WithMany()
                        .HasForeignKey("ContainerId")
                        .HasConstraintName("fk_samples_containers_container_id");

                    b.HasOne("PeakLims.Domain.Samples.Sample", "ParentSample")
                        .WithMany()
                        .HasForeignKey("ParentSampleId")
                        .HasConstraintName("fk_samples_samples_parent_sample_id");

                    b.HasOne("PeakLims.Domain.Patients.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .HasConstraintName("fk_samples_patients_patient_id");

                    b.Navigation("Container");

                    b.Navigation("ParentSample");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("PeakLims.Domain.TestOrders.TestOrder", b =>
                {
                    b.HasOne("PeakLims.Domain.Accessions.Accession", "Accession")
                        .WithMany("TestOrders")
                        .HasForeignKey("AccessionId")
                        .HasConstraintName("fk_test_orders_accessions_accession_id");

                    b.HasOne("PeakLims.Domain.Panels.Panel", "AssociatedPanel")
                        .WithMany()
                        .HasForeignKey("AssociatedPanelId")
                        .HasConstraintName("fk_test_orders_panels_associated_panel_id");

                    b.HasOne("PeakLims.Domain.Tests.Test", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .HasConstraintName("fk_test_orders_tests_test_id");

                    b.Navigation("Accession");

                    b.Navigation("AssociatedPanel");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("PeakLims.Domain.Users.UserRole", b =>
                {
                    b.HasOne("PeakLims.Domain.Users.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PeakLims.Domain.Accessions.Accession", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Contacts");

                    b.Navigation("TestOrders");
                });

            modelBuilder.Entity("PeakLims.Domain.HealthcareOrganizations.HealthcareOrganization", b =>
                {
                    b.Navigation("Contacts");
                });

            modelBuilder.Entity("PeakLims.Domain.Users.User", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
