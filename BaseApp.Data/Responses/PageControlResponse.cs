namespace BaseApp.Data.Responses
{
    public class PageControlResponse
    {
        public PageControlResponse(int page, int maxPage, int count, int totalCount)
        {
            Page = page;
            MaxPage = maxPage;
            Count = count;
            TotalCount = totalCount;
        }
        public int Page { get; }
        public int MaxPage { get; }
        public int Count { get; }
        public int TotalCount { get; }
    }
}