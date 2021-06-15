use [master]
create database Clinic
go

use Clinic
go

create table [dbo].[Account](
	[Id] int primary key identity,
	[Type] int not null,
	[Login] nvarchar(30),
	[Password] nvarchar(30) not null

	constraint UC_Account_Login unique ([Login])
);

create table [dbo].[Medicine](
	[Id] int primary key identity,
	[Name] nvarchar(30) not null,
	[Price] float not null, 
	[Doses] int not null,
	[Quantity] int not null
);

create table [dbo].[Equipment](
	[Id] int primary key identity,
	[Name] nvarchar(30) not null,
	[Price] float not null, 
	[Quantity] int not null
);

create table [dbo].[Category](
	[Id] int primary key identity,
	[Value] nvarchar(30) not null

	constraint UC_Category_Value unique ([Value])
);

create table [dbo].[Cabinet](
	[Id] int primary key identity,
	[Number] int not null

	constraint UC_Cabinet_Number unique ([Number])
);

create table [dbo].[CabinetEquipment](
	[IdCabinet] int not null,
	[IdEquipment] int not null

	constraint FK_CabinetEquipment_IdCabinet foreign key ([IdCabinet])
		references [Cabinet](Id),
	constraint FK_CabinetEquipment_IdEquipment foreign key ([IdEquipment])
		references [Equipment](Id)
);

create table [dbo].[CabinetMedicine](
	[IdCabinet] int not null,
	[IdMedicine] int not null

	constraint FK_CabinetMedicine_IdCabinet foreign key ([IdCabinet])
		references [Cabinet](Id),
	constraint FK_CabinetMedicine_IdMedicine foreign key ([IdMedicine])
		references [Medicine](Id)
);

create table [dbo].[Procedure](
	[Id] int primary key identity,
	[Name] nvarchar(30),
	[Description] nvarchar(max),
	[Price] float not null,
	[Duration] int not null
);

create table [dbo].[Admin](
	[Id] int primary key identity,
	[IsMain] bit,
	[Name] nvarchar(30) ,
	[Surname] nvarchar(30),
	[Middlename] nvarchar(30),
	[Email] nvarchar(50), 
	[Phone] nvarchar(20),
	[Address] nvarchar(max),
	[Gender] nvarchar(50),
	[BirthDate] datetime,
	[IdAdminAccount] int not null

	constraint UC_Admin_Phone unique ([Phone]),
	constraint UC_Admin_Email unique ([Email]),
	constraint FK_Account_IdAdminAccount foreign key ([IdAdminAccount]) 
		references [Account]([Id]),
	constraint CK_Admin_Name check([Name] <> N'' and [Name] is not null),
	constraint CK_Admin_Surname check([Surname] <> N'' and [Surname] is not null),
	constraint CK_Admin_Middlename check([Middlename] <> N'' and [Middlename] is not null),
	constraint CK_Admin_Email check([Email] like N'%@%')

);

create table [dbo].[Patient](
	[Id] int primary key identity,
	[Name] nvarchar(30) ,
	[Surname] nvarchar(30),
	[Middlename] nvarchar(30),
	[Email] nvarchar(50), 
	[Phone] nvarchar(20),
	[Address] nvarchar(max),
	[Gender] nvarchar(50),
	[BirthDate] datetime,
	[IdPatientAccount] int not null

	constraint UC_Patient_Phone unique ([Phone]),
	constraint UC_Patient_Email unique ([Email]),
	constraint FK_Account_IdPatientAccount foreign key ([IdPatientAccount]) 
		references [Account]([Id]),
	constraint CK_Patient_Name check([Name] <> N'' and [Name] is not null),
	constraint CK_Patient_Surname check([Surname] <> N'' and [Surname] is not null),
	constraint CK_Patient_Middlename check([Middlename] <> N'' and [Middlename] is not null),
	constraint CK_Patient_Email check([Email] like N'%@%')

);

create table [dbo].[Doctor](
	[Id] int primary key identity,
	[Name] nvarchar(30),
	[Surname] nvarchar(30),
	[Middlename] nvarchar(30),
	[Email] nvarchar(50), 
	[Phone] nvarchar(20),
	[Address] nvarchar(max),
	[Gender] nvarchar(50),
	[BirthDate] datetime,
	[InterestRate] float,
	[ScheduleDailyBegin] datetime,
	[ScheduleDailyEnd] datetime,
	[ScheduleWeeklyBegin] datetime,
	[ScheduleWeeklyEnd] datetime,
	[ScheduleRestBegin] datetime,
	[ScheduleRestEnd] datetime,
	[IdDoctorAccount] int not null,
	[IdDoctorCategory] int not null,
	[IdDoctorCabinet] int not null

	constraint UC_Doctor_Number unique ([Phone]),
	constraint UC_Doctor_Email unique ([Email]),
	constraint FK_Account_IdDoctorAccount foreign key ([IdDoctorAccount]) 
		references [Account]([Id]),
	constraint FK_Category_IdDoctorCategory foreign key ([IdDoctorCategory]) 
		references [Category]([Id]),
	constraint FK_Cabinet_IdDoctorCabinet foreign key ([IdDoctorCabinet]) 
		references [Cabinet]([Id]),
	constraint CK_Doctor_Name check([Name] <> N'' and [Name] is not null),
	constraint CK_Doctor_Surname check([Surname] <> N'' and [Surname] is not null),
	constraint CK_Doctor_Middlename check([Middlename] <> N'' and [Middlename] is not null),
	constraint CK_Doctor_Email check([Email] like N'%@%')
);

create table [dbo].[DoctorPatient](
	[IdDoctor] int not null,
	[IdPatient] int not null

	constraint FK_DoctorPatient_IdDoctor foreign key ([IdDoctor])
		references [Doctor](Id),
	constraint FK_DoctorPatient_IdPatient foreign key ([IdPatient])
		references [Patient](Id)
);

create table [dbo].[DoctorProcedures](
	[IdDoctor] int not null,
	[IdProcedure] int not null

	constraint FK_DoctorProcedures_IdDoctor foreign key ([IdDoctor])
		references [Doctor](Id),
	constraint FK_DoctorProcedures_IdProcedure foreign key ([IdProcedure])
		references [Procedure](Id)
);

create table [dbo].[ProcedureResult](
	[Id] int primary key identity,
	[Recipe] nvarchar(max),
	[ProcudureTimestampBegin] datetime,
	[ProcudureTimestampEnd] datetime
);

