CREATE SCHEMA Tarefas;
GO

CREATE TABLE [Tarefas].[Task](
	[taskID] [int] IDENTITY(1,1) NOT NULL,
	[desTitulo] [varchar](40) NOT NULL,
	[desDescricao] [varchar](100) NOT NULL,
	[indStatus] [varchar](15) NOT NULL,
	[datCriacao] [date] NOT NULL,
	[datUltAlteracao] [date] NULL,
	[datConclusao] [date] NULL,
 CONSTRAINT [PK_Tarefa] PRIMARY KEY CLUSTERED 
(
	[taskID] ASC
));
GO


CREATE PROCEDURE Tarefas.GetTarefas(
	@TaskID int
)
AS
BEGIN
	SET NOCOUNT ON;
	IF @TaskID = 0
	BEGIN
		SELECT t.taskID as TaskID, t.desDescricao as DesDescricao, t.desTitulo as DesTitulo, 
			   t.indStatus as IndStatus, t.datCriacao as DatCriacao, t.datUltAlteracao as DatUltAlteracao, t.datConclusao as DatConclusao
			FROM Tarefas.Task T 
			order by t.indStatus desc;
	end
	else
		SELECT t.taskID as TaskID, t.desDescricao as DesDescricao, t.desTitulo as DesTitulo, 
			   t.indStatus as IndStatus, t.datCriacao as DatCriacao, t.datUltAlteracao as DatUltAlteracao, t.datConclusao as DatConclusao
			FROM Tarefas.Task T 
			where t.taskID = @taskID
			order by t.indStatus desc;
	
END;
GO
GRANT EXECUTE ON Tarefas.GetTarefas TO supero;
GO



CREATE PROCEDURE Tarefas.SetTarefas (
	@TaskID int,
	@DesTitulo varchar(40),
	@DesDescricao VARCHAR(100),
	@IndStatus VARCHAR(15),
	@DatCriacao date,
	@DatUltAlteracao date,
	@DatConclusao date
)
AS
BEGIN
	SET NOCOUNT ON;

	IF @TaskID = 0
	BEGIN
		INSERT Tarefas.Task (desTitulo, desDescricao,indStatus,datCriacao)
		VALUES (@DesTitulo, @DesDescricao, @IndStatus, @DatCriacao);
	END;
	ELSE
		if @IndStatus = 'C'
		BEGIN
			UPDATE Tarefas.Task
			SET desTitulo = @DesTitulo, 
				desDescricao = @DesDescricao, 
				indStatus = @IndStatus, 
				datUltAlteracao = @datUltAlteracao, 
				datConclusao = @datConclusao
			WHERE taskID = @TaskID;
		END
		else
			UPDATE Tarefas.Task
			SET desTitulo = @DesTitulo, 
				desDescricao = @DesDescricao, 
				indStatus = @IndStatus, 
				datUltAlteracao = @datUltAlteracao
			WHERE taskID = @TaskID;
END;
GO
GRANT EXECUTE ON Tarefas.SetTarefas TO supero;
GO

CREATE PROCEDURE Tarefas.DelTarefas (
	@TaskID INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE Tarefas.Task
	WHERE taskID = @TaskID;
		
END;
GO
GRANT EXECUTE ON Tarefas.DelTarefas TO supero;
GO
