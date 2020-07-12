SELECT [SubCategoryId]
      ,[SubCategoryName]
      ,sc.[MainCategoryId]
	  ,mc.[MainCategoryName]
FROM [CascadingDropDownListsDemo].[dbo].[SubCategory] AS sc, [CascadingDropDownListsDemo].[dbo].[MainCategory] AS mc
WHERE sc.MainCategoryId = mc.MainCategoryId