create table [dbo].[Analysis](
	[Id] int primary key identity,
	[Name] nvarchar(30),
	[Result] nvarchar(max),
	[Timestamp] datetime,
	[IsDone] bit,
	[IdAnalysisProcedureResult] int

	constraint FK_Analysis_IdAnalysisProcedureResult foreign key ([IdAnalysisProcedureResult])
		references [ProcedureResult](Id),
	constraint CK_Analysis_Name check([Name] <> N'' and [Name] is not null)
);

create table [dbo].[Diagnosis](
	[Id] int primary key identity,
	[Description] nvarchar(max) not null,
	[Timestamp] datetime,
	[IdDiagnosisProcedureResult] int

	constraint FK_Diagnosis_IdDiagnosisProcedureResult foreign key ([IdDiagnosisProcedureResult])
		references [ProcedureResult](Id)
);

create table [dbo].[Record](
	[Id] int primary key identity,
	[Timestamp] datetime,
	[IdRecordDoctor] int not null,
	[IdRecordPatient] int not null,
	[IdRecordProcedure] int not null,
	[IdRecordProcedureResult] int not null

	constraint FK_Doctor_IdRecordDoctor foreign key ([IdRecordDoctor])
		references [Doctor](Id),
	constraint FK_Patient_IdRecordPatient foreign key ([IdRecordPatient])
		references [Patient](Id),
	constraint FK_Procedure_IdRecordProcedure foreign key ([IdRecordProcedure])
		references [Procedure](Id),
	constraint FK_Doctor_IdRecordProcedureResult foreign key ([IdRecordProcedureResult])
		references [ProcedureResult](Id)
);

create table Avatar(
	Id int primary key identity(1, 1),
	[Image] varbinary(max),
	[IdAccount] int not null,
	constraint FK_Avatar_IdAccount foreign key ([IdAccount])
		references [Account](Id)
);

create table Logs(
	Id int primary key identity(1, 1),
	[Message] nvarchar(max),
	[Type] int,
	[Action] int,
	[Timestamp] datetime
);
-- 1. Получение аккаунта админа по его Id в Admin.
-- 2. Получение анализа по Id пациента И доктора , совершившего этот анализ в Analysis.
-- 3. Получение всей экипировки (Equipment) определённого кабинета (Cabinet) по его Id.
-- 4. Получение всех медикаментов (Medicine) определённого кабинета по его Id.
-- 5. Получение всех диагнозов (Diagnosis) по Id пациента и Id доктора, поставившего диагноз.
-- 6. Получение всех докторов по Id кабинета, в котором они находятся.
-- 7. Получение аккаунта доктора по его Id в Doctor.
-- 8. Получение категории (Category) доктора по его Id в Doctor.
-- 9. Получение кабинета доктора по его Id в Doctor.
-- 10. Получение списка пациентов по Id доктора в Doctor.
-- 11. Получение списка процедур, которые может выполнять доктор, по его Id в Doctor.
-- 12. Получение аккаунта пациента по его Id в Patient.
-- 13. Получение всех записей (Record) по Id доктора, который выполнил запись.
-- 14. Получение доктора по его Id в Record.
-- 15. Получение пациента по его Id в Record.
-- 16. Получение процедуры по её Id в Record.
-- 17. Получение результата процедуры (ProcedureResult) по её Id в Record.
-- 18. Получение доктора по Id в Diagnosis.
-- 19. Получение пациента по Id в Diagnosis.

-- 1. Получение аккаунта админа по его Id в Admin.
go
create procedure SelectAccountByAdminId (@admin_id int)
as
begin
	select Account.Id, Type, [Login], [Password]
	from Account
	join [Admin] on [Admin].Id = @admin_id
	where Account.Id = [Admin].IdAdminAccount
end
-- 2. Получение анализа по Id пациента И доктора , совершившего этот анализ в Analysis.
go
create procedure SelectAnalysisByPatientIdAndDoctorId (@patient_id int, @doctor_id int)
as
begin
	select Analysis.Id, Analysis.[Name], Analysis.[Result], Analysis.[Timestamp], 
	Analysis.[IsDone], Analysis.IdAnalysisProcedureResult
	from Analysis
	join ProcedureResult on ProcedureResult.Id = Analysis.IdAnalysisProcedureResult
	join Record on Record.IdRecordProcedureResult = ProcedureResult.Id
	join Doctor on Doctor.Id = Record.IdRecordDoctor
	join Patient on Patient.Id = Record.IdRecordPatient
	where Record.IdRecordPatient = @patient_id
	and Record.IdRecordDoctor = @doctor_id
end

go
-- 3. Получение всей экипировки (Equipment) определённого кабинета (Cabinet) по его Id.
-- PS Добавил группировку, чтобы получать Quantity
create procedure SelectEquipmentByCabinetId (@cabinet_id int)
as
begin
	select Equipment.Id, Equipment.[Name], Equipment.Price, count(Equipment.Id) as Quantity
	from Equipment
	join CabinetEquipment on Equipment.Id = CabinetEquipment.IdEquipment
	join Cabinet on Cabinet.Id = CabinetEquipment.IdCabinet
	where CabinetEquipment.IdCabinet = @cabinet_id
	group by Equipment.Id, Equipment.[Name], Equipment.Price
end

go
-- 4. Получение всех медикаментов (Medicine) определённого кабинета по его Id.
-- также добавил группировку
create procedure SelectMedicineByCabinetId (@cabinet_id int)
as
begin
	select Medicine.Id, Medicine.[Name], Medicine.Price, count(Medicine.Id) as Quantity
	from Medicine
	join CabinetMedicine on Medicine.Id = CabinetMedicine.IdMedicine
	join Cabinet on Cabinet.Id = CabinetMedicine.IdCabinet
	where CabinetMedicine.IdCabinet = @cabinet_id
	group by Medicine.Id, Medicine.[Name], Medicine.Price
end

go
-- 5. Получение всех диагнозов (Diagnosis) по Id пациента и Id доктора, поставившего диагноз.
create procedure SelectDiagnosisByPatientIdAndDoctorId (@patient_id int, @doctor_id int)
as
begin
	select Diagnosis.Id, Diagnosis.[Description], Diagnosis.[Timestamp], Diagnosis.IdDiagnosisProcedureResult
	from Diagnosis
	join ProcedureResult on ProcedureResult.Id = Diagnosis.IdDiagnosisProcedureResult
	join Record on Record.IdRecordProcedureResult = ProcedureResult.Id
	join Doctor on Doctor.Id = Record.IdRecordDoctor
	join Patient on Patient.Id = Record.IdRecordPatient
	where Record.IdRecordPatient = @patient_id
	and Record.IdRecordDoctor = @doctor_id
end

