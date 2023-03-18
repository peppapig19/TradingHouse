# Вариант 7. ТОРГОВЛЯ

Отделы крупного торгового дома ежедневно продают различные виды товаров и ведут учет сведений о проданных товарах.
Необходимо спроектировать базу данных ТОРГОВЛЯ, информация которой будет использоваться для анализа выполнения плана реализации продукции в отделах; определения товаров, пользующихся наибольшим спросом и др.

В БД должна храниться информация:

– об ***отделах***: код отдела, наименование отдела, Ф.И.О. заведующего отделом, телефон, объем реализации в день (руб.);

– ***товарах***: артикул товара, наименование товара, единица измерения, розничная цена товара (руб.);

– ***продажах***: артикул товара, дата продажи, количество проданного товара.

Могут понадобится дополнительные таблицы, для полноты описания предметной области.

При проектировании БД необходимо учитывать следующее:

– отдел ежедневно осуществляет несколько продаж. Каждая продажа имеет отношение только к одному отделу;

– товар участвует в нескольких продажах. Каждая продажа соотносится только с одним товаром.

Кроме того, следует учесть:

– каждый отдел обязательно осуществляет продажу. Каждая продажа обязательно осуществляется отделом;

– товар не обязательно может участвовать в продаже (может быть не востребован). В продаже обязательно участвует товар.