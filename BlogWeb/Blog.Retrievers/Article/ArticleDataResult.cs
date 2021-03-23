using System;
using System.Data;

namespace Blog.Retrievers.Article
{
    public class ArticleDataResult
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public Guid? CoverFileId { get; set; }

        public ArticleDataResult()
        {

        }

        public ArticleDataResult(IDataReader dataReader)
            : this()
        {
            this.Id = (Guid)dataReader["Id"];
            this.Title = (string)dataReader["Title"];
            if (dataReader["CoverFileId"] != DBNull.Value)
                this.CoverFileId = (Guid)dataReader["CoverFileId"];
        }
    }
}
