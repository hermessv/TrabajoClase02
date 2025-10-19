-- Diseño de la base de datos para sistema de parqueo de autos
-- Archivo: diseño-bd.sql
-- MySQL compatible

SET @OLD_FOREIGN_KEY_CHECKS = @@FOREIGN_KEY_CHECKS;
SET FOREIGN_KEY_CHECKS = 0;

-- Tabla PRQ_IngresoAutomoviles se elimina primero por dependencias
DROP TABLE IF EXISTS `PRQ_IngresoAutomoviles`;
DROP TABLE IF EXISTS `PRQ_Automoviles`;
DROP TABLE IF EXISTS `PRQ_Parqueo`;

-- Tabla PRQ_Automoviles
CREATE TABLE `PRQ_Automoviles` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `color` VARCHAR(50) NOT NULL,
  `year` SMALLINT NOT NULL,
  `manufacturer` VARCHAR(100) NOT NULL,
  `type` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `idx_prq_automoviles_type` (`type`),
  INDEX `idx_prq_automoviles_manufacturer` (`manufacturer`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla PRQ_Parqueo
CREATE TABLE `PRQ_Parqueo` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `province_name` VARCHAR(100) NOT NULL,
  `name` VARCHAR(150) NOT NULL,
  `price_per_hour` DECIMAL(10,2) NOT NULL DEFAULT 0.00,
  PRIMARY KEY (`id`),
  INDEX `idx_prq_parqueo_province` (`province_name`),
  INDEX `idx_prq_parqueo_name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla PRQ_IngresoAutomoviles
CREATE TABLE `PRQ_IngresoAutomoviles` (
  `consecutive` INT NOT NULL AUTO_INCREMENT,
  `prq_parqueo_id` INT NOT NULL,
  `prq_automovil_id` INT NOT NULL,
  `entry_datetime` DATETIME NOT NULL,
  `exit_datetime` DATETIME NULL,
  PRIMARY KEY (`consecutive`),
  INDEX `idx_prq_ingreso_parqueo` (`prq_parqueo_id`),
  INDEX `idx_prq_ingreso_automovil` (`prq_automovil_id`),
    CONSTRAINT `fk_prq_ingreso_parqueo` FOREIGN KEY (`prq_parqueo_id`) REFERENCES `PRQ_Parqueo` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `fk_prq_ingreso_automovil` FOREIGN KEY (`prq_automovil_id`) REFERENCES `PRQ_Automoviles` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

SET FOREIGN_KEY_CHECKS = @OLD_FOREIGN_KEY_CHECKS;

-- Fin del archivo
