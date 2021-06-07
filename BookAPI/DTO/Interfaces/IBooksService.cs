using System.Collections.Generic;

namespace BookAPI.DTO.Interfaces
{
    public interface IBooksService
    {
        string GetUrl(string bookName);
        string GetResponse(string uri);
        List<string> GetDataFromSearch(string htmlText);
        void BookDownload(List<string> dataBook, int key, string userFormat);
    }
}
