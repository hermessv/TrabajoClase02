-- Diseño de la base de datos para sistema de parqueo de autos
-- Archivo: diseño-bd.sql
-- SQL Server (T-SQL) compatible version

-- Eliminar tablas si existen (SQL Server 2016+ soporta DROP TABLE IF EXISTS)
DROP TABLE IF EXISTS [PRQ_IngresoAutomoviles];
DROP TABLE IF EXISTS [PRQ_Automoviles];
DROP TABLE IF EXISTS [PRQ_Parqueo];

-- Tabla PRQ_Automoviles
CREATE TABLE [PRQ_Automoviles] (
  [id] INT IDENTITY(1,1) NOT NULL,
  [color] VARCHAR(50) NOT NULL,
  [year] SMALLINT NOT NULL,
  [manufacturer] VARCHAR(100) NOT NULL,
  [type] VARCHAR(20) NOT NULL,
  CONSTRAINT [PK_PRQ_Automoviles] PRIMARY KEY ([id])
);

-- Índices para PRQ_Automoviles
CREATE INDEX [idx_prq_automoviles_type] ON [PRQ_Automoviles]([type]);
CREATE INDEX [idx_prq_automoviles_manufacturer] ON [PRQ_Automoviles]([manufacturer]);

-- Tabla PRQ_Parqueo
CREATE TABLE [PRQ_Parqueo] (
  [id] INT IDENTITY(1,1) NOT NULL,
  [province_name] VARCHAR(100) NOT NULL,
  [name] VARCHAR(150) NOT NULL,
  [price_per_hour] DECIMAL(10,2) NOT NULL DEFAULT(0.00),
  CONSTRAINT [PK_PRQ_Parqueo] PRIMARY KEY ([id])
);

-- Índices para PRQ_Parqueo
CREATE INDEX [idx_prq_parqueo_province] ON [PRQ_Parqueo]([province_name]);
CREATE INDEX [idx_prq_parqueo_name] ON [PRQ_Parqueo]([name]);

-- Tabla PRQ_IngresoAutomoviles
CREATE TABLE [PRQ_IngresoAutomoviles] (
  [consecutive] INT IDENTITY(1,1) NOT NULL,
  [prq_parqueo_id] INT NOT NULL,
  [prq_automovil_id] INT NOT NULL,
  [entry_datetime] DATETIME2 NOT NULL,
  [exit_datetime] DATETIME2 NULL,
  CONSTRAINT [PK_PRQ_IngresoAutomoviles] PRIMARY KEY ([consecutive])
);

-- Índices y claves foráneas para PRQ_IngresoAutomoviles
CREATE INDEX [idx_prq_ingreso_parqueo] ON [PRQ_IngresoAutomoviles]([prq_parqueo_id]);
CREATE INDEX [idx_prq_ingreso_automovil] ON [PRQ_IngresoAutomoviles]([prq_automovil_id]);

ALTER TABLE [PRQ_IngresoAutomoviles]
  ADD CONSTRAINT [fk_prq_ingreso_parqueo] FOREIGN KEY ([prq_parqueo_id]) REFERENCES [PRQ_Parqueo]([id]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE [PRQ_IngresoAutomoviles]
  ADD CONSTRAINT [fk_prq_ingreso_automovil] FOREIGN KEY ([prq_automovil_id]) REFERENCES [PRQ_Automoviles]([id]) ON DELETE CASCADE ON UPDATE CASCADE;

-- Fin del archivo (T-SQL version)
