using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BookAPI.DTO.Interfaces;

namespace BookAPI.Controllers
{
    [Route("books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBooksService _booksService; 

        public BookController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet("{bookName}/{key}/{userFormat}")]
        public void Download(string bookName, int key, string userFormat)
        {
            var uri = _booksService.GetUrl(bookName);
            var htmlText = _booksService.GetResponse(uri);
            var dataBook = _booksService.GetDataFromSearch(htmlText);
            _booksService.BookDownload(dataBook, key, userFormat);
        }
    }
}
