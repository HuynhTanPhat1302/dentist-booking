-- Create the database
CREATE DATABASE DentistBooking;

-- Use the newly created database
USE DentistBooking;

-- Create Illness table
CREATE TABLE Illness (
  IllnessID INT IDENTITY(1,1) PRIMARY KEY,
  IllnessName NVARCHAR(150)
);

-- Create Treatment table
CREATE TABLE Treatment (
  TreatmentID INT IDENTITY(1,1) PRIMARY KEY,
  TreatmentName NVARCHAR(150),
  Price MONEY,
  EstimatedTime FLOAT
);

-- Create Dentist table
CREATE TABLE Dentist (
  DentistID INT IDENTITY(1,1) PRIMARY KEY,
  Email NVARCHAR(255),
  DentistName NVARCHAR(150),
  PhoneNumber NVARCHAR(20)
);

-- Create Patient table
CREATE TABLE Patient (
  PatientID INT IDENTITY(1,1) PRIMARY KEY,
  Email NVARCHAR(255) UNIQUE,
  PatientName NVARCHAR(150),
  PhoneNumber NVARCHAR(20),
  DateOfBirth DATETIME,
  PatientCode NVARCHAR(20),
  Address NVARCHAR(255)
);

-- Create Staff table
CREATE TABLE Staff (
  StaffID INT IDENTITY(1,1) PRIMARY KEY,
  StaffName NVARCHAR(150),
  PhoneNumber NVARCHAR(20),
  Email NVARCHAR(255) UNIQUE
);

-- Create ProposeAppointment table
CREATE TABLE ProposeAppointment (
  ProposeAppointmentID INT IDENTITY(1,1) PRIMARY KEY,
  PatientID INT,
  Datetime DATETIME,
  Name NVARCHAR(150),
  PhoneNumber NVARCHAR(20),
  Note NVARCHAR(MAX),
  Status NVARCHAR(20),
  FOREIGN KEY (PatientID) REFERENCES Patient(PatientID)
);

-- Create MedicalRecord table
CREATE TABLE MedicalRecord (
  MedicalRecordID INT IDENTITY(1,1) PRIMARY KEY,
  PatientID INT,
  DentistID INT,
  TeethNumber INT,
  IllnessID INT,
  TreatmentID INT,
  Status NVARCHAR(20),
  FOREIGN KEY (PatientID) REFERENCES Patient(PatientID),
  FOREIGN KEY (DentistID) REFERENCES Dentist(DentistID),
  FOREIGN KEY (IllnessID) REFERENCES Illness(IllnessID),
  FOREIGN KEY (TreatmentID) REFERENCES Treatment(TreatmentID)
);

-- Create Appointment table
CREATE TABLE Appointment (
  AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
  PatientID INT,
  DentistID INT,
  StaffID INT,
  Datetime DATETIME,
  Duration FLOAT,
  Status NVARCHAR(20),
  FOREIGN KEY (PatientID) REFERENCES Patient(PatientID),
  FOREIGN KEY (DentistID) REFERENCES Dentist(DentistID),
  FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
);

-- Create AppointmentDetail table
CREATE TABLE AppointmentDetail (
  AppointmentDetailID INT IDENTITY(1,1) PRIMARY KEY,
  AppointmentID INT,
  MedicalRecordID INT,
  FOREIGN KEY (AppointmentID) REFERENCES Appointment(AppointmentID),
  FOREIGN KEY (MedicalRecordID) REFERENCES MedicalRecord(MedicalRecordID)
);

-- Create DentistAvailability table
CREATE TABLE DentistAvailability (
  AvailabilityID INT IDENTITY(1,1) PRIMARY KEY,
  DentistID INT,
  DayOfWeek NVARCHAR(20),
  StartTime TIME,
  EndTime TIME,
  FOREIGN KEY (DentistID) REFERENCES Dentist(DentistID)
);
