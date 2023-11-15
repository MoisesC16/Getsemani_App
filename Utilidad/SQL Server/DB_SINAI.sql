create database DB_SINAI

GO

USE DB_SINAI

GO

CREATE TABLE  CATEGORIA(
IdCategoria int primary key identity,
Descripcion varchar(50),
Estado bit default 1,
FechaCreacion datetime default getdate()
)

go

CREATE TABLE  EDITORIAL(
IdEditorial int primary key identity,
Descripcion varchar(50),
Estado bit default 1,
FechaCreacion datetime default getdate()
)

go


CREATE TABLE  AUTOR(
IdAutor int primary key identity,
Descripcion varchar(50),
Estado bit default 1,
FechaCreacion datetime default getdate()
)

go

CREATE TABLE LIBRO(
IdLibro int primary key identity,
Titulo varchar(100),
RutaPortada varchar(100),
NombrePortada varchar(100),
IdAutor int references AUTOR(IdAutor),
IdCategoria int references CATEGORIA(IdCategoria),
IdEditorial int references EDITORIAL(IdEditorial),
Ubicacion varchar(50),
Ejemplares int,
Estado bit default 1,
FechaCreacion datetime default getdate()
)

GO

CREATE TABLE TIPO_USUARIO(
IdTipoUsuario  int primary key,
Descripcion varchar(50),
Estado bit default 1,
FechaCreacion datetime default getdate()
)

GO

CREATE TABLE USUARIO(
IdUsuario int primary key identity,
Nombre varchar(50),
Apellido varchar(50),
Correo varchar(50),
Clave varchar(50),
Codigo varchar(50),
IdTipoUsuario int references TIPO_USUARIO(IdTipoUsuario),
Estado bit default 1,
FechaCreacion datetime default getdate()
)

go

CREATE TABLE ESTADO_VENTA(
IdEstadoVenta int primary key,
Descripcion varchar(50),
Estado bit default 1,
FechaCreacion datetime default getdate()
)
GO

CREATE TABLE VENTA(
IdVenta int primary key identity,
IdEstadoVenta int references ESTADO_VENTA(IdEstadoVenta),
IdUsuario int references USUARIO(IdUsuario),
IdLibro int references Libro(IdLibro),
FechaVenta datetime,
FechaConfirmacionVenta datetime,
EstadoEntregado varchar(100),
EstadoRecibido varchar(100),
Estado bit default 1,
FechaCreacion datetime default getdate()
)

--PROCEDIMIENTO PARA GUARDAR CATEGORIA
create procedure sp_RegistrarCategoria(
@Descripcion varchar(50),
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)

		insert into CATEGORIA(Descripcion) values (
		@Descripcion
		)
	ELSE
		SET @Resultado = 0
	
end


go

--PROCEDIMIENTO PARA MODIFICAR CATEGORIA
create procedure sp_ModificarCategoria(
@IdCategoria int,
@Descripcion varchar(60),
@Estado bit,
@Resultado bit output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion =@Descripcion and IdCategoria != @IdCategoria)
		
		update CATEGORIA set 
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdCategoria = @IdCategoria
	ELSE
		SET @Resultado = 0

end

GO


--PROCEDIMIENTO PARA GUARDAR EDITORIAL
CREATE PROC sp_RegistrarEditorial(
@Descripcion varchar(50),
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM EDITORIAL WHERE Descripcion = @Descripcion)

		insert into EDITORIAL(Descripcion) values (
		@Descripcion
		)
	ELSE
		SET @Resultado = 0
	
end


go

--PROCEDIMIENTO PARA MODIFICAR EDITORIAL
create procedure sp_ModificarEditorial(
@IdEditorial int,
@Descripcion varchar(60),
@Estado bit,
@Resultado bit output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM EDITORIAL WHERE Descripcion =@Descripcion and IdEditorial != @IdEditorial)
		
		update EDITORIAL set 
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdEditorial = @IdEditorial
	ELSE
		SET @Resultado = 0

end


GO


--PROCEDIMIENTO PARA GUARDAR AUTOR
CREATE PROC sp_RegistrarAutor(
@Descripcion varchar(50),
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM AUTOR WHERE Descripcion = @Descripcion)

		insert into AUTOR(Descripcion) values (
		@Descripcion
		)
	ELSE
		SET @Resultado = 0
	
end


go

--PROCEDIMIENTO PARA MODIFICAR AUTOR
create procedure sp_ModificarAutor(
@IdAutor int,
@Descripcion varchar(60),
@Estado bit,
@Resultado bit output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM AUTOR WHERE Descripcion =@Descripcion and IdAutor != @IdAutor)
		
		update AUTOR set 
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdAutor = @IdAutor
	ELSE
		SET @Resultado = 0

end


go

