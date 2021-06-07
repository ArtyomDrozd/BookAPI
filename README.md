# About application
This application for downloading books from **flibusta.site**. For downloading tou must enter _book name_, _key book_ from found books and _book format_.

## Developing
Used .NET CORE | WEB API. 

## URI creating
**GET** /books/{book_name}/{search_number}/{book_format}
where:
* book_name — the name of the book to search
* earch_number — number of the book from the list of those found by keyword 
* book_format — download file format 

## Examples of GET  request 
 ```html
 https://localhost:5001/books/мартин+иден/1/fb2
 ```
 ```html
 https://localhost:5001/books/шантарам/1/fb2
 ```
  ```html
 https://localhost:5001/books/отцы+и+дети/3/epub
 ```
  ```html
 https://localhost:5001/books/Generation+P/4/pdf
 ```
