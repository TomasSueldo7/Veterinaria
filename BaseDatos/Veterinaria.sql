--Creo la base de datos
CREATE DATABASE Veterinaria;
--Selecciono la base de datos para hacer modificaciones
USE Veterinaria;

CREATE TABLE Empleados(
	Legajo int IDENTITY(1,1) NOT NULL,
	Tipo_doc varchar(10) NOT NULL,
	Nro_doc varchar(10) NOT NULL,
	Nombre varchar(20) NOT NULL,
	Apellido varchar(20) NOT NULL,
	Fecha_nacimiento date NOT NULL,
	Fecha_ingreso date NOT NULL,
	Matricula varchar(10) NOT NULL,
	Fecha_baja date,
	Activo int NOT NULL, 
	CONSTRAINT Legajo_pk PRIMARY KEY (Legajo),
	CONSTRAINT Activo_ck CHECK(Activo > -1 AND Activo < 2)
	);

CREATE TABLE Razas(
	Id_raza int IDENTITY(1,1) NOT NULL,
	Denominacion varchar(20),
	Peso_min_hembra float NOT NULL,
	Peso_max_hembra float,
	Peso_min_macho float,
	Peso_max_macho float,
	Altura_media_hembra float,
	Altura_media_hombre float,
	CONSTRAINT Id_raza_pk PRIMARY KEY (Id_raza),
	);

CREATE TABLE Dueños(
	Id_dueño int IDENTITY(1,1) NOT NULL,
	Nombre varchar(20) NOT NULL,
	Apellido varchar(20) NOT NULL,
	Telefono int,
	CONSTRAINT Id_dueño PRIMARY KEY (Id_dueño),
	);

CREATE TABLE Perros(
	Nro_HC int IDENTITY(1,1) NOT NULL,
	Nombre varchar(10) NOT NULL,
	Fecha_nacimiento date,
	Id_raza int,
	Peso varchar(20) NOT NULL,
	Altura float NOT NULL,
	Id_owner int NOT NULL,
	CONSTRAINT Nro_HC_pk PRIMARY KEY (Nro_HC),
	CONSTRAINT Perros_Id_raza_fk FOREIGN KEY (Id_raza) REFERENCES Razas(Id_raza),
	CONSTRAINT Perros_Id_owner_fk FOREIGN KEY (Id_owner) REFERENCES Dueños(Id_dueño),
	);

CREATE TABLE Vacunas(
	Id_vacuna int IDENTITY(1,1) NOT NULL,
	Nombre varchar(50),
	CONSTRAINT Id_vacuna_pk PRIMARY KEY (Id_vacuna),
	);

CREATE TABLE Sintomas(
	Id_sintoma int,
	Descripcion varchar(100),
	CONSTRAINT Id_sintoma_pk PRIMARY KEY (Id_sintoma),
	);

CREATE TABLE Calendario_Vacunacion(
	Nro_HC int,
	Id_vacuna int,
	Fecha_prevista date,
	Fecha_real date,
	Legajo int,
	Dosis int,
	CONSTRAINT Nro_vacuna_pk PRIMARY KEY (Nro_HC, Id_vacuna),
	CONSTRAINT Calendario_Legajo_fk FOREIGN KEY (Legajo) REFERENCES Empleados(Legajo),
	CONSTRAINT Calendario_Nro_HC_fk FOREIGN KEY (Nro_HC) REFERENCES Perros(Nro_HC),
	CONSTRAINT Calendario_Id_vacuna_fk FOREIGN KEY (Id_vacuna) REFERENCES Vacunas(Id_vacuna),
	);

CREATE TABLE Medicamentos(
	Id_medicamento int IDENTITY(1,1) NOT NULL,
	Nombre varchar(50),
	CONSTRAINT Id_medicamento_pk PRIMARY KEY (Id_medicamento),
	);

CREATE TABLE Consultas(
	Nro_HC int,
	Id_consulta int, --hay que ver como trato el identity por cada perro, no se como lo trata SQL
	Legajo int,
	Fecha_entrada date,
	Fecha_salida date,
	CONSTRAINT Nro_Id_pk PRIMARY KEY (Nro_HC, Id_consulta),
	CONSTRAINT Consultas_Legajo_fk FOREIGN KEY (Legajo) REFERENCES Empleados(Legajo),
	CONSTRAINT Consultas_Nro_HC_fk FOREIGN KEY (Nro_HC) REFERENCES Perros(Nro_HC),
	);

CREATE TABLE SintomasXConsultas(
	Nro_HC int,
	Id_consulta int,
	Id_sintoma int,
	CONSTRAINT Nro_Consulta_Sintoma_pk PRIMARY KEY (Nro_HC, Id_consulta, Id_sintoma),
	CONSTRAINT SintXCons_Nro_HC_Consulta_fk FOREIGN KEY (Nro_HC, Id_consulta) REFERENCES Consultas(Nro_HC,Id_consulta),
	CONSTRAINT SintXCons_Id_Sintoma_fk FOREIGN KEY (Id_sintoma) REFERENCES Sintomas(Id_sintoma),
	);

CREATE TABLE Diagnosticos(
	Id_diagnostico int IDENTITY(1,1) NOT NULL,
	Descripcion varchar(100),
	CONSTRAINT Id_diagnostico PRIMARY KEY (Id_diagnostico),
	);

CREATE TABLE DiagnosticosXConsulta(
	Nro_HC int,
	Id_consulta int,
	Id_diagnostico int,
	CONSTRAINT Nro_Consulta_Diagnostico_pk PRIMARY KEY (Nro_HC, Id_consulta, Id_diagnostico),
	CONSTRAINT DiagXCons_Nro_HC_Consulta_fk FOREIGN KEY (Nro_HC, Id_consulta) REFERENCES Consultas(Nro_HC,Id_consulta),
	CONSTRAINT DiagXCons_Id_Diagnostico_fk FOREIGN KEY (Id_diagnostico) REFERENCES Diagnosticos(Id_diagnostico),
	);

CREATE TABLE MedicamentossXConsulta(
	Id_medicamento int,
	Nro_HC int,
	Id_consulta int,
	Dosis int,
	Periocidad varchar(50),
	CONSTRAINT Nro_Consulta_Medicamento_pk PRIMARY KEY (Nro_HC, Id_consulta, Id_medicamento),
	CONSTRAINT MedicXCons_Nro_HC_Consulta_fk FOREIGN KEY (Nro_HC, Id_consulta) REFERENCES Consultas(Nro_HC,Id_consulta),
	CONSTRAINT MedicXCons_Id_Medicamento_fk FOREIGN KEY (Id_medicamento) REFERENCES Medicamentos(Id_medicamento),
	);

