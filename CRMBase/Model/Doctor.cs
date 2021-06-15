using CRMBase.Model.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CRMBase.Model
{
	public class Doctor : Person<Doctor>
	{
		public Doctor() 
		{
			Validator = DoctorValidator.Instance;
			ScheduleDailyBegin = ScheduleDailyEnd = ScheduleRestBegin = ScheduleRestEnd = ScheduleWeeklyBegin = ScheduleWeeklyEnd = DateTime.Now;
		}
		private Account account;
		public Account Account
		{
			get => account;
			set
			{
				account = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Account)));
			}
		}
		private Category category;
		public Category Category
		{
			get => category;
			set
			{
				category = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Category)));
			}
		}
		private Cabinet cabinet;
		public Cabinet Cabinet
		{
			get => cabinet;
			set
			{
				cabinet = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Cabinet)));
			}
		}
		private DateTime? scheduleDailyBegin;
		public DateTime? ScheduleDailyBegin
		{
			get => scheduleDailyBegin;
			set
			{
				scheduleDailyBegin = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(ScheduleDailyBegin)));
			}
		}
		private DateTime? scheduleDailyEnd;
		public DateTime? ScheduleDailyEnd
		{
			get => scheduleDailyEnd;
			set
			{
				scheduleDailyEnd = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(ScheduleDailyEnd)));
			}
		}
		private DateTime? scheduleWeeklyBegin;
		public DateTime? ScheduleWeeklyBegin
		{
			get => scheduleWeeklyBegin;
			set
			{
				scheduleWeeklyBegin = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(ScheduleWeeklyBegin)));
			}
		}
		private DateTime? scheduleWeeklyEnd;
		public DateTime? ScheduleWeeklyEnd
		{
			get => scheduleWeeklyEnd;
			set
			{
				scheduleWeeklyEnd = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(ScheduleWeeklyEnd)));
			}
		}
		private DateTime? scheduleRestBegin;
		public DateTime? ScheduleRestBegin
		{
			get => scheduleRestBegin;
			set
			{
				scheduleRestBegin = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(ScheduleRestBegin)));
			}
		}
		private DateTime? scheduleRestEnd;
		public DateTime? ScheduleRestEnd
		{
			get => scheduleRestEnd;
			set
			{
				scheduleRestEnd = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(ScheduleRestEnd)));
			}
		}
		private float? interestRate;
		public float? InterestRate
		{
			get => interestRate;
			set
			{
				interestRate = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(InterestRate)));
			}
		}
		private HashSet<Patient> patients;
		public HashSet<Patient> Patients
		{
			get => patients;
			set
			{
				patients = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Patients)));
			}
		}
		private HashSet<Procedure> procedures;
		public HashSet<Procedure> Procedures
		{
			get => procedures;
			set
			{
				procedures = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Procedures)));
			}
		}
	}
}