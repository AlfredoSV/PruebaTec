CREATE DATABASE PruebaTec;

use DATABASE PruebaTec;


CREATE TABLE diasFestivos( id Uniqueidentifier, dia int, mes int, descripcion varchar(250));

INSERT INTO diasFestivos VALUES(newid(),1,1,'Año nuevo');
INSERT INTO diasFestivos values(newid(),7,2,'Festivo por el día de la Constitución');
INSERT INTO diasFestivos values(newid(),21,3,'Día del natalicio de Benito Juárez');

INSERT INTO diasFestivos VALUES(newid(),1,5,'Día del Trabajo');

INSERT INTO diasFestivos VALUES(newid(),16,9,'Día de la Independencia de México');
INSERT INTO diasFestivos VALUES(newid(),21,11,'Festivo por el día de la Revolución Mexicana');

INSERT INTO diasFestivos VALUES(newid(),25,12,'Navidad');

select * from diasFestivos