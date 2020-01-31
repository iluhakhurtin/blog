using System;
using System.Runtime.Serialization;

namespace Blog.Web.Models.jqGrid
{
    [DataContract]
    public class jqGridFilter
    {
        [DataMember]
        public string groupOp { get; set; }

        [DataMember]
        public jqGridRule[] rules { get; set; }

        [DataMember]
        public jqGridFilter[] groups { get; set; }



        public string GetFilterByFieldName(string fieldName)
        {
            string filterValue = this.GetFilterByFieldNameRecursive(this, fieldName);
            return filterValue;
        }

        private string GetFilterByFieldNameRecursive(jqGridFilter filter, string fieldName)
        {
            if (filter.groups != null)
            {
                foreach (jqGridFilter group in filter.groups)
                {
                    string filterValue = this.GetFilterByFieldNameRecursive(group, fieldName);
                    if (!String.IsNullOrEmpty(filterValue))
                        return filterValue;
                }
            }

            if (filter.rules != null)
            {
                foreach (jqGridRule rule in filter.rules)
                {
                    if (rule.field == fieldName)
                        return rule.data;
                }
            }

            return null;
        }
    }

    [DataContract]
    public class jqGridRule
    {
        [DataMember]
        public string field { get; set; }

        [DataMember]
        public string op { get; set; }

        [DataMember]
        public string data { get; set; }
    }
}