go
-- 6. Получение всех докторов по Id кабинета, в котором они находятся.
create procedure SelectDoctorByCabinetId (@cabinet_id int)
as
begin
	select Doctor.Id, Doctor.[Name], Doctor.[Surname], Doctor.[Middlename], Doctor.[Email], Doctor.[Phone], 
	Doctor.[Address], Doctor.[Gender], Doctor.[BirthDate], Doctor.[InterestRate], 
	Doctor.[ScheduleDailyBegin], Doctor.[ScheduleDailyEnd], 
	Doctor.[ScheduleWeeklyBegin], Doctor.[ScheduleWeeklyEnd], 
	Doctor.[ScheduleRestBegin], Doctor.[ScheduleRestEnd], Doctor.[IdDoctorAccount],
	Doctor.[IdDoctorCategory], Doctor.[IdDoctorCabinet]
	from Doctor
	join [Cabinet] on Doctor.IdDoctorCabinet = Cabinet.Id
	where Cabinet.Id = @cabinet_id
end
 
go
-- 7. Получение аккаунта доктора по его Id в Doctor.
create procedure SelectAccountByDoctorId (@doctor_id int)
as
begin
	select Account.Id, [Type], [Login], [Password]
	from Account
	join Doctor on Doctor.Id = @doctor_id
	where Account.Id = Doctor.IdDoctorAccount
end

go
-- 8. Получение категории (Category) доктора по его Id в Doctor.
create procedure SelectCategoryByDoctorId (@doctor_id int)
as
begin
	select Category.Id, Category.[Value]
	from Category
	join Doctor on Doctor.IdDoctorCategory = Category.Id
	where Doctor.Id = @doctor_id
end
go
-- 9. Получение кабинета доктора по его Id в Doctor.
create procedure SelectCabinetByDoctorId (@doctor_id int)
as
begin
	select Cabinet.Id, Cabinet.Number
	from Cabinet
	join Doctor on Doctor.Id = @doctor_id
	where Cabinet.Id = Doctor.IdDoctorCabinet
end

go
-- 10. Получение списка пациентов по Id доктора в Doctor.
create procedure SelectAllPatientsByDoctorId (@doctor_id int)
as
begin
	select Patient.Id, Patient.[Name], Patient.[Surname], Patient.[Middlename], Patient.[Email], 
	Patient.[Phone], Patient.[Address], Patient.[Gender], Patient.[BirthDate], 
	Patient.[IdPatientAccount]
	from Patient
	join Doctor on Doctor.Id = @doctor_id
	join DoctorPatient on DoctorPatient.IdDoctor = @doctor_id
	where Patient.Id = DoctorPatient.IdPatient
end

go
-- 11. Получение списка процедур, которые может выполнять доктор, по его Id в Doctor.
create procedure SelectAllProceduresByDoctorId (@doctor_id int)
as
begin
	select [Procedure].Id, [Procedure].[Name], [Procedure].[Description], 
	[Procedure].Price, [Procedure].Duration
	from [Procedure]
	join Doctor on Doctor.Id = @doctor_id
	join [DoctorProcedures] on [DoctorProcedures].IdDoctor = @doctor_id
	where [Procedure].Id = [DoctorProcedures].IdProcedure
end

go
-- 12. Получение аккаунта пациента по его Id в Patient.
create procedure SelectAccountByPatientId (@patient_id int)
as
begin
	select Account.Id, [Type], [Login], [Password]
	from Account
	join Patient on Patient.Id = @patient_id
	where Account.Id = Patient.IdPatientAccount
end

go
-- 13. Получение всех записей (Record) по Id доктора, который выполнил запись.
create procedure AllRecordsByDoctorId (@doctor_id int)
as
begin
	select Record.Id, Record.[Timestamp], Record.IdRecordDoctor, Record.IdRecordPatient,
		   Record.IdRecordProcedure, Record.IdRecordProcedureResult
	from Record
	where Record.IdRecordDoctor = @doctor_id
end

go
-- 14. Получение доктора по его Id в Record.
create procedure SelectDoctorByRecordId (@record_id int)
as
begin
	select Doctor.Id, Doctor.[Name], Doctor.[Surname], Doctor.[Middlename], Doctor.[Email], Doctor.[Phone], 
	Doctor.[Address], Doctor.[Gender], Doctor.[BirthDate], Doctor.[InterestRate], 
	Doctor.[ScheduleDailyBegin], Doctor.[ScheduleDailyEnd], 
	Doctor.[ScheduleWeeklyBegin], Doctor.[ScheduleWeeklyEnd], 
	Doctor.[ScheduleRestBegin], Doctor.[ScheduleRestEnd], Doctor.[IdDoctorAccount],
	Doctor.[IdDoctorCategory], Doctor.[IdDoctorCabinet]
	from Doctor
	join Record on Record.Id = @record_id
	where Doctor.Id = Record.IdRecordDoctor
end

go
-- 15. Получение пациента по его Id в Record.
create procedure SelectPatientByRecordId (@record_id int)
as
begin
	select distinct Patient.Id, Patient.[Name], Patient.[Surname], Patient.[Middlename], Patient.[Email], 
	Patient.[Phone], Patient.[Address], Patient.[Gender], Patient.[BirthDate], 
	Patient.[IdPatientAccount]
	from Patient
	join Record on Record.Id = @record_id
	where Patient.Id = Record.IdRecordPatient
end

go
-- 16. Получение процедуры по её Id в Record.
create procedure SelectProcedureByRecordId (@record_id int)
as
begin
	select distinct [Procedure].Id, [Procedure].[Name], [Procedure].[Description], 
	[Procedure].Price, [Procedure].Duration
	from [Procedure]
	join Record on Record.Id = @record_id
	where [Procedure].Id = Record.IdRecordProcedure
end

go
-- 17. Получение результата процедуры (ProcedureResult) по её Id в Record.
create procedure SelectProcedureResultByRecordId (@record_id int)
as
begin
	select distinct [ProcedureResult].Id, [ProcedureResult].[Recipe], [ProcedureResult].[ProcudureTimestampBegin], 
	[ProcedureResult].[ProcudureTimestampEnd]
	from [ProcedureResult]
	join Record on Record.Id = @record_id
	where [ProcedureResult].Id = Record.IdRecordProcedureResult
end

go
-- по Id Record и Doctor получить Analysis
create procedure SelectAnalysisByRecordAndDoctorId(@record_id int, @doctor_id int)
as
begin
	select Analysis.Id, Analysis.[Name], Analysis.[Result], Analysis.[Timestamp], 
	Analysis.[IsDone], Analysis.IdAnalysisProcedureResult
	from Analysis
	join ProcedureResult on ProcedureResult.Id = Analysis.IdAnalysisProcedureResult
	join Record on Record.IdRecordProcedureResult = ProcedureResult.Id
	where Record.Id = @record_id and Record.IdRecordDoctor = @doctor_id
