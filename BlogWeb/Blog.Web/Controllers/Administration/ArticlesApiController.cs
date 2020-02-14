using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Web.Models.jqGrid;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.Controllers.Administration
{
    [Route("api/Administration/ArticlesApi")]
    public class ArticlesApiController : BaseApiAdministrationController
    {
        private readonly IArticlesRetriever articlesRetriever;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ArticlesApiController(
            ILog log,
            IRetrievers retrievers,
            RoleManager<ApplicationRole> roleManager)
            : base(log)
        {
            this.articlesRetriever = retrievers.ArticlesRetriever;
            this.roleManager = roleManager;
        }

        // GET: api/ArticlesApi
        [HttpGet]
        public async Task<jqGridResult> Get(
            bool _search,
            long nd,
            int page,
            int rows,
            string sidx,
            string sord,
            int id,
            string filters)
        {
            try
            {
                string title = null;
                string roles = null;
                string categories = null;

                if (!String.IsNullOrEmpty(filters))
                {
                    jqGridFilter filter = JsonConvert.DeserializeObject<jqGridFilter>(filters);
                    title = filter.GetFilterByFieldName(ArticlesPagedDataTable.Title);
                    roles = filter.GetFilterByFieldName(ArticlesPagedDataTable.Roles);
                    categories = filter.GetFilterByFieldName(ArticlesPagedDataTable.Categories);
                }

                ArticlesPagedDataTable pagedDataTable = await this.articlesRetriever.GetArticlesPagedAsync(
                    title,
                    roles,
                    categories,
                    sidx,
                    sord,
                    page,
                    rows);

                var result = new jqGridResult(
                    pagedDataTable.TotalPagesCount,
                    pagedDataTable.PageNumber,
                    pagedDataTable.Rows.Count,
                    pagedDataTable,
                    PagedDataTable.Id);

                return result;
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return null;
            }
        }

        // POST: api/ArticlesApi
        [HttpPost]
        public async Task<HttpResponseMessage> Post(
            [FromForm]string id,
            [FromForm]string oper,
            [FromForm]string Title,
            [FromForm]string Body,
            [FromForm]string Roles)
        {

            try
            {
                //switch (oper)
                //{
                //    case jqGridActions.Add:
                //        return await this.AddArticle(Email, Password, Roles);

                //    case jqGridActions.Edit:
                //        return await this.EditUser(id, Password, Roles);

                //    case jqGridActions.Delete:
                //        return await this.DeleteUser(id);
                //}
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.CreateErrorResponseMessage();
            }
            return base.CreateErrorResponseMessage("Not implemented");
        }

        //private async Task<HttpResponseMessage> AddUser(
        //    string email,
        //    string password,
        //    string roles)
        //{
        //    ApplicationUser user = await this.userManager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        var userDuplicationErrorResponseMessage = base.CreateErrorResponseMessage("The email is aready registered in the system.");
        //        return await Task.FromResult(userDuplicationErrorResponseMessage);
        //    }

        //    //validate the roles
        //    var parsedRoles = this.ParseRolesFromString(roles);
        //    string errorMessage = await this.ValidateRoles(parsedRoles);
        //    if (!String.IsNullOrEmpty(errorMessage))
        //    {
        //        var errorResponseMessage = base.CreateErrorResponseMessage(errorMessage);
        //        return await Task.FromResult(errorResponseMessage);
        //    }

        //    user = new ApplicationUser();
        //    user.Email = email;
        //    user.UserName = email;

        //    try
        //    {
        //        await this.userManager.CreateAsync(user, password);

        //        foreach (string role in parsedRoles)
        //        {
        //            await this.userManager.AddToRoleAsync(user, role);
        //        }

        //        var okResponseMessage = base.CreateOkResponseMessage();
        //        return await Task.FromResult(okResponseMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            await this.userManager.DeleteAsync(user);
        //        }
        //        finally
        //        {
        //            if (base.log.IsErrorEnabled)
        //                base.log.Error(ex);
        //        }
        //        return base.CreateErrorResponseMessage();
        //    }
        //}

        //private async Task<HttpResponseMessage> EditUser(
        //    string userId,
        //    string password,
        //    string roles)
        //{
        //    ApplicationUser user = await this.userManager.FindByIdAsync(userId);

        //    //validate the roles
        //    var parsedRoles = this.ParseRolesFromString(roles);
        //    string errorMessage = await this.ValidateRoles(parsedRoles);
        //    if (!String.IsNullOrEmpty(errorMessage))
        //    {
        //        var errorResponseMessage = base.CreateErrorResponseMessage(errorMessage);
        //        return await Task.FromResult(errorResponseMessage);
        //    }

        //    if (!String.IsNullOrEmpty(password))
        //    {
        //        string token = await this.userManager.GeneratePasswordResetTokenAsync(user);
        //        var passwordResult = await this.userManager.ResetPasswordAsync(user, token, password);
        //    }

        //    //remove user from all roles
        //    var currentRoles = await this.userManager.GetRolesAsync(user);
        //    var result = await this.userManager.RemoveFromRolesAsync(user, currentRoles);

        //    foreach (string role in parsedRoles)
        //    {
        //        await this.userManager.AddToRoleAsync(user, role);
        //    }

        //    var okResponseMessage = base.CreateOkResponseMessage();
        //    return await Task.FromResult(okResponseMessage);
        //}

        //private async Task<HttpResponseMessage> DeleteUser(string userId)
        //{
        //    ApplicationUser user = await this.userManager.FindByIdAsync(userId);
        //    HttpResponseMessage httpResponseMessage = base.CreateOkResponseMessage();
        //    try
        //    {
        //        var result = await this.userManager.DeleteAsync(user);
        //        if (!result.Succeeded)
        //            httpResponseMessage = base.CreateErrorResponseMessage();
        //    }
        //    catch(Exception ex)
        //    {
        //        httpResponseMessage = base.CreateErrorResponseMessage(ex.Message);
        //    }
        //    return await Task.FromResult(httpResponseMessage);
        //}

        private async Task<string> ValidateRoles(IEnumerable<string> roles)
        {
            foreach (string role in roles)
            {
                bool roleCheck = await this.roleManager.RoleExistsAsync(role);
                if (!roleCheck)
                {
                    string roleDoesNotExistResponseMessageText = String.Concat("The given role ", role, " does not exist in the system");
                    return roleDoesNotExistResponseMessageText;
                }
            }
            return String.Empty;
        }

        private IEnumerable<string> ParseRolesFromString(string rolesString)
        {
            char[] separators = new char[] { ',', ' ' };
            return rolesString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
