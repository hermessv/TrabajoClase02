-- Archivo: insertar-registros.sql
-- Inserciones de prueba para el sistema de parqueo
-- Se asumen tablas ya creadas por diseño-bd.sql

SET FOREIGN_KEY_CHECKS = 0;

-- Insertar PRQ_Automoviles (especificando id para facilitar referencias)
INSERT INTO `PRQ_Automoviles` (`id`, `color`, `year`, `manufacturer`, `type`) VALUES
(1, 'Rojo', 2024, 'Toyota', 'sedán'),
(2, 'Azul', 2020, 'Honda', 'sedán'),
(3, 'Negro', 2022, 'Ford', '4x4'),
(4, 'Blanco', 2025, 'Yamaha', 'moto'),
(5, 'Gris', 2019, 'Chevrolet', 'sedán');

-- Insertar PRQ_Parqueo
INSERT INTO `PRQ_Parqueo` (`id`, `province_name`, `name`, `price_per_hour`) VALUES
(1, 'Pichincha', 'Parqueadero Centro', 1.50),
(2, 'Guayas', 'Parqueadero Norte', 2.00);

-- Insertar PRQ_IngresoAutomoviles (15 registros combinando parkings y autos)
-- Usaremos fechas realistas 2024-2025; algunos exit_datetime serán NULL.
INSERT INTO `PRQ_IngresoAutomoviles` (`consecutive`, `prq_parqueo_id`, `prq_automovil_id`, `entry_datetime`, `exit_datetime`) VALUES
(1, 1, 1, '2024-11-01 08:15:00', '2024-11-01 11:30:00'),
(2, 1, 2, '2024-11-02 09:00:00', '2024-11-02 12:45:00'),
(3, 2, 3, '2024-12-10 14:20:00', '2024-12-10 16:00:00'),
(4, 2, 4, '2025-01-05 07:50:00', NULL),
(5, 1, 5, '2025-02-14 18:10:00', '2025-02-14 20:00:00'),
(6, 1, 3, '2025-03-01 10:00:00', NULL),
(7, 2, 2, '2025-03-02 11:30:00', '2025-03-02 13:00:00'),
(8, 1, 4, '2025-03-03 09:45:00', '2025-03-03 10:15:00'),
(9, 2, 1, '2025-03-04 20:00:00', NULL),
(10, 1, 2, '2025-03-05 08:00:00', '2025-03-05 09:30:00'),
(11, 2, 5, '2024-10-20 07:30:00', '2024-10-20 08:00:00'),
(12, 1, 1, '2024-12-31 23:00:00', '2025-01-01 01:00:00'),
(13, 2, 3, '2025-04-10 15:00:00', '2025-04-10 18:00:00'),
(14, 1, 5, '2025-04-15 12:00:00', NULL),
(15, 2, 4, '2025-05-01 06:30:00', '2025-05-01 07:00:00');

SET FOREIGN_KEY_CHECKS = 1;

-- Fin del archivo