end
go

-- по Id Record и Doctor получить Diagnosis
create procedure SelectDiagnosisByRecordAndDoctorId(@record_id int, @doctor_id int)
as
begin
	select Diagnosis.Id, Diagnosis.[Description], Diagnosis.[Timestamp], Diagnosis.IdDiagnosisProcedureResult
	from Diagnosis
	join ProcedureResult on ProcedureResult.Id = Diagnosis.IdDiagnosisProcedureResult
	join Record on Record.IdRecordProcedureResult = ProcedureResult.Id
	join Doctor on Doctor.Id = Record.IdRecordDoctor
	where Record.Id = @record_id and Record.IdRecordDoctor = @doctor_id
end
go

-- по Id Patient получить Record
create procedure AllRecordsByPatientId (@patient_id int)
as
begin
	select Record.Id, Record.[Timestamp], Record.IdRecordDoctor, Record.IdRecordPatient,
		   Record.IdRecordProcedure, Record.IdRecordProcedureResult
	from Record
	where Record.IdRecordPatient = @patient_id
end
go

-- 20 Select all для всех таблиц
create procedure SelectAllAccount
as
begin
	select *
	from [Account]
end

go
create procedure SelectAllAdmin
as
begin
	select *
	from [Admin]
end

go
create procedure SelectAllAnalysis
as
begin
	select *
	from [Analysis]
end

go
create procedure SelectAllCabinet
as
begin
	select *
	from [Cabinet]
end

go
create procedure SelectAllCategory
as
begin
	select *
	from [Category]
end

go
create procedure SelectAllDiagnosis
as
begin
	select *
	from [Diagnosis]
end

go
create procedure SelectAllDoctor
as
begin
	select *
	from [Doctor]
end

go
create procedure SelectAllDoctorPatient
as
begin
	select *
	from [DoctorPatient]
end

go
create procedure SelectAllDoctorProcedures
as
begin
	select *
	from [DoctorProcedures]
end

go
create procedure SelectAllEquipment
as
begin
	select *
	from [Equipment]
end

go
create procedure SelectAllMedicine
as
begin
	select *
	from [Medicine]
end

go
create procedure SelectAllAPatient
as
begin
	select *
	from [Patient]
end

go
create procedure SelectAllProcedure
as
begin
	select *
	from [Procedure]
end

go
create procedure SelectAllProcedureResult
as
begin
	select *
	from [ProcedureResult]
end

go
create procedure SelectAllRecord
as
begin
	select *
	from [Record]
end

