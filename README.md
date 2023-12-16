1) This project run on http://localhost:38943 in News Folder ther is api project and News.Test there is test project
2) [(http://localhost:38943/swagger)http://localhost:38943/swagger on this url swagger will open  in sagger "/api/Story/GetStories" will come
3) There is only web api fot initially show default record after that search and accoring paging show data.
4) There is 2 project one for web api and other for Test project
5) Below is sp for getting  data and Table script
========================
Create PROCEDURE [dbo].[GetNewList] 
 @Page INT =1,
 @PageSize INT = 10,
 @SearchTerm VARCHAR(255)=''
AS
BEGIN
SET NOCOUNT ON;
SELECT
    Title,Url FROM Story WHERE
    (@SearchTerm IS NULL OR Title LIKE '%' + @SearchTerm + '%')
ORDER BY CreatedAt DESC
OFFSET
    (@Page - 1) * @PageSize ROWS
FETCH NEXT
    @PageSize ROWS ONLY;
   end

6)Table script

CREATE TABLE [dbo].[Story](
	[Title] [varchar](50) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[Url] [nvarchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Story] ADD  CONSTRAINT [DF_Stores_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