create proc sp_registrarLibro(
@Titulo varchar(100),
@RutaPortada varchar(100),
@NombrePortada varchar(100),
@IdAutor int,
@IdCategoria int,
@IdEditorial int,
@Ubicacion varchar(100),
@Ejemplares int,
@Resultado int output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM LIBRO WHERE Titulo = @Titulo)
	begin
		insert into LIBRO(Titulo,RutaPortada,NombrePortada,IdAutor,IdCategoria,IdEditorial,Ubicacion,Ejemplares) values (
		@Titulo,@RutaPortada,@NombrePortada,@IdAutor,@IdCategoria,@IdEditorial,@Ubicacion,@Ejemplares)

		SET @Resultado = scope_identity()
	end
end

go

create proc sp_modificarLibro(
@IdLibro int,
@Titulo varchar(100),
@IdAutor int,
@IdCategoria int,
@IdEditorial int,
@Ubicacion varchar(100),
@Ejemplares int,
@Estado bit,
@Resultado bit output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM LIBRO WHERE Titulo = @Titulo and IdLibro != @IdLibro)
	begin

		update LIBRO set 
		Titulo = @Titulo,
		IdAutor = @IdAutor,
		IdCategoria = @IdCategoria,
		IdEditorial = @IdEditorial,
		Ubicacion = @Ubicacion,
		Ejemplares = @Ejemplares,
		Estado = @Estado
		where IdLibro = @IdLibro

		SET @Resultado = 1
	end
end

GO

create proc sp_actualizarRutaImagen(
@IdLibro int,
@NombrePortada varchar(500)
)
as
begin
	update libro set NombrePortada = @NombrePortada where IdLibro = @IdLibro
end



GO

CREATE FUNCTION fn_obtenercorrelativo(@numero int)

RETURNS varchar(100)
AS
BEGIN
	DECLARE @correlativo varchar(100)

	set @correlativo = 'LE' + RIGHT('000000' + CAST(@numero AS varchar), 6)

	RETURN @correlativo

END

GO

--PROCEDIMIENTO PARA REGISTRAR USUARIO
create PROC sp_RegistrarUsuario(
@Nombre varchar(50),
@Apellido varchar(50),
@Correo varchar(50),
@Clave varchar(50),
@IdTipoUsuario int,
@Resultado bit output
)as
begin
	SET @Resultado = 1
	DECLARE @IDUSUARIO INT 
	IF NOT EXISTS (SELECT * FROM usuario WHERE correo = @Correo)
	begin
		insert into usuario(Nombre,Apellido,Correo,Clave,IdTipoUsuario) values (
		@Nombre,@Apellido,@Correo,@Clave,@IdTipoUsuario)

		SET @IDUSUARIO = SCOPE_IDENTITY()
		print @IDUSUARIO
		if(@IdTipoUsuario = 3)
		begin
			print 'si es igual'
			UPDATE USUARIO SET 
			Codigo = dbo.fn_obtenercorrelativo(@IDUSUARIO),
			Clave = dbo.fn_obtenercorrelativo(@IDUSUARIO)
			WHERE IdUsuario = @IDUSUARIO
		end
	end
	ELSE
		SET @Resultado = 0
	
end


go

--PROCEDIMIENTO PARA MODIFICAR Usuario
create procedure sp_ModificarUsuario(
@IdUsuario int,
@Nombre varchar(50),
@Apellido varchar(50),
@Correo varchar(50),
@Clave varchar(50),
@IdTipoUsuario int,
@Estado bit,
@Resultado bit output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM usuario WHERE correo =@Correo and IdUsuario != @IdUsuario)
		
		update USUARIO set 
		Nombre = @Nombre,
		Apellido = @Apellido,
		Correo = @Correo,
		IdTipoUsuario = @IdTipoUsuario,
		Estado = @Estado
		where IdUsuario = @IdUsuario
	ELSE
		SET @Resultado = 0

end

GO

--REGISTRAR VENTA
create PROC sp_RegistrarVenta(
@IdEstadoVenta int,
@IdUsuario int,
@IdLibro int,
@FechaVenta datetime,
@EstadoEntregado varchar(500),
@Resultado bit output
)as
begin
	SET DATEFORMAT dmy; 
	INSERT INTO VENTA(IdEstadoVenta,IdUsuario,IdLibro,FechaVenta,EstadoEntregado)
	values(@IdEstadoVenta,@IdUsuario,@IdLibro,@FechaVenta,@EstadoEntregado)

	SET @Resultado = 1
end


go

--VERIFICAR VENTA
create PROC sp_existeVenta(
@IdUsuario int,
@IdLibro int,
@Resultado bit output
)as
begin
	SET @Resultado = 0

	if(exists(select * from VENTA where IdUsuario = @IdUsuario and IdLibro =@IdLibro ))
	begin
		SET @Resultado = 1
	end
	
end

--INGRESO DE DATOS INICIALES--
go

insert into TIPO_USUARIO(IdTipoUsuario, Descripcion) values
(1,'Administrador'),
(2,'Cliente'),
(3,'Cliente_UV')

GO
insert into USUARIO(nombre,apellido,correo,clave,IdTipoUsuario) values
('Moises','Cabrera','mcabrera@gmail.com','123',1),
('Victoria','Sura','vsura@gmail.com','456',2)



go


INSERT INTO ESTADO_VENTA(IdEstadoVenta,Descripcion) VALUES
(1,'RESERVA'),
(2,'ENTREGADO')