-- 21 Процедуры exists
go
create procedure AccountIdExists(@id int)
as
begin
	if exists(select [Account].[Id] 
			  from [Account] 
			  where [Account].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure AdminIdExists(@id int)
as
begin
	if exists(select [Admin].[Id] 
			  from [Admin] 
			  where [Admin].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure AnalysisIdExists(@id int)
as
begin
	if exists(select [Analysis].[Id] 
			  from [Analysis] 
			  where [Analysis].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure CabinetIdExists(@id int)
as
begin
	if exists(select [Cabinet].[Id] 
			  from [Cabinet] 
			  where [Cabinet].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure CategoryIdExists(@id int)
as
begin
	if exists(select [Category].[Id] 
			  from [Category] 
			  where [Category].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure DiagnosisIdExists(@id int)
as
begin
	if exists(select [Diagnosis].[Id] 
			  from [Diagnosis] 
			  where [Diagnosis].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure DoctorIdExists(@id int)
as
begin
	if exists(select [Doctor].[Id] 
			  from [Doctor] 
			  where [Doctor].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure EquipmentIdExists(@id int)
as
begin
	if exists(select [Equipment].[Id] 
			  from [Equipment] 
			  where [Equipment].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure MedicineIdExists(@id int)
as
begin
	if exists(select [Medicine].[Id] 
			  from [Medicine] 
			  where [Medicine].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure PatientIdExists(@id int)
as
begin
	if exists(select [Patient].[Id] 
			  from [Patient] 
			  where [Patient].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure ProcedureIdExists(@id int)
as
begin
	if exists(select [Procedure].[Id] 
			  from [Procedure] 
			  where [Procedure].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure ProcedureResultIdExists(@id int)
as
begin
	if exists(select [ProcedureResult].[Id] 
			  from [ProcedureResult] 
			  where [ProcedureResult].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

go
create procedure RecordIdExists(@id int)
as
begin
	if exists(select [Record].[Id] 
			  from [Record] 
			  where [Record].[Id] = @id) 
			  begin 
					select 1 
			  end 
	else 
		begin 
			select 0 
		end
end

-- 22 Процедуры удаления
go
create procedure DeleteByAccountId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if exists (select [Admin].IdAdminAccount
					   from Account
					   join [Admin] on [Admin].IdAdminAccount = Account.Id
					   where Account.Id = @id)
				begin
					set @return = 0
					rollback tran
				end		
	
			else if exists (select Doctor.IdDoctorAccount
							from Account
							join Doctor on Doctor.IdDoctorAccount = Account.Id
							where Account.Id = @id)
				begin
					set @return = 0
					rollback tran
				end
	
			else if exists (select Patient.IdPatientAccount
							from Account
							join Patient on Patient.IdPatientAccount = Account.Id
							where Account.Id = @id)
				begin
					set @return = 0
					rollback tran
				end
			else if not exists (select Id
								from Account
								where Account.Id = @id)
				begin
					set @return = 0
					rollback tran
				end
			else
				begin					
					delete from Account
					where Account.Id = @id
					set @return = 1
					commit transaction
				end
	end try
	begin catch
		set @return = 0
		rollback tran
	end catch
	select @return
end
go

create procedure DeleteByAdminId(@id int)
as
begin
	delete from [Admin] 
	where [Admin].[Id] = @id
end
go

create procedure DeleteByAnalysisId(@id int)
as
begin
	delete from [Analysis] 
	where [Analysis].[Id] = @id
end
go

create procedure DeleteByDiagnosisId(@id int)
as
begin
	delete from [Diagnosis] 
	where [Diagnosis].[Id] = @id
end
go

create procedure DeleteByCategoryId
	@id int
as 
begin
	declare @return bit
	begin try
		begin transaction
			if exists (select [Doctor].IdDoctorCategory
					   from Category
					   join Doctor on Doctor.IdDoctorCategory = Category.Id
					   where Category.Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if not exists (select Id
								from Category
								where Category.Id = @id)
							begin
								set @return = 0
								rollback tran
							end
			else 
				begin					
					delete from Category
					where Category.Id = @id
					set @return = 1
					commit transaction
				end

	end try
	begin catch
		set @return = 0
		rollback tran
	end catch
	select @return
end
go

create procedure DeleteByCabinetId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if exists (select Doctor.IdDoctorCabinet
					   from Cabinet
					   join Doctor on Doctor.IdDoctorCabinet = Cabinet.Id
					   where Cabinet.Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if not exists (select Id
								from Cabinet
								where Cabinet.Id = @id)
							begin
								set @return = 0
								rollback tran
							end
			else 
				begin	
					delete from CabinetEquipment
					where CabinetEquipment.IdCabinet = @id 
					delete from CabinetMedicine
					where CabinetMedicine.IdCabinet = @id 
					delete from Cabinet
					where Cabinet.Id = @id
					set @return = 1
					commit transaction
				end
		end try
		begin catch
			set @return = 0
			rollback tran
		end catch
	select @return
end
go

create procedure DeleteByDoctorId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if exists (select Record.IdRecordDoctor
					        from Doctor
						    join Record on Record.IdRecordDoctor = Doctor.Id
					        where Doctor.Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if not exists (select Id
								from Doctor
								where Doctor.Id = @id)
							begin
								set @return = 0
								rollback tran
							end
			else 
				begin	
					delete from DoctorPatient
					where DoctorPatient.IdDoctor = @id 
					delete from DoctorProcedures
					where DoctorProcedures.IdDoctor = @id
					delete from Doctor
					where Doctor.Id = @id
					set @return = 1
					commit transaction
				end
		end try
		begin catch
			set @return = 0
			rollback tran
		end catch
	select @return
end
go

create procedure DeleteByPatientId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if exists (select Record.IdRecordPatient
					        from Patient
						    join Record on Record.IdRecordPatient = Patient.Id
					        where Patient.Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if not exists (select Id
								from Patient
								where Patient.Id = @id)
							begin
								set @return = 0
								rollback tran
							end
			else 
				begin	
					delete from DoctorPatient
					where DoctorPatient.IdPatient = @id
					delete from Patient
					where Patient.Id = @id
					set @return = 1
					commit transaction
				end
		end try
		begin catch
			set @return = 0
			rollback tran
		end catch
	select @return
end
go

create procedure DeleteByEquipmentId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if not exists (select Id
							from Equipment
							where Equipment.Id = @id)
						begin
							set @return = 0
							rollback tran
						end
			else 
				begin	
					delete from CabinetEquipment
					where CabinetEquipment.IdEquipment = @id
					delete from Equipment
					where Equipment.Id = @id
					set @return = 1
					commit transaction
				end
		end try
		begin catch
			set @return = 0
			rollback tran
		end catch
	select @return
end
go

create procedure DeleteByMedicineId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if not exists (select Id
							from Medicine
							where Medicine.Id = @id)
						begin
							set @return = 0
							rollback tran
						end
			else 
				begin	
					delete from CabinetMedicine
					where CabinetMedicine.IdMedicine = @id
					delete from Medicine
					where Medicine.Id = @id
					set @return = 1
					commit transaction
				end
		end try
		begin catch
			set @return = 0
			rollback tran
		end catch
	select @return
end
go

create procedure DeleteByProcedureId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if exists (select Record.IdRecordProcedure
					        from [Procedure]
						    join Record on Record.IdRecordProcedure = [Procedure].Id
					        where [Procedure].Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if not exists (select Id
								from [Procedure]
								where [Procedure].Id = @id)
							begin
								set @return = 0
								rollback tran
							end
			else 
				begin	
					delete from DoctorProcedures
					where DoctorProcedures.IdProcedure = @id
					delete from [Procedure]
					where [Procedure].Id = @id
					set @return = 1
					commit transaction
				end
		end try
		begin catch
			set @return = 0
			rollback tran
		end catch
	select @return
end

go
create procedure DeleteByRecordId(@id int)
as
begin
	delete from [Record] 
	where [Record].[Id] = @id
end

go
create procedure DeleteByProcedureResultId
	@id int
as
begin
	declare @return bit
	begin try
		begin transaction
			if exists (select Record.IdRecordProcedure
					        from ProcedureResult
						    join Record on Record.IdRecordProcedureResult = ProcedureResult.Id
					        where ProcedureResult.Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if exists (select Diagnosis.IdDiagnosisProcedureResult
					        from ProcedureResult
						    join Diagnosis on Diagnosis.IdDiagnosisProcedureResult = ProcedureResult.Id
					        where ProcedureResult.Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if exists (select Analysis.IdAnalysisProcedureResult
					        from ProcedureResult
						    join Analysis on Analysis.IdAnalysisProcedureResult = ProcedureResult.Id
					        where ProcedureResult.Id = @id)
					begin
						set @return = 0
						rollback tran
					end
			else if not exists (select Id
								from ProcedureResult
								where ProcedureResult.Id = @id)
							begin
								set @return = 0
								rollback tran
							end
			else 
				begin
					delete from ProcedureResult
					where ProcedureResult.Id = @id
					set @return = 1
					commit transaction
				end
		end try
		begin catch
			set @return = 0
			rollback tran
		end catch
	select @return
end

-- 23 Процедуры добавления
go
create procedure InsertAccount
	@account_type int,
	@login nvarchar(30),
	@password nvarchar(30)
as
begin
	insert into [Account] values (@account_type, @login, @password)
end

go
create procedure InsertAdmin
	@is_main bit,
	@name nvarchar(30),
	@surname nvarchar(30),
	@middle_name nvarchar(30),
	@email nvarchar(50),
	@phone nvarchar(20),
	@address nvarchar(max),
	@gender nvarchar(50),
	@bith_date datetime,
	@id_admin_account int
as
begin
	insert into [Admin] 
	values (@is_main, @name, @surname ,@middle_name, @email, @phone, 
			@address, @gender, @bith_date, @id_admin_account)
end

go
create procedure InsertAnalysis
	@name nvarchar(30),
	@result nvarchar(max),
	@time_stamp datetime,
	@is_done bit,
	@id_analysis_procedure_result int
as
begin
	insert into [Analysis] 
	values (@name, @result, @time_stamp ,@is_done, 
			@id_analysis_procedure_result)
end

go
create procedure InsertCabinet
	@number int
as
begin
	insert into [Cabinet] 
	values (@number)
end

go
create procedure InsertCabinetEquipment
	@id_cabinet int,
	@id_equipment int
as
begin
	insert into [CabinetEquipment] values (@id_cabinet, @id_equipment)
end

go
create procedure InsertCabinetMedicine
	@id_cabinet int,
	@id_medicine int
as
begin
	insert into [CabinetMedicine] values (@id_cabinet, @id_medicine)
end

go
create procedure InsertCategory
	@value nvarchar(30)
as
begin
	insert into [Category] values (@value)
end

go
create procedure InsertDiagnosis
	@description nvarchar(max),
	@time_stamp datetime,
	@id_diagnosis_procedure_result int
as
begin
	insert into [Diagnosis] 
	values (@description, @time_stamp, @id_diagnosis_procedure_result)
end

go
create procedure InsertDoctor
	@name nvarchar(30),
	@surname nvarchar(30),
	@middle_name nvarchar(30),
	@email nvarchar(50),
	@phone nvarchar(20),
	@address nvarchar(max),
	@gender nvarchar(50),
	@bith_date datetime,
	@interest_rate float,
	@schedule_daily_begin datetime,
	@schedule_daily_end datetime,
	@schedule_weekly_begin datetime,
	@schedule_weekly_end datetime,
	@schedule_rest_begin datetime,
	@schedule_rest_end datetime,
	@id_doctor_account int,
	@id_doctor_category int,
	@id_doctor_cabinet int
as
begin
	insert into [Doctor] 
	values (@name, @surname, @middle_name, @email, @phone, @address, 
			@gender, @bith_date, @interest_rate, 
			@schedule_daily_begin, @schedule_daily_end, 
			@schedule_weekly_begin, @schedule_weekly_end,
			@schedule_rest_begin, @schedule_rest_end,
			@id_doctor_account, @id_doctor_category, @id_doctor_cabinet)
end

go
create procedure InsertDoctorPatient
	@id_doctor int,
	@id_patient int
as
begin
	insert into [DoctorPatient] values (@id_doctor, @id_patient)
end

go
create procedure InsertDoctorProcedures
	@id_doctor int,
	@id_procedure int
as
begin
	insert into [DoctorProcedures] values (@id_doctor, @id_procedure)
end

go
create procedure InsertEquipment
	@name nvarchar(30),
	@price float,
	@quantity int
as
begin
	insert into [Equipment] values (@name, @price, @quantity)
end

go
create procedure InsertMedicine
	@name nvarchar(30),
	@price float,
	@doses int,
	@quantity int
as
begin
	insert into [Medicine] values (@name, @price, @doses, @quantity)
end

go
create procedure InsertPatient
	@name nvarchar(30),
	@surname nvarchar(30),
	@middle_name nvarchar(30),
	@email nvarchar(50),
	@phone nvarchar(20),
	@address nvarchar(max),
	@gender nvarchar(50),
	@bith_date datetime,
	@id_patient_account int
as
begin
	insert into [Patient] 
	values (@name, @surname, @middle_name, @email, @phone, @address, 
			@gender, @bith_date, @id_patient_account)
end

go
create procedure InsertProcedure
	@name nvarchar(30),
	@description nvarchar(max),
	@price float,
	@duration int
as
begin
	insert into [Procedure] values (@name, @description, @price, @duration)
end

go
create procedure InsertProcedureResult
	@recipe nvarchar(max),
	@procudure_timestamp_begin datetime,
	@procudure_timestamp_end datetime
as
begin
	insert into [ProcedureResult] 
	values (@recipe, @procudure_timestamp_begin, @procudure_timestamp_end)
end

go
create procedure InsertRecord
	@timestamp datetime,
	@id_record_doctor int,
	@id_record_patient int,
	@id_record_procedure int,
	@id_record_procedure_reult int
as
begin
	insert into [Record] 
	values (@timestamp, @id_record_doctor, @id_record_patient,
			@id_record_procedure, @id_record_procedure_reult)
end

-- 24 Процедура, которая по логину и пассворду возвращает аккаунт
go
create procedure AccountByLoginAndPassword
	@login nvarchar(30),
	@password nvarchar(30)
as
begin
	select Account.Id, [Type], [Login], [Password]
	from Account
	where [Login] = @login and [Password] = @password
end

-- 25 Процедуры выбора по Id
go
create procedure SelectAccountById(@id int)
as
begin
	select * 
	from [Account] where [Account].[Id] = @id
end

go
create procedure SelectAdminById(@id int)
as
begin
	select * 
	from [Admin] where [Admin].[Id] = @id
end

go
create procedure SelectAnalysisById(@id int)
as
begin
	select * 
	from [Analysis] where [Analysis].[Id] = @id
end

go
create procedure SelectCabinetById(@id int)
as
begin
	select * 
	from [Cabinet] where [Cabinet].[Id] = @id
end

go
create procedure SelectCategoryById(@id int)
as
begin
	select * 
	from [Category] where [Category].[Id] = @id
end

go
create procedure SelectDiagnosisById(@id int)
as
begin
	select * 
	from [Diagnosis] where [Diagnosis].[Id] = @id
end

go
create procedure SelectDoctorById(@id int)
as
begin
	select * 
	from [Doctor] where [Doctor].[Id] = @id
end

go
create procedure SelectEquipmentById(@id int)
as
begin
	select * 
	from [Equipment] where [Equipment].[Id] = @id
end

go
create procedure SelectMedicineById(@id int)
as
begin
	select * 
	from [Medicine] where [Medicine].[Id] = @id
end

go
create procedure SelectPatientById(@id int)
as
begin
	select * 
	from [Patient] where [Patient].[Id] = @id
end

go
create procedure SelectProcedureById(@id int)
as
begin
	select * 
	from [Procedure] where [Procedure].[Id] = @id
end

go
create procedure SelectProcedureResultById(@id int)
as
begin
	select * 
	from [ProcedureResult] where [ProcedureResult].[Id] = @id
end

go
create procedure SelectRecordById(@id int)
as
begin
	select * 
	from [Record] where [Record].[Id] = @id
end

-- 26 Процедуры обновления
go
create procedure UpdateAccountById
	@id int,
	@account_type int,
	@login nvarchar(30),
	@password nvarchar(30)
as
begin
	update [Account] 
	set [Account].[Type] = @account_type, 
		[Account].[Login] = @login, 
		[Account].[Password] = @password 
	where [Account].[Id] = @id
end

go
create procedure UpdateAdminById
	@id int,
	@is_main bit,
	@name nvarchar(30),
	@surname nvarchar(30),
	@middle_name nvarchar(30),
	@email nvarchar(50),
	@phone nvarchar(20),
	@address nvarchar(max),
	@gender nvarchar(50),
	@bith_date datetime,
	@id_admin_account int
as
begin
	update [Admin] 
	set [Admin].IsMain = @is_main,
		[Admin].[Name] = @name, 
		[Admin].[Surname] = @surname,
		[Admin].[Middlename] = @middle_name,
		[Admin].[Email] = @email, 
		[Admin].[Phone] = @phone,
		[Admin].[Address] = @address,
		[Admin].[Gender] = @gender, 
		[Admin].[BirthDate] = @bith_date,
		[Admin].[IdAdminAccount] = @id_admin_account
	where [Admin].[Id] = @id
end

go
create procedure UpdateAnalysisById
	@id int,
	@name nvarchar(30),
	@result nvarchar(max),
	@time_stamp datetime,
	@is_done bit,
	@id_analysis_procedure_result int
as
begin
	update [Analysis] 
	set [Analysis].[Name] = @name, 
		[Analysis].[Result] = @result, 
		[Analysis].[Timestamp] = @time_stamp,
		[Analysis].[IsDone] = @is_done,
		[Analysis].[IdAnalysisProcedureResult] = @id_analysis_procedure_result
	where [Analysis].[Id] = @id
end

go
create procedure UpdateCabinetById
	@id int,
	@number int
as
begin
	update [Cabinet] 
	set [Cabinet].[Number] = @number
	where [Cabinet].[Id] = @id
end

go
create procedure UpdateCategoryById
	@id int,
	@value nvarchar(30)
as
begin
	update [Category] 
	set [Category].[Value] = @value
	where [Category].[Id] = @id
end

go
create procedure UpdateDiagnosisById
	@id int,
	@description nvarchar(max),
	@time_stamp datetime,
	@id_diagnosis_procedure_result int
as
begin
	update [Diagnosis] 
	set [Diagnosis].[Description] = @description, 
		[Diagnosis].[Timestamp] = @time_stamp,
		[Diagnosis].[IdDiagnosisProcedureResult] = @id_diagnosis_procedure_result
	where [Diagnosis].[Id] = @id
end

go
create procedure UpdateDoctorById
	@id int,
	@name nvarchar(30),
	@surname nvarchar(30),
	@middle_name nvarchar(30),
	@email nvarchar(50),
	@phone nvarchar(20),
	@address nvarchar(max),
	@gender nvarchar(50),
	@bith_date datetime,
	@interest_rate float,
	@schedule_daily_begin datetime,
	@schedule_daily_end datetime,
	@schedule_weekly_begin datetime,
	@schedule_weekly_end datetime,
	@schedule_rest_begin datetime,
	@schedule_rest_end datetime,
	@id_doctor_account int,
	@id_doctor_category int,
	@id_doctor_cabinet int
as
begin
	update [Doctor] 
	set [Doctor].[Name] = @name,
		[Doctor].Surname = @surname,
		[Doctor].Middlename = @middle_name,
		[Doctor].Email = @email,
		[Doctor].Phone = @phone,
		[Doctor].[Address] = @address,
		[Doctor].Gender = @gender,
		[Doctor].BirthDate = @bith_date,
		[Doctor].InterestRate = @interest_rate,
		[Doctor].ScheduleDailyBegin = @schedule_daily_begin,
		[Doctor].ScheduleDailyEnd = @schedule_daily_end,
		[Doctor].ScheduleWeeklyBegin = @schedule_weekly_begin,
		[Doctor].ScheduleWeeklyEnd = @schedule_weekly_end,
		[Doctor].ScheduleRestBegin = @schedule_rest_begin,
		[Doctor].ScheduleRestEnd = @schedule_rest_end,
		[Doctor].IdDoctorAccount = @id_doctor_account,
		[Doctor].IdDoctorCategory = @id_doctor_category,
		[Doctor].IdDoctorCabinet = @id_doctor_cabinet
	where [Doctor].[Id] = @id
end

go
create procedure UpdateEquipmentById
	@id int,
	@name nvarchar(30),
	@price float,
	@quantity int
as
begin
	update [Equipment] 
	set [Equipment].[Name] = @name, 
		[Equipment].[Price] = @price,
		[Equipment].[Quantity] = @quantity
	where [Equipment].[Id] = @id
end

go
create procedure UpdateMedicineById
	@id int,
	@name nvarchar(30),
	@price float,
	@doses int,
	@quantity int
as
begin
	update Medicine 
	set Medicine.[Name] = @name, 
		Medicine.[Price] = @price,
		Medicine.Doses = @doses,
		Medicine.[Quantity] = @quantity
	where Medicine.[Id] = @id
end

go
create procedure UpdatePatientById
	@id int,
	@name nvarchar(30),
	@surname nvarchar(30),
	@middle_name nvarchar(30),
	@email nvarchar(50),
	@phone nvarchar(20),
	@address nvarchar(max),
	@gender nvarchar(50),
	@bith_date datetime,
	@id_patient_account int
as
begin
	update Patient 
	set Patient.[Name] = @name,
		Patient.Surname = @surname,
		Patient.Middlename = @middle_name,
		Patient.Email = @email,
		Patient.Phone = @phone,
		Patient.[Address] = @address,
		Patient.Gender = @gender,
		Patient.BirthDate = @bith_date, 
		Patient.IdPatientAccount = @id_patient_account
	where Patient.[Id] = @id
end

go
create procedure UpdateProcedureById
	@id int,
	@name nvarchar(30),
	@description nvarchar(max),
	@price float,
	@duration int
as
begin
	update [Procedure] 
	set [Procedure].[Name] = @name, 
		[Procedure].[Description] = @description, 
		[Procedure].[Price] = @price,
		[Procedure].[Duration] = @duration 
	where [Procedure].[Id] = @id
end

go
create procedure UpdateProcedureResultById
	@id int,
	@recipe nvarchar(max),
	@procudure_timestamp_begin datetime,
	@procudure_timestamp_end datetime
as
begin
	update [ProcedureResult] 
	set [ProcedureResult].Recipe = @recipe, 
		[ProcedureResult].ProcudureTimestampBegin = @procudure_timestamp_begin, 
		[ProcedureResult].ProcudureTimestampEnd = @procudure_timestamp_end
	where [ProcedureResult].[Id] = @id
end

go
create procedure UpdateRecordById
	@id int,
	@timestamp datetime,
	@id_record_doctor int,
	@id_record_patient int,
	@id_record_procedure int,
	@id_record_procedure_reult int
as
begin
	update [Record] 
	set [Record].[Timestamp] = @timestamp, 
		[Record].IdRecordDoctor = @id_record_doctor, 
		[Record].IdRecordPatient = @id_record_patient,
		[Record].IdRecordProcedure = @id_record_procedure, 
		[Record].IdRecordProcedureResult = @id_record_procedure_reult 
	where [Record].[Id] = @id
end
go
--процедура для получения кол-ва аккаунтов типа админ в базе
create procedure CountAdminAccounts
as
begin
	select count(Id)
	from Account
	where [Type] = 1
end
go
--получение последнего айди аккаунта и админа
create procedure LastAccountId
as
begin
	select top(1) Id
	from Account
	order by Id desc
end
go
create procedure LastAdminId
as
begin	
	select top(1) Id
	from [Admin]
	order by Id desc
end
--получение аватара по id аккаунта
go
create procedure GetAvatarByAccountId
@account_id int
as
begin
	select Avatar.Id, Avatar.[Image], Avatar.[IdAccount]
	from Avatar
	join Account on Account.Id = Avatar.[IdAccount]
end
--загрузка аватара
go
create procedure UploadAvatar
@account_id int,
@image varbinary(max)
as
begin
	if (exists(select Id from Avatar where Avatar.[IdAccount] = @account_id))
		begin
			update Avatar
			set Avatar.[Image] = @image
			where Avatar.[IdAccount] = @account_id
		end
		else begin
			insert into Avatar
			values (@image, @account_id)
		end
	
end
go
--получение последнего айди доктора
create procedure LastDoctorId
as
begin
	select top(1) Id
	from Doctor
	order by Id desc
end
go
--получение последнего айди доктора
create procedure LastPatientId
as
begin
	select top(1) Id
	from Patient
	order by Id desc
end
go
create procedure LastProcedureResultId
as
begin
	select top(1) Id
	from ProcedureResult
	order by Id desc
end
-- процедура для вставки лога в таблицу
go
create procedure InsertLog
@message nvarchar(max),
@log_type int,
@action_type int,
@timestamp datetime
as
begin
	insert into Logs values(@message, @log_type, @action_type, @timestamp)
end
go
-- получение ВСЕХ логов
create procedure SelectAllLogs
as
begin
	select *
	from Logs
end
go

-- Проверка Account
create procedure CheckAccount
	@login nvarchar(30)
as
begin
	if exists(select *
			  from Account 
			  where [Login] = @login)
		select 0
	else
		select 1
end
go

-- Проверка Admin
create procedure CheckAdmin
	@phone nvarchar(20),
	@email nvarchar(50)
as
begin
	if exists(select *
			  from [Admin] 
			  where [Phone] = @phone 
			  or [Email] = @email)
		select 0
	else
		select 1
end
go

-- Проверка Analysis
create procedure CheckAnalysis
	@name nvarchar(30),
	@result nvarchar(max),
	@timestamp datetime,
	@is_done bit,
	@id_analysis_procedure_result int
as
begin
	if exists(select * 
			  from [Analysis] 
			  where [Name] = @name 
			  and Result = @result
			  and [Timestamp] = @timestamp
			  and IsDone = @is_done
			  and IdAnalysisProcedureResult = @id_analysis_procedure_result)
		select 0
	else
		select 1
end
go

-- Проверка Cabinet
create procedure CheckCabinet
	@number int
as
begin
	if exists(select *
			  from Cabinet
			  where [Number] = @number)
		select 0
	else
		select 1
end
go

-- Проверка Category
create procedure CheckCategory
	@value nvarchar(30)
as
begin
	if exists(select *
			  from Category
			  where [Value] = @value)
		select 0
	else
		select 1
end
go

-- Проверка Diagnosis
create procedure CheckDiagnosis
	@description nvarchar(max),
	@timestamp datetime,
	@id_diagnosis_procedure_result int
as
begin
	if exists(select * 
			  from [Diagnosis] 
			  where [Description] = @description 
			  and [Timestamp] = @timestamp
			  and IdDiagnosisProcedureResult = @id_diagnosis_procedure_result)
		select 0
	else
		select 1
end
go

-- Проверка Doctor
create procedure CheckDoctor
	@phone nvarchar(20),
	@email nvarchar(50)
as
begin
	if exists(select *
			  from [Doctor] 
			  where [Phone] = @phone 
			  or [Email] = @email)
		select 0
	else
		select 1
end
go

-- Проверка Equipment
create procedure CheckEquipment
	@name nvarchar(30),
	@price float,
	@quantity int
as
begin
	if exists(select *
			  from [Equipment] 
			  where [Name] = @name 
			  and [Price] = @price
			  and [Quantity] = @quantity)
		select 0
	else
		select 1
end
go

-- Проверка Medicine
create procedure CheckMedicine
	@name nvarchar(30),
	@price float,
	@doses int,
	@quantity int
as
begin
	if exists(select *
			  from [Medicine] 
			  where [Name] = @name 
			  and [Price] = @price
			  and [Doses] = @doses
			  and [Quantity] = @quantity)
		select 0
	else
		select 1
end
go

-- Проверка Patient
create procedure CheckPatient
	@phone nvarchar(20),
	@email nvarchar(50)
as
begin
	if exists(select *
			  from [Patient] 
			  where [Phone] = @phone 
			  or [Email] = @email)
		select 0
	else
		select 1
end
go

-- Проверка Procedure
create procedure CheckProcedure
	@name nvarchar(30),
	@description nvarchar(max),
	@price float,
	@duration int
as
begin
	if exists(select *
			  from [Procedure] 
			  where [Name] = @name 
			  and [Description] = @description
			  and [Price] = @price
			  and [Duration] = @duration)
		select 0
	else
		select 1
end
go

-- Проверка ProcedureResult
create procedure CheckProcedureResult
	@recipe nvarchar(max),
	@procudure_timestamp_begin datetime,
	@procudure_timestamp_end datetime
as
begin
	if exists(select *
			  from [ProcedureResult] 
			  where Recipe = @recipe 
			  and ProcudureTimestampBegin = @procudure_timestamp_begin
			  and ProcudureTimestampEnd = @procudure_timestamp_end)
		select 0
	else
		select 1
end
go

-- Проверка Record
create procedure CheckRecord
	@timestamp datetime,
	@id_record_doctor int,
	@id_record_patient int,
	@id_record_procedure int,
	@id_record_procedure_result int
as
begin
	if exists(select *
			  from [Record] 
			  where [Timestamp] = @timestamp 
			  and IdRecordDoctor = @id_record_doctor
			  and IdRecordPatient = @id_record_patient
			  and IdRecordProcedure = @id_record_procedure
			  and IdRecordProcedureResult = @id_record_procedure_result)
		select 0
	else
		select 1
end
go
--получение последнего айди диагноза и анализа
create procedure LastDiagnosisId
as
begin
	select top(1) Id
	from Diagnosis
	order by Id desc
end
go
create procedure LastAnalysisId
as
begin
	select top(1) Id
	from Analysis
	order by Id desc
end