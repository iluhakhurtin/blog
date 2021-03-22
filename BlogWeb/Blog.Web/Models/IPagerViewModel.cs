namespace Blog.Web.Models
{
    public interface IPagerViewModel
    {
        int TotalPagesCount { get; }
        int PageNumber { get; }
    }
}
