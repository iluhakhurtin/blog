using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;

namespace Blog.Web.Models.jqGrid
{
    /// <summary>
    /// Class returns result in jqGrid json reader format 
    /// </summary>
    /// <example>
    /// { 
    ///     total: "xxx", 
    ///     page: "yyy", 
    ///     records: "zzz",
    ///     rows : [
    ///         {id:"1", cell:["cell11", "cell12", "cell13"]},
    ///         {id:"2", cell:["cell21", "cell22", "cell23"]},
    ///         ...
    ///     ]
    /// }
    /// </example>
    /// <see cref="http://www.secondpersonplural.ca/jqgriddocs/_2eb0f6jhe.htm"/>
    [DataContract]
    public class jqGridResult
    {
        #region Fields and properties
        /// <summary>
        /// Gets or sets total pages for the query.
        /// </summary>
        /// <value>
        [DataMember]
        public int total { get; set; }

        /// <summary>
        /// Gets or sets current page of the query.
        /// </summary>
        [DataMember]
        public int page { get; set; }

        /// <summary>
        /// Gets or sets total number of records for the query.
        /// </summary>
        [DataMember]
        public int records { get; set; }


        /// <summary>
        /// Gets or sets an array that contains the actual data
        /// </summary>
        [DataMember]
        public List<jqGridRowResult> rows { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="jqGridResult"/> class from being created.
        /// </summary>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="page">The page.</param>
        /// <param name="records">The records.</param>
        private jqGridResult(int totalPages, int page, int records)
        {
            this.total = totalPages;
            this.page = page;
            this.records = records;
            this.rows = new List<jqGridRowResult>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="jqGridResult"/> class.
        /// </summary>
        /// <param name="totalPages">Total pages for the query.</param>
        /// <param name="page">Current page of the query.</param>
        /// <param name="records">Total number of records for the query.</param>
        /// <param name="data">Actual data.</param>
        /// <param name="identityProperty">Property name of identity property of the actual data items. 
        /// It will be row identifier</param>
        /// <param name="properties">Names of properties to serialize</param>
        public jqGridResult(int totalPages, int page, int records, IEnumerable<Object> data, string identityProperty, params string[] properties)
            : this(totalPages, page, records)
        {
            foreach (var item in data)
            {
                this.rows.Add(new jqGridRowResult(item, identityProperty, properties));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="jqGridResult"/> class.
        /// </summary>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="page">The page.</param>
        /// <param name="records">The records.</param>
        /// <param name="data">The data table to serialize.</param>
        /// <param name="identityProperty">The identity property.</param>
        public jqGridResult(int totalPages, int page, int records, DataTable data, string identityProperty)
            : this(totalPages, page, records)
        {
            List<string> columns = new List<string>();

            foreach (DataColumn column in data.Columns)
            {
                if (column.ColumnName == identityProperty)
                    continue;
                columns.Add(column.ColumnName);
            }

            foreach (DataRow dr in data.Rows)
            {
                this.rows.Add(new jqGridRowResult(dr, identityProperty, columns.ToArray()));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="jqGridResult"/> class.
        /// </summary>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="page">The page.</param>
        /// <param name="records">The records.</param>
        /// <param name="data">The data.</param>
        /// <param name="identityProperty">The identity property.</param>
        /// <param name="properties">The properties.</param>
        public jqGridResult(int totalPages, int page, int records, DataTable data, string identityProperty, params string[] properties)
            : this(totalPages, page, records)
        {
            foreach (DataRow item in data.Rows)
            {
                this.rows.Add(new jqGridRowResult(item, identityProperty, properties));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="jqGridResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="identityProperty">The identity property.</param>
        public jqGridResult(DataTable data, string identityProperty)
            : this(1, 0, 8, data, identityProperty)
        {
        }

        #endregion
    }


    /// <summary>
    /// Represents one row in jqGrid json data format
    /// </summary>
    /// <see cref="http://www.secondpersonplural.ca/jqgriddocs/_2eb0f6jhe.htm"/>
    [DataContract]
    public class jqGridRowResult
    {
        #region Fields and properties

        /// <summary>
        /// Gets the unique id of the row.
        /// </summary>
        [DataMember]
        public object id { get; private set; }

        /// <summary>
        /// Gets an array that contains the data for a row.
        /// </summary>
        [DataMember]
        public string[] cell { get; private set; }

        #endregion

        #region Constructors

        public jqGridRowResult()
        {
        }

        public jqGridRowResult(Object data, string identityProperty, params string[] properties)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.id = data.GetType().GetProperty(identityProperty).GetValue(data, null);
            //if no fields names for serialization, then use reflection and DataMemberAttribute
            if (properties.Length == 0)
                this.cell = this.GetDataFieldsValues(data).ToArray();
            else
                this.cell = this.GetDataFieldsValues(data, properties).ToArray();
        }

        public jqGridRowResult(DataRow dataRow, string identityColumn, params string[] columns)
        {
            if (dataRow == null)
                throw new ArgumentNullException("data");
            if (String.IsNullOrEmpty(identityColumn))
            {
                //user first column as identity
                this.id = dataRow[0];
            }
            else
            {
                //user specified column as identity
                this.id = dataRow[identityColumn];
            }
            if (columns.Length == 0)
            {
                //use all columns
                this.cell = this.GetDataRowValues(dataRow).ToArray();
            }
            else
            {
                this.cell = this.GetDataRowValues(dataRow, columns).ToArray();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data field values for fields that are marked with
        /// <typeparamref name="DataMemberAttribute"/>
        /// </summary>
        /// <exception cref="System.ArgumentNullException">If data is null</exception>
        /// <returns></returns>
        private List<string> GetDataFieldsValues(Object data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            List<string> result = new List<string>();
            if (data != null)
            {
                var props = data.GetType().GetProperties();
                foreach (var pi in props)
                {
                    if (pi.IsDefined(typeof(DataMemberAttribute), false))
                    {
                        result.Add(Convert.ToString(pi.GetValue(data, null)));
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Gets the data fields values.
        /// </summary>
        /// <param name="data">The data for serialization.</param>
        /// <param name="properies">Names of properties to serialize</param>
        /// <exception cref="System.ArgumentNullException">If data is null</exception>
        /// <returns></returns>
        private List<string> GetDataFieldsValues(Object data, string[] properies)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (properies == null)
                throw new ArgumentNullException("properies");

            List<string> result = new List<string>();
            Type dType = data.GetType();
            foreach (string p in properies)
            {
                PropertyInfo pi = dType.GetProperty(p);
                result.Add(Convert.ToString(pi.GetValue(data, null)));
            }
            return result;
        }

        /// <summary>
        /// Gets the data row values.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// dataRow
        /// or
        /// columns
        /// </exception>
        private List<string> GetDataRowValues(DataRow dataRow, string[] columns)
        {
            if (dataRow == null)
                throw new ArgumentNullException("dataRow");
            if (columns == null)
                throw new ArgumentNullException("columns");
            List<string> result = new List<string>();
            foreach (string col in columns)
            {
                result.Add(Convert.ToString(dataRow[col]));
            }
            return result;
        }

        /// <summary>
        /// Gets values of the row.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">dataRow</exception>
        private List<string> GetDataRowValues(DataRow dataRow)
        {
            if (dataRow == null)
                throw new ArgumentNullException("dataRow");
            List<string> result = new List<string>();
            foreach (object obj in dataRow.ItemArray)
            {
                result.Add(Convert.ToString(obj));
            }
            return result;
        }

        #endregion
    }
}